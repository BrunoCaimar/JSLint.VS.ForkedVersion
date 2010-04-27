using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using EnvDTE;
using EnvDTE80;

namespace JSLint
{
    /// <summary>
    /// Class that holds references to Solution files needed to be checked and executes JSLint
    /// </summary>
    public class JSLintProcessor
    {
        // List of files for checking along with methods that expose it to other classes
        private List<FileDataHolder> _fileCheckList = new List<FileDataHolder>();
        public List<string> ActiveFileCheckList
        {
            get
            {
                List<string> active = new List<string>();
                foreach (FileDataHolder fdh in _fileCheckList)
                    active.Add(fdh.FilePath);

                return active;
            }
        }

        public void AddToFileCheckList(string filePath)
        {
            AddToFileCheckList(filePath, _fileCheckList);
        }

        internal void ClearFileCheckList()
        {
            _fileCheckList.Clear();
        }

        private void AddToFileCheckList(string filePath, List<FileDataHolder> collection)
        {
            foreach (FileDataHolder data in collection)
            {
                if (data.FilePath == filePath)
                {
                    return;
                }
            }

            collection.Add(new FileDataHolder(filePath));
        }

        private string _processResults = "";
        /// <summary>
        /// Method that executes JSLint over needed files/code fragments
        /// </summary>
        /// <param name="waitTime">Command timeout (in seconds), how much time to allow JSLint WSH process to run before it is killed. 
        /// Prevents blocking of Visual Studio for very large scripts.</param>
        /// <param name="checkOptions">List of JSLint options that are passed to checking engine</param>
        /// <param name="codeFragment">If code fragment is checked rather then files from Solution</param>
        /// <returns>List of errors JSLint found</returns>
        public List<string[]> PerformCheck(int waitTime, string checkOptions, string codeFragment)
        {
            StringBuilder forCheckingContent = new StringBuilder();
            
            // Check either defined files or code fragment
            if (string.IsNullOrEmpty(codeFragment))
            {
                if (_fileCheckList.Count == 0)
                {
                    return new List<string[]>();
                }

                foreach (FileDataHolder fdh in _fileCheckList)
                {
                    forCheckingContent.AppendFormat("{0}\r\n", fdh.FileContent);
                }
            }
            else
            {
                forCheckingContent.Append(codeFragment);
            }


            // Write files needed for linting to hard disk
            string folderRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string cshPath = string.Format(@"{0}\csh.cmd", folderRoot);
            string whsPath = string.Format(@"{0}\wsh.js", folderRoot);
            string tempPath = string.Format(@"{0}\temp.js", folderRoot);

            CreateFileIfNeeded(cshPath);

            // Write JSLint file with options that will be run by WSH
            CreateFileIfNeeded(whsPath, "/*[[0]]*/null", checkOptions);

            // Write joined file with JavaScripts that will be checked
            File.WriteAllText(tempPath, forCheckingContent.ToString());


            // Run WSH process
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            ProcessStartInfo psi = new ProcessStartInfo(string.Format("\"{0}\"", cshPath), 
                string.Format("\"{0}\" <\"{1}\"", whsPath, tempPath));
            p.StartInfo = psi;
            p.EnableRaisingEvents = false;
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;

            try
            {
                p.Start();

                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadProcessStream), p.StandardError);

                // Wait for process to finish, and kill if it's not finished in predefined time
                bool finished = p.WaitForExit(waitTime * 1000);
                if (!finished)
                {
                    p.Kill();
                    List<string[]> ret = new List<string[]>();
                    ret.Add(new string[] {"0", "", string.Format("JSLint.VS failed to finish processing in {0} seconds", waitTime),
                        "0", "0", "0", "0" });
                    return ret;
                }
            }
            finally
            {
                p.Close();
            }

            string[] errors = _processResults.Split('\n');

            // Parse results
            int lineStart = 1;
            int checkingIndex = 0;
            List<FileDataHolder> newFileCheckingList = new List<FileDataHolder>();
            List<string[]> errorsToReturn = new List<string[]>();
            foreach (string errorInfo in errors)
            {
                if (string.IsNullOrEmpty(errorInfo))
                {
                    continue;
                }

                try
                {
                    // JSLint was run over one big concatenated file, so it is needed to 
                    // distribute checking results to single files
                    int lineNo = int.Parse(errorInfo.Split('\t')[0]);
                    string errorText = errorInfo.Split('\t')[2];

                    FileDataHolder fileData = _fileCheckList[checkingIndex];

                    int currentFileLinesNo = fileData.TotalLines;

                    while (lineNo - lineStart > currentFileLinesNo)
                    {
                        lineStart += currentFileLinesNo;
                        checkingIndex++;

                        fileData = _fileCheckList[checkingIndex];
                        currentFileLinesNo = fileData.TotalLines;
                    }

                    int relativeLineNo = lineNo - lineStart + 1;

                    AddToFileCheckList(fileData.FilePath, newFileCheckingList);

                    //string errorInfoWithFile = string.Format("{0}\t{1}\t{2}", errorInfo, 
                    //    fileData.FilePath, relativeLineNo);
                    errorsToReturn.Add(new string[] { errorText, fileData.FilePath, relativeLineNo.ToString() });
                }
                catch (Exception ex)
                {
                    // Block used for debugging of exceptions raised during parsing
                }
            }

            _fileCheckList = newFileCheckingList;
            return errorsToReturn;
        }

        /// <summary>
        /// Copy file from resources to hard disk if it does not exist there
        /// </summary>
        /// <param name="filePath">Path on which to save file</param>
        private static void CreateFileIfNeeded(string filePath)
        {
            CreateFileIfNeeded(filePath, null, null);
        }

        /// <summary>
        /// Copy file from resources to hard disk if it does not exist there and replace some it's content
        /// </summary>
        /// <param name="filePath">Path on which to save file</param>
        /// <param name="replacePattern">Value to find</param>
        /// <param name="replaceWithValue">Value to replace with</param>
        private static void CreateFileIfNeeded(string filePath, string replacePattern, string replaceWithValue)
        {
            if (!File.Exists(filePath) || !string.IsNullOrEmpty(replacePattern))
            {
                string fileName = new FileInfo(filePath).Name;
                string ns = typeof(OptionsForm).Namespace;

                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    string.Format("{0}.JSLintFiles.{1}", ns, fileName))))
                {
                    StringBuilder sb = new StringBuilder(sr.ReadToEnd());
                    if (!string.IsNullOrEmpty(replacePattern))
                    {
                        sb.Replace(replacePattern, replaceWithValue);
                    }

                    File.WriteAllText(filePath, sb.ToString());
                }
            }
        }

        /// <summary>
        /// Used to read results from WSH process
        /// </summary>
        /// <param name="stream">Result stream from WSH process</param>
        private void ReadProcessStream(object stream)
        {
            using (StreamReader sr = (StreamReader)stream)
            {
                lock (_processResults)
                {
                    _processResults = sr.ReadToEnd();
                }
            }
        }
    }

    /// <summary>
    /// Helper class that holds information about file needed to be checked by JSLint
    /// </summary>
    class FileDataHolder
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private string _fileContent;
        public string FileContent
        {
            get { return _fileContent; }
        }

        private int _totalLines;
        public int TotalLines
        {
            get { return _totalLines; }
        }

        public FileDataHolder(string filePath)
        {
            FilePath = filePath;
            _fileContent = File.ReadAllText(filePath);

            int hasExtraLine = (FileContent.EndsWith("\r\n") || FileContent.EndsWith("\n") ? 1 : 0);
            _totalLines = File.ReadAllLines(filePath).Length + hasExtraLine;
        }
    }
}