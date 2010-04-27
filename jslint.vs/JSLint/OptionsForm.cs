using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using EnvDTE;
using JSLint.Properties;
using System.Collections.Specialized;

namespace JSLint
{
    /// <summary>
    /// Form that enables user to tune JSLint options and choose files that are checked
    /// </summary>
    public partial class OptionsForm : Form
    {
        private const char SPLIT_CHAR = '|';

        public OptionsForm()
        {
            InitializeComponent();
            InitJSLintOptions();

            groupIntegrateWithBuild.DataBindings.Add("Enabled", chkIntegrateWithBuild, "Checked");
            treeSolution.DataBindings.Add("Enabled", chkScheckingScope, "Checked");

            if (ActiveSln == null)
            {
                groupCheckingScope.Enabled = false;
            }

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                Deserialize();

                this.chkIntegrateWithBuild.CheckedChanged += new System.EventHandler(this.chkIntegrateWithBuild_CheckedChanged);
                this.treeSolution.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeSolution_AfterCheck);
                this.chkScheckingScope.CheckedChanged += new System.EventHandler(this.chkScheckingScope_CheckedChanged);
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DesignMode)
                Serialize();
        }

        // Deserialze options from hard disk
        private void Deserialize()
        {
            IntegrateWithBuild = Settings.Default.IntegrateWithBuild;
            StopBuildOnErrors = Settings.Default.StopBuildOnErrors;
            CheckItemsInString(Settings.Default.SelectedOptions);
            DontCheckWholeSolution = Settings.Default.DontCheckWholeSolution;
            JSLintProcessWaitTime = Settings.Default.JSLintProcessWaitTime;

            DeserializeSolutionCheckingScope();
        }

        private void DeserializeSolutionCheckingScope()
        {
            if (ActiveSln != null)
            {
                afterCheckInProcess = true;

                Dictionary<string, string> solutionCheckingScope = new Dictionary<string, string>();
                DictionarySerializer.Deserialize(Settings.Default.CheckingScope, solutionCheckingScope);

                string searchKey = ActiveSln.FullName;
                if (solutionCheckingScope.ContainsKey(searchKey))
                {
                    SetSelectedSolutionItems(treeSolution.Nodes,
                        new List<string>(solutionCheckingScope[searchKey].Split(SPLIT_CHAR)));
                }

                afterCheckInProcess = false;
            }
        }

        private void SetSelectedSolutionItems(TreeNodeCollection treeNodeCollection, List<string> selectedNodes)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                SetSelectedSolutionItems(node.Nodes, selectedNodes);

                if (selectedNodes.Contains(node.FullPath))
                    node.Checked = true;
            }
        }

        // Serialize options to hard disk
        private void Serialize()
        {
            Settings.Default.IntegrateWithBuild = IntegrateWithBuild;
            Settings.Default.StopBuildOnErrors = StopBuildOnErrors;
            Settings.Default.SelectedOptions = ActiveOptionsString;
            Settings.Default.DontCheckWholeSolution = DontCheckWholeSolution;
            Settings.Default.JSLintProcessWaitTime = JSLintProcessWaitTime;

            SerializeSolutionCheckingScope();

            Settings.Default.Save();
        }

        private void SerializeSolutionCheckingScope()
        {
            if (ActiveSln != null)
            {
                Dictionary<string, string> solutionCheckingScope = new Dictionary<string, string>();
                DictionarySerializer.Deserialize(Settings.Default.CheckingScope, solutionCheckingScope);

                string searchKey = ActiveSln.FullName;
                if (!solutionCheckingScope.ContainsKey(searchKey))
                {
                    solutionCheckingScope.Add(searchKey, null);
                }

                List<string> selectedNodes = new List<string>();
                solutionCheckingScope[searchKey] = string.Join(SPLIT_CHAR.ToString(),
                    GetSelectedSolutionItems(treeSolution.Nodes, selectedNodes, false).ToArray());

                Settings.Default.CheckingScope = DictionarySerializer.Serialize(solutionCheckingScope);
            }
        }

        private List<string> GetSelectedSolutionItems(TreeNodeCollection treeNodeCollection, List<string> selectedNodes, bool filePath)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                GetSelectedSolutionItems(node.Nodes, selectedNodes, filePath);

                if (node.Checked)
                {
                    if (filePath)
                    {
                        selectedNodes.Add(node.Name);
                    }
                    else
                    {
                        selectedNodes.Add(node.FullPath);
                    }
                }
            }

            return selectedNodes;
        }

        // JSLint options
        private XmlDocument _xdJsLint = null;
        private void InitJSLintOptions()
        {
            _xdJsLint = new XmlDocument();
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                string.Format("{0}.JSLintOptions.xml", typeof(OptionsForm).Namespace))))
            {
                _xdJsLint.Load(sr);
            }

            chkListJSLintOptions.Items.Clear();
            foreach (XmlNode node in _xdJsLint.DocumentElement.ChildNodes)
            {
                chkListJSLintOptions.Items.Add(new JSLintOptionsItem(
                    node.Attributes["id"].Value, node.Attributes["name"].Value), false);
            }
        }

        public string ActiveOptionsJSON
        {
            get
            {
                List<string> options = new List<string>();
                for (int i = 0 ; i < chkListJSLintOptions.Items.Count; i++)
                {
                    JSLintOptionsItem oi = (JSLintOptionsItem)chkListJSLintOptions.Items[i]; 
                    options.Add(string.Format("\"{0}\" : {1}",
                        oi.Id, chkListJSLintOptions.GetItemChecked(i).ToString().ToLower()));
                }

                return "{" + string.Join(",", options.ToArray()) + "};";
            }
        }

        public string ActiveOptionsString
        {
            get
            {
                List<string> options = new List<string>();
                for (int i = 0; i < chkListJSLintOptions.Items.Count; i++)
                {
                    if (chkListJSLintOptions.GetItemChecked(i))
                    {
                        JSLintOptionsItem oi = (JSLintOptionsItem)chkListJSLintOptions.Items[i];
                        options.Add(oi.Id);
                    }
                }

                return string.Join(",", options.ToArray());
            }
            set
            {
                CheckItemsInString(value);
            }
        }

        // Options related to integrating JSLint with Visual Studio Build command
        private Solution _activeSln;
        public Solution ActiveSln
        {
            get { return _activeSln; }
            set
            {
                _activeSln = value;
                treeSolution.Nodes.Clear();

                groupCheckingScope.Enabled = (_activeSln != null);

                if (_activeSln != null)
                {
                    BuildTreeSolution(_activeSln);
                    DeserializeSolutionCheckingScope();
                }
            }
        }

        private void BuildTreeSolution(Solution activeSln)
        {
            foreach (Project project in activeSln.Projects)
            {
                if (project.ProjectItems != null)
                {
                    IterateProjectItems(project.ProjectItems, treeSolution.Nodes.Add(project.FullName, project.Name));
                }
            }
        }

        public void IterateProjectItems(ProjectItems projectItems, TreeNode node)
        {
            foreach (ProjectItem pi in projectItems)
            {
                if (pi.ProjectItems == null)
                {
                    continue;
                }

                if (pi.Kind != Constants.vsProjectItemKindPhysicalFile && pi.ProjectItems.Count > 0)
                {
                    TreeNode newNode = node.Nodes.Add(pi.get_FileNames(1), pi.Name);
                    IterateProjectItems(pi.ProjectItems, newNode);
                }
            }

            foreach (ProjectItem pi in projectItems)
            {
                if (pi.ProjectItems == null)
                {
                    continue;
                }

                if (pi.Kind == Constants.vsProjectItemKindPhysicalFile && pi.ProjectItems.Count == 0)
                {
                    TreeNode newNode = node.Nodes.Add(pi.get_FileNames(1), pi.Name);
                    IterateProjectItems(pi.ProjectItems, newNode);
                }
            }
        }

        public bool IntegrateWithBuild
        {
            get { return chkIntegrateWithBuild.Checked; }
            set { chkIntegrateWithBuild.Checked = value; }
        }

        public bool StopBuildOnErrors
        {
            get { return chkStopBuildOnErrors.Checked; }
            set { chkStopBuildOnErrors.Checked = value; }
        }

        public int JSLintProcessWaitTime
        {
            get { return (int)numJSLintProcessWaitTime.Value; }
            set { numJSLintProcessWaitTime.Value = value; }
        }

        public bool DontCheckWholeSolution
        {
            get { return chkScheckingScope.Checked; }
            set { chkScheckingScope.Checked = value; }
        }

        // Event handlers
        private void btnClearJSLintOptions_Click(object sender, EventArgs e)
        {
            CheckItemsInString("");
        }

        private void btnRecommendedJSLintOptions_Click(object sender, EventArgs e)
        {
            CheckItemsInString(_xdJsLint.DocumentElement.Attributes["recommended"].Value);
        }

        private void btnGoodJavaScriptParts_Click(object sender, EventArgs e)
        {
            CheckItemsInString(_xdJsLint.DocumentElement.Attributes["goodparts"].Value);
        }

        private void CheckItemsInString(string itemsToCheck)
        {
            for (int i = 0; i < chkListJSLintOptions.Items.Count; i++)
            {
                JSLintOptionsItem oi = (JSLintOptionsItem)chkListJSLintOptions.Items[i];
                chkListJSLintOptions.SetItemChecked(i, itemsToCheck.IndexOf(oi.Id) > -1);
            }
        }

        private void chkIntegrateWithBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkIntegrateWithBuild.Checked)
                chkStopBuildOnErrors.Checked = false;
        }

        private bool afterCheckInProcess = false;
        private void treeSolution_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (afterCheckInProcess)
                return;
            else
                afterCheckInProcess = true;

            _cachedSelectedSolutionItems = null;
            IterateChildNodes(e.Node.Nodes, e.Node.Checked);
            RaiseSelectedSolutionItemsChanged();

            afterCheckInProcess = false;
        }

        private void IterateChildNodes(TreeNodeCollection treeNodeCollection, bool toCheck)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                IterateChildNodes(node.Nodes, toCheck);
                node.Checked = toCheck;
            }
        }

        private void chkScheckingScope_CheckedChanged(object sender, EventArgs e)
        {
            RaiseSelectedSolutionItemsChanged();
        }

        public event MethodInvoker SelectedSolutionItemsChanged;
        private void RaiseSelectedSolutionItemsChanged()
        {
            if (SelectedSolutionItemsChanged != null)
                SelectedSolutionItemsChanged();
        }

        private List<string> _cachedSelectedSolutionItems = null;
        public List<string> SelectedSolutionItems
        {
            get
            {
                if (!chkScheckingScope.Checked)
                {
                    return null;
                }

                if (_cachedSelectedSolutionItems == null)
                {
                    _cachedSelectedSolutionItems = 
                        GetSelectedSolutionItems(treeSolution.Nodes, new List<string>(), true);
                }

                return _cachedSelectedSolutionItems;
            }
        }

        private void groupIntegrateWithBuild_EnabledChanged(object sender, EventArgs e)
        {
            chkIntegrateWithBuild.Enabled = true;
        }
    }

    /// <summary>
    /// Helper class that holds JSLint options id and name (initialized from JSLintOptions.xml)
    /// </summary>
    class JSLintOptionsItem
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public JSLintOptionsItem(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}