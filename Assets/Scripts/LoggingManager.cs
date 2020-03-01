using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class LoggingManager : Singleton<LoggingManager> {

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
#if WINDOWS_UWP
            logData = true;
#endif
            if (logData) {
                Debug.Log("Currently logging data: " + logData.ToString());
                csvFilename = System.DateTime.Now.ToString().Replace(' ', '_').Replace('\\', '_').Replace('/', '_').Replace(':', '-') + ".csv";
                filePath = Path.Combine(Application.persistentDataPath, csvFilename);
                Debug.Log(filePath);
                streamWriter = new StreamWriter(new FileStream(filePath, FileMode.Create)); ;
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
            bool isNewColumn = !GetColumnLookUp().ContainsKey(key);
            if (isNewColumn) {
                columnLookup[key] = GetRow().Count;
                row.Add(value);
            }
            UpdateLogColumn(key, value);
            return isNewColumn;
        }

        public void UpdateLogColumn(string key, string value) {
            row[columnLookup[key]] = value;
            Assert.AreEqual(columnLookup.Keys.Count, row.Count);
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
            if (!logData) {
                return;
            }
            HumanStateManager.instance.UpdateKC(); // TODO: this is hack
            streamWriter.WriteLine(string.Join(",", Time.time.ToString(), string.Join(",", row)));
            ResetRow();
        }

        private void FixedUpdate() {
            WriteRowToCSV();
        }

        public void FinishLogging(bool hasQuit = false) {
            if (!logData) {
                return;
            }
            var ordered = columnLookup.OrderBy(x => x.Value);
            List<string> columnNames = new List<string>();
            foreach (var pairKeyVal in ordered) {
                columnNames.Add(pairKeyVal.Key);
            }

            streamWriter.WriteLine(string.Join(",", "Time", string.Join(",", columnNames)));
            streamWriter.Close();
            Debug.Log("Logged data to: " + filePath);
        }

        // Write out columns, will be at end of file
        void OnApplicationQuit() {
            FinishLogging(true);
        }
    }
}