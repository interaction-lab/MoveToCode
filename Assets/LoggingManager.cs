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
            if (logData) {
                Debug.Log("Currently logging data: " + logData.ToString());
                csvFilename = System.DateTime.Now.ToString().Replace(' ', '_').Replace('\\', '_').Replace('/', '_').Replace(':', '-') + ".csv";
                columnLookup = new Dictionary<string, int>();
                row = new List<string>();
                filePath = Path.Combine(Application.persistentDataPath, csvFilename);
                Debug.Log(filePath);
                streamWriter = new StreamWriter(new FileStream(filePath, FileMode.Create)); ;
            }
            else {
                Debug.LogError("NOT LOGGING DATA");
            }
        }


        /// <summary>
        /// Adds key to rowLookup, updates the value
        /// </summary>
        /// <param name="key">Column Name</param>
        /// <param name="value">Value for Column</param>
        /// <returns>True if key not already in row, false otherwise</returns>
        public bool AddLogColumn(string key, string value) {
            bool isNewColumn = !columnLookup.ContainsKey(key);
            if (isNewColumn) {
                columnLookup[key] = row.Count;
                row.Add(value);
            }
            UpdateLogColumn(key, value);
            return isNewColumn;
        }

        public void UpdateLogColumn(string key, string value) {
            row[columnLookup[key]] = value;
            Assert.AreEqual(columnLookup.Keys.Count, row.Count);
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
            streamWriter.WriteLine(string.Join(",", Time.time.ToString(), string.Join(",", row)));
            ResetRow();
        }

        private void FixedUpdate() {
            WriteRowToCSV(); // Wirte a row each fixed update
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
        }

        // Write out columns, will be at end of file
        void OnApplicationQuit() {
            FinishLogging(true);
        }
    }
}