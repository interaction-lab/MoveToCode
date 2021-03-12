using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;

using Firebase;
using System.Threading.Tasks;
using System.Threading;
using Firebase.Storage;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using TMPro;

namespace MoveToCode {
    /// <summary>
    /// UploadManager class uploads a CSV file to Firebase Storage
    /// when user presses the Upload File button. It also
    /// uses a progress bar to track the file upload process. 
    /// </summary>
    public class UploadManager : Singleton<UploadManager> {

        private progressBarController progressBar;
        private TextMeshProUGUI progressText;
        private long totalBytes = 0;
        private long transferredBytes = 0;
        protected static string UriFileScheme = Uri.UriSchemeFile + "://";
        private int count = 1;

        private void Awake() {
            progressText = GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
            progressBar = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>().GetComponent<progressBarController>();
        }
        
        void Update() {
            // Changes progress bar appropriately by bytes that have been uploaded (total value is out of 100)
            if (totalBytes != 0)
            {
                progressBar.ChangeBytesUploaded((100 * transferredBytes) / totalBytes);
            }    
        }


        IEnumerator RestartBtn() {
            yield return new WaitForSeconds(5);
            progressText.text = "Upload file";
        }

        /// <summary>
        /// Gets the local filename as a URI relative to the persistent data path if the path isn't already a file URI
        /// </summary>
        /// <returns>URI of local filename</returns>
        protected virtual string PathToPersistentDataPathUriString(string filename) {
            if (filename.StartsWith(UriFileScheme)) {
                return filename;
            }
            return String.Format("{0}{1}/{2}", UriFileScheme, Application.persistentDataPath,
                filename);
        }
    
        /// <summary>
        /// Gets the finished CSV file from LoggingManager and uploads it to Firebase 
        /// </summary>
        public void UploadLog() {
            LoggingManager.instance.FinishLogging(true);
            LogCSVToFirebase();
        }

        /// <summary>
        /// Uploads CSV file to Firebase and displays upload progress to user
        /// </summary>
        private void LogCSVToFirebase() {
            progressText.text = "Started upload";
            var storage = FirebaseStorage.DefaultInstance;
            /// <value>count is the version number of the file. It keeps track of number of times the user uploads the same filename in one session.</value>
            var csvRef = storage.GetReference($"/csvfiles/({count}){LoggingManager.instance.getCSVFileName()}");
            var filePath = PathToPersistentDataPathUriString(LoggingManager.instance.getCSVFileName());
            // Starts uploading a file
            var task = csvRef.PutFileAsync(filePath, null,
                new StorageProgress<UploadState>(state => {
                    totalBytes = state.TotalByteCount;
                    transferredBytes = state.BytesTransferred;
                    // Called periodically during the upload
                    Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
                                    state.BytesTransferred, state.TotalByteCount));
                }), CancellationToken.None, null);

            task.ContinueWithOnMainThread(resultTask =>
            {
                // Checks that file is successfully uploaded and restarts upload button 
                if (!resultTask.IsFaulted && !resultTask.IsCanceled)
                {
                    progressText.text = "Finished upload";
                    count++;
                    StartCoroutine(RestartBtn());
                    Debug.Log("Upload finished.");
                }
            });
        }
    }
}
