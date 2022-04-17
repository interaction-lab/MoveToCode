using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class LoggingManager : Singleton<LoggingManager> {

        [HideInInspector]
        public bool logData = false;
        bool initialized = false;
        Dictionary<string, int> columnLookup;
        List<string> row;
        StreamWriter streamWriter;

        string csvFilename;
        string filePath;

        private void Awake() {
            Init();
        }

        void Init() {
            if (initialized) {
                return;
            }
            else {
                initialized = true;
            }
#if !UNITY_EDITOR
            logData = true;
#endif
            if (logData) {
                Debug.Log("Currently logging data: " + logData.ToString());
                csvFilename = System.DateTime.Now.ToString().Replace(' ', '_').Replace('\\', '_').Replace('/', '_').Replace(':', '-') + "_" + UserIDManager.PlayerId + ".csv";
                filePath = Path.Combine(Application.persistentDataPath, csvFilename);
                Debug.Log(filePath);
                streamWriter = new StreamWriter(new FileStream(filePath, FileMode.Create));
            }
            else {
                Debug.LogWarning("NOT LOGGING DATA, data is autologged when deployed to the Hololens 2 but not by default for the Unity editor. If you want logging, check the \"logData\" public box of the LoggingManager component");
            }
        }

        public Dictionary<string, int> GetColumnLookUp() {
            if (columnLookup == null) {
                columnLookup = new Dictionary<string, int>();
            }
            return columnLookup;
        }

        public List<string> GetRow() {
            if (row == null) {
                row = new List<string>();
            }
            return row;
        }

        /// <summary>
        /// Adds key to rowLookup, updates the value
        /// </summary>
        /// <param name="key">Column Name</param>
        /// <param name="value">Value for Column</param>
        /// <returns>True if key not already in row, false otherwise</returns>
        public bool AddLogColumn(string key, string value) {
            if (!initialized) {
                Init(); // jank protection against init order, need to fix singleton init order
            }
            bool isNewColumn = !GetColumnLookUp().ContainsKey(key);
            if (isNewColumn) {
                columnLookup[key] = GetRow().Count;
                row.Add(value);
            }
            UpdateLogColumn(key, value);
            return isNewColumn;
        }

        public void UpdateLogColumn(string key, string value) {
            row[columnLookup[key]] = CleanCSVString(value);
            Assert.AreEqual(columnLookup.Keys.Count, row.Count);
        }

        private static string CleanCSVString(string value) {
            if (value.Contains("\"")) {
                value = value.Replace("\"", "\"\"");
                value = string.Format("\"{0}\"", value);
            }
            else if (value.Contains(",") || value.Contains(System.Environment.NewLine)) {
                value = string.Format("\"{0}\"", value);
            }
            return value;
        }

        public string GetValueInRowAt(string key) {
            return row[columnLookup[key]];
        }

        void ResetRow() {
            for (int i = 0; i < row.Count; ++i) {
                row[i] = "";
            }
        }

        void WriteRowToCSV() {
            HumanStateManager.instance.UpdateKC();
            if (!logData) {
                return;
            }
            WriteValuesIfStreamWriterOpen();
            ResetRow();
        }

        /// <summary>
        /// Writes the values in row of CSV file if StreamWriter is open
        /// </summary>
        private void WriteValuesIfStreamWriterOpen() {
            List<string> rowDuplicate = new List<string>(row);
            if (StreamWriterIsOpen()) {
                streamWriter.WriteLine(string.Join(",", Time.time.ToString(), string.Join(",", rowDuplicate)));
            }
        }

        private void FixedUpdate() {
            WriteRowToCSV();
        }

        public void FinishLogging(bool hasQuit = false) {
            // Added StreamWriter condition for uploading CSV
            if (!logData || !StreamWriterIsOpen()) {
                return;
            }
            WriteKeysIfStreamWriterOpen();
            Debug.Log("Logged data to: " + filePath);

        }

        /// <summary>
        /// Writes the keys in the bottom columns of CSV file before uploading
        /// </summary>
        private void WriteKeysIfStreamWriterOpen() {
            var ordered = columnLookup.OrderBy(x => x.Value);
            List<string> columnNames = new List<string>();
            foreach (var pairKeyVal in ordered) {
                columnNames.Add(pairKeyVal.Key);
            }
            if (StreamWriterIsOpen()) {
                streamWriter.WriteLine(string.Join(",", "Time", string.Join(",", columnNames)));
                streamWriter.Close();
            }
        }

        // Added helper function for uploading CSV to check that streamWriter is open 
        /// <summary>
        /// Checks if streamWriter is open when uploading CSV file and returns the result
        /// </summary>
        /// <returns>
        /// True if streamWriter is open; otherwise, False if null
        /// </returns>
        private bool StreamWriterIsOpen() {
            return streamWriter.BaseStream != null;
        }

        // Added helper function that returns file path for uploading CSV
        /// <summary>
        /// Retrieves the file path to CSV file that will be uploaded
        /// </summary>
        /// <returns>
        /// File path to the CSV file to be uploaded
        /// </returns>
        public string getFilePath() {
            return filePath;
        }

        // Added helper function that returns csvFilename for uploading CSV
        /// <summary>
        /// Retrieves the file name of CSV file that will be uploaded
        /// </summary>
        /// <returns>
        /// File name of the CSV file to be uploaded
        /// </returns>
        public string getCSVFileName() {
            return csvFilename;
        }

        // Write out columns, will be at end of file
        void OnApplicationQuit() {
            // Commented out line below for uploading CSV
            //   FinishLogging(true); 
        }
    }
}