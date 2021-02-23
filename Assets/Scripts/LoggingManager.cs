using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

using System.Threading.Tasks;
using System.Threading;
using Firebase.Storage;
using Firebase.Extensions;
using System;

namespace MoveToCode {
    public class LoggingManager : Singleton<LoggingManager> {

        public bool logData = false;
        bool initialized = false;
        Dictionary<string, int> columnLookup;
        List<string> row;
        StreamWriter streamWriter;

        string csvFilename;
        string filePath;

        // ADDED this for progress bar
        private UploadBarController progressBar;
        long total_bytes = 0;
        long transferred_bytes = 0;

        private void Awake() {
            Init();
            progressBar = GameObject.Find("progressSlider").GetComponent<UploadBarController>();
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
            HumanStateManager.instance.UpdateKC();
            if (!logData) {
                return;
            }
            List<string> rowDuplicate = new List<string>(row);
            // added this
            if (StreamWriterIsOpen()) {
                streamWriter.WriteLine(string.Join(",", Time.time.ToString(), string.Join(",", rowDuplicate)));
            }
           // streamWriter.WriteLine(string.Join(",", Time.time.ToString(), string.Join(",", rowDuplicate)));
            ResetRow();
        }

        private void FixedUpdate() {
            WriteRowToCSV();
        }

        // ADDED THIS
        /*
        public void UploadLog() {
            // to finish up the csv
            FinishLogging(true);
            // Create a reference to the file you want to upload
            var storage = FirebaseStorage.DefaultInstance;
            var csvRef = storage.GetReference($"/csvfiles/{csvFilename}");
            // Start uploading a file
            var task = csvRef.PutFileAsync(filePath, null,
                    new StorageProgress<UploadState>(state => {
            // called periodically during the upload
            Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
                            state.BytesTransferred, state.TotalByteCount));
                    }), CancellationToken.None, null);

            task.ContinueWithOnMainThread(resultTask =>
            {
                if (!resultTask.IsFaulted && !resultTask.IsCanceled)
                {
                    Debug.Log("Upload finished.");
                }
            });

        }
        */

        // ADDED this..
        private void Update()
        {
            // update progress bar
            if (total_bytes != 0) {
                // change progress bar appropriately (total value is out of 100)
                progressBar.changeBytesUploaded((100 * transferred_bytes) / total_bytes);
            }
        }

        public void FinishLogging(bool hasQuit = false) {
            // Added steamwriter condition
            if (!logData || !StreamWriterIsOpen()) {
                return;
            }
            var ordered = columnLookup.OrderBy(x => x.Value);
            List<string> columnNames = new List<string>();
            foreach (var pairKeyVal in ordered) {
                columnNames.Add(pairKeyVal.Key);
            }
            // Added this to check that streamwriter is not closed
            if (streamWriter.BaseStream != null) {
                streamWriter.WriteLine(string.Join(",", "Time", string.Join(",", columnNames)));
                streamWriter.Close();
            }

            Debug.Log("Logged data to: " + filePath);


            // ADDED this
            // Create a reference to the file you want to upload
            var storage = FirebaseStorage.DefaultInstance;
            var csvRef = storage.GetReference($"/csvfiles/{csvFilename}");
            // Start uploading a file
            var task = csvRef.PutFileAsync(filePath, null,
                new StorageProgress<UploadState>(state => {
                    total_bytes = state.TotalByteCount;
                    transferred_bytes = state.BytesTransferred;
                    // called periodically during the upload
                    Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
                                    state.BytesTransferred, state.TotalByteCount));
                }), CancellationToken.None, null);

            task.ContinueWithOnMainThread(resultTask =>
            {
                if (!resultTask.IsFaulted && !resultTask.IsCanceled)
                {
                    Debug.Log("Upload finished.");
                }
            });

            /*
            // ADDED THIS
            // Create a reference to the file you want to upload
            var storage = FirebaseStorage.DefaultInstance;
            var csvRef = storage.GetReference($"/csvfiles/{csvFilename}");

            // Upload the file to the path csvfiles folder
            csvRef.PutFileAsync(filePath)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                // Uh-oh, an error occurred!
            }
                else
                {
                // Metadata contains file metadata such as size, content-type, and download URL.
                StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });
            */



        }

        // ADDED THIS
        bool StreamWriterIsOpen()
        {
            return streamWriter.BaseStream != null;
        }

        // Write out columns, will be at end of file
        void OnApplicationQuit() {
        //   FinishLogging(true);
        }
    }
}