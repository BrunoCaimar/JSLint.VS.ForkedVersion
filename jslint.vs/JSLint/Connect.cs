using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Util.AddIns;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace JSLint
{
    /// <summary>
    /// Main Add-In class that registers menus and handles events from Visual Studio IDE
    /// </summary>
	public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        #region Connection & Events
        public Connect()
		{
		}

		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
            if (!_initialized)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;
                string toolsMenuName;

                try
                {
                    //If you would like to move the command to a different menu, change the word "Tools" to the 
                    //  English version of the menu. This code will take the culture, append on the name of the menu
                    //  then add the command to that menu. You can find a list of all the top-level menus in the file
                    //  CommandBar.resx.
                    ResourceManager resourceManager = new ResourceManager("JSLint.CommandBar", Assembly.GetExecutingAssembly());
                    CultureInfo cultureInfo = new System.Globalization.CultureInfo(_applicationObject.LocaleID);
                    string resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, "Tools");
                    toolsMenuName = resourceManager.GetString(resourceName);
                }
                catch
                {
                    //We tried to find a localized version of the word Tools, but one was not found.
                    //  Default to the en-US word, which may work for the current culture.
                    toolsMenuName = "Tools";
                }

                //Place the command on the tools menu.
                //Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
                Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"];

                //Find the Tools command bar on the MenuBar command bar:
                CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
                CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;



                // JSLint.VS initialization logic
                _usefulFunctions = new UsefulFunctions(_applicationObject, _addInInstance);

                try
                {
                    //Add a command to the Commands collection:
                    Command command = commands.AddNamedCommand2(_addInInstance, "JSLintOptions", "JSLint.VS Options", "Set JSLint.VS Options", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);

                    //Add a control for the command to the tools menu:
                    if ((command != null) && (toolsPopup != null))
                    {
                        command.AddControl(toolsPopup.CommandBar, 1);
                    }
                }
                catch (System.ArgumentException)
                {
                    //If we are here, then the exception is probably because a command with that name
                    //  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
                }

                try
                {
                    Command cmdLintFile = commands.AddNamedCommand2(_addInInstance, "JSLintFile", "JSLint File",
                        "Use JSLint.VS on this file", true, 59, ref contextGUIDS,
                        (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                        (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    CommandBar cb = _usefulFunctions.GetCommandBar("Item", null);
                    cmdLintFile.AddControl(cb, cb.accChildCount + 1);
                }
                catch (System.ArgumentException)
                { }

                try
                {
                    // Create commands and add them to menus
                    Command cmdLintSection = commands.AddNamedCommand2(_addInInstance, "JSLintSection", "JSLint selected fragment",
                        "Use JSLint.VS on this code fragment", true, 59, ref contextGUIDS,
                        (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                        (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    CommandBar cb = _usefulFunctions.GetCommandBar("HTML Context", null);
                    cmdLintSection.AddControl(cb, cb.accChildCount + 1);

                    cb = _usefulFunctions.GetCommandBar("Script Context", null);
                    cmdLintSection.AddControl(cb, cb.accChildCount + 1);

                    cb = _usefulFunctions.GetCommandBar("ASPX Context", null);
                    cmdLintSection.AddControl(cb, cb.accChildCount + 1);
                }
                catch (System.ArgumentException)
                { }

                _initialized = true;
            }

            // create variables that point to event collections so that 
            // subscriptions that follow are not destroyed up by Garbage Collector
            _textEditorEvents = _applicationObject.Events.get_TextEditorEvents(null);
            _buildEvents = _applicationObject.Events.BuildEvents;
            _solutionEvents = _applicationObject.Events.SolutionEvents;

            // subscribe to events
            _textEditorEvents.LineChanged += new _dispTextEditorEvents_LineChangedEventHandler(_textEditorEvents_LineChanged);
            _buildEvents.OnBuildBegin += new _dispBuildEvents_OnBuildBeginEventHandler(_buildEvents_OnBuildBegin);
            _buildEvents.OnBuildProjConfigBegin += new _dispBuildEvents_OnBuildProjConfigBeginEventHandler(_buildEvents_OnBuildProjConfigBegin);
            _buildEvents.OnBuildDone += new _dispBuildEvents_OnBuildDoneEventHandler(_buildEvents_OnBuildDone);
            _solutionEvents.Opened += new _dispSolutionEvents_OpenedEventHandler(_solutionEvents_Opened);
            _solutionEvents.AfterClosing += new _dispSolutionEvents_AfterClosingEventHandler(_solutionEvents_AfterClosing);

            _optionsForm.SelectedSolutionItemsChanged += new System.Windows.Forms.MethodInvoker(_optionsForm_SelectedSolutionItemsChanged);
        }

        // Event handlers
        void _optionsForm_SelectedSolutionItemsChanged()
        {
            InitialFileChangedList();
        }

        void _solutionEvents_AfterClosing()
        {
            _optionsForm.ActiveSln = null;
            _checkList.ClearFileCheckList();
        }

        void _solutionEvents_Opened()
        {
            _optionsForm.ActiveSln = _applicationObject.Solution;
            InitialFileChangedList();
        }

        private bool _buildChecked = false;
        void _buildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            if (_buildChecked)
                return;

            _buildChecked = true;

            _checkList.ClearFileCheckList();
            foreach (string s in _filesToCheckWhenBuilding)
            {
                _checkList.AddToFileCheckList(s);
            }

            PerformProjectCheck();

            _filesToCheckWhenBuilding = _checkList.ActiveFileCheckList;
        }

        void _buildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            _buildChecked = false;
        }

        void _buildEvents_OnBuildProjConfigBegin(string Project, string ProjectConfig, string Platform, string SolutionConfig)
        {
        }

        void _textEditorEvents_LineChanged(TextPoint StartPoint, TextPoint EndPoint, int Hint)
        {
            string fullFileName = _applicationObject.ActiveDocument.FullName;
            
            if (IsValidCheckListFile(fullFileName))
                _filesToCheckWhenBuilding.Add(fullFileName);
        }

		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}
	
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		public void OnStartupComplete(ref Array custom)
		{
		}

		public void OnBeginShutdown(ref Array custom)
		{
		}

        /// <summary>
        /// Code that determines whenever or not menu item should be displayed/enabled
        /// </summary>
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
                bool commandEnabled = false;
                if (commandName == "JSLint.Connect.JSLintOptions")
				{
                    commandEnabled = true;
				}
                else if (commandName == "JSLint.Connect.JSLintFile")
                {
                    UIHierarchy UIH = (UIHierarchy)_applicationObject.ToolWindows.GetToolWindow(Constants.vsWindowKindSolutionExplorer);
                    UIHierarchyItem item = (UIHierarchyItem)((System.Array)UIH.SelectedItems).GetValue(0);

                    commandEnabled = item.Name.EndsWith(".js");
                }
                else if (commandName == "JSLint.Connect.JSLintSection")
                {
                    commandEnabled = _usefulFunctions.TextRead().Length > 0;
                }


                status = vsCommandStatus.vsCommandStatusSupported;
                if (commandEnabled)
                    status |= vsCommandStatus.vsCommandStatusEnabled;
                else
                    status |= vsCommandStatus.vsCommandStatusInvisible;
			}
		}

        /// <summary>
        /// Code that handles clicks on menu items registred in OnConnect method
        /// </summary>
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if(commandName == "JSLint.Connect.JSLintOptions")
				{
                    Form dummyOwnerForm = new Form();
                    try
                    {
                        _optionsForm.ShowDialog(dummyOwnerForm);
                    }
                    catch
                    {
                    }

					handled = true;
                }
                else if (commandName == "JSLint.Connect.JSLintFile")
                {
                    UIHierarchy UIH = (UIHierarchy)_applicationObject.ToolWindows.GetToolWindow(Constants.vsWindowKindSolutionExplorer);
                    UIHierarchyItem item = (UIHierarchyItem)((System.Array)UIH.SelectedItems).GetValue(0);
                    ProjectItem projItem = (ProjectItem)item.Object;

                    _checkList.ClearFileCheckList();
                    _checkList.AddToFileCheckList(projItem.get_FileNames(1));
                    PerformProjectCheck();

                    handled = true;
                }
                else if (commandName == "JSLint.Connect.JSLintSection")
                {
                    TextSelection ts = _usefulFunctions.TextDocument().Selection;

                    StringBuilder sb = new StringBuilder();
                    for(int i = 0 ; i < ts.TopLine-1; i++)
                        sb.Append("\r\n");

                    sb.Append(ts.Text);

                    _checkList.ClearFileCheckList();
                    _checkList.AddToFileCheckList(_applicationObject.ActiveDocument.FullName);
                    PerformProjectCheck(sb.ToString());

                    handled = true;
                }
			}
        }
        #endregion

        private DTE2 _applicationObject;
		private AddIn _addInInstance;

        // JSLint.VS attributes
        private static bool _initialized = false;
        private static UsefulFunctions _usefulFunctions;

        private TextEditorEvents _textEditorEvents;
        private BuildEvents _buildEvents;
        private SolutionEvents _solutionEvents;
        private static OptionsForm _optionsForm = new OptionsForm();
        private JSLintProcessor _checkList = new JSLintProcessor();
        private List<string> _filesToCheckWhenBuilding = new List<string>();

        // JSLint.VS methods

        /// <summary>
        /// Builds initial list of files that should be checked when Solution is build
        /// When Solution is loaded up for the first time this method gets all .js files
        /// </summary>
        private void InitialFileChangedList()
        {
            _checkList.ClearFileCheckList();

            foreach (Project p in _applicationObject.Solution.Projects)
            {
                if (p.ProjectItems != null)
                {
                    InitialFileChangedList(p.ProjectItems);
                }
            }
        }

        /// <summary>
        /// Recursive method that builds list of files that are checked by JSLint.VS.
        /// Used by InitialFileChangedList()
        /// </summary>
        /// <param name="projectItems"></param>
        private void InitialFileChangedList(ProjectItems projectItems)
        {
            foreach (ProjectItem pi in projectItems)
            {
                if (pi.ProjectItems == null)
                {
                    continue;
                }

                if (pi.ProjectItems.Count > 0)
                {
                    InitialFileChangedList(pi.ProjectItems);
                }
                else
                {
                    string itemFullName = pi.get_FileNames(1);
                    if (IsValidCheckListFile(itemFullName))
                        _filesToCheckWhenBuilding.Add(itemFullName);
                }
            }
        }

        /// <summary>
        /// Checks whenever file is valid target for JSLint
        /// </summary>
        /// <param name="fullFileName">Full path to file</param>
        /// <returns>true/false</returns>
        private bool IsValidCheckListFile(string fullFileName)
        {
            if (_optionsForm.SelectedSolutionItems != null)
            {
                if (!_optionsForm.SelectedSolutionItems.Contains(fullFileName))
                {
                    return false;
                }
            }

            return fullFileName.EndsWith(".js");                
        }

        /// <summary>
        /// Overload that invokes PerformProjectCheck using files added previously to _checkList
        /// </summary>
        private void PerformProjectCheck()
        {
            PerformProjectCheck(null);
        }

        /// <summary>
        /// Performs JSLint.VS checks
        /// </summary>
        /// <param name="codeFragment">Pass string if code fragment rather then files from list should be checked</param>
        private void PerformProjectCheck(string codeFragment)
        {
            _applicationObject.StatusBar.Text = "JSLinting...";

            List<string[]> errors = _checkList.PerformCheck(
                _optionsForm.JSLintProcessWaitTime, _optionsForm.ActiveOptionsJSON, codeFragment);

            // Populate TaskList with errors
            TaskList list = _applicationObject.ToolWindows.TaskList;
            TaskItems2 ti = (TaskItems2)list.TaskItems;
            
            foreach (TaskItem item in ti)
                item.Delete();

            foreach (string[] errorInfo in errors)
            {
                try
                {
                    TaskItems2 items = (TaskItems2)list.TaskItems;
                    items.Add2("JSLint", "", errorInfo[0], (int)vsTaskPriority.vsTaskPriorityMedium, vsTaskIcon.vsTaskIconCompile, false, errorInfo[1], int.Parse(errorInfo[2]), true, false, true);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("JSLint.VS", 
                                        string.Format("{0} {1} StackTrace:{2}", 
                                                      ex.Message, 
                                                      Environment.NewLine, 
                                                      ex.StackTrace));
                }
            }

            list.TaskItems.ForceItemsToTaskList();

            // If there are errors show TaskList and display appropriate message in status bar
            if (errors.Count > 0)
            {
                _applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList).Activate();

                object customIn = "Add-ins and Macros";
                object customOut = null;
                _applicationObject.Commands.Raise(
                    "{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}", 2200, ref customIn, ref customOut);

                _applicationObject.StatusBar.Text = "JSLint.VS found errors";

                if (_optionsForm.StopBuildOnErrors)
                    _applicationObject.ExecuteCommand("Build.Cancel", "");
            }
            else
            {
                _applicationObject.StatusBar.Text = "JSLint.VS finished with no found errors";
            }
        }
	}
}