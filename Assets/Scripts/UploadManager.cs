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
    public class UploadManager : MonoBehaviour
    {
        // ADDED this for progress bar
        private UploadBarController progressBar;
        private TextMeshProUGUI progressText;
        private long total_bytes = 0;
        private long transferred_bytes = 0;
        protected static string UriFileScheme = Uri.UriSchemeFile + "://";

        // initializes progress bar and progress text
        private void Awake()
        {
            progressText = GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
            progressBar = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>().GetComponent<UploadBarController>();
        }
        

        // Update is called once per frame
        void Update()
        {
            // update progress bar
            // change progress bar appropriately (total value is out of 100)
            if (total_bytes != 0)
            {
                progressBar.changeBytesUploaded((100 * transferred_bytes) / total_bytes);
            }    
        }

        IEnumerator restartBtn() {
           // progressText.text = "Upload file";
           Debug.Log("Called!");
            yield return new WaitForSeconds(5);
            // restart upload button
            progressText.text = "Upload file";
        }

        // Get the local filename as a URI relative to the persistent data path if the path isn't
        // already a file URI.
        protected virtual string PathToPersistentDataPathUriString(string filename) {
        if (filename.StartsWith(UriFileScheme)) {
            return filename;
        }
        return String.Format("{0}{1}/{2}", UriFileScheme, Application.persistentDataPath,
            filename);
        }
    

        // ADDED THIS
        public void UploadLog() {
            progressText.text = "Started upload";
            // finish the logging upload
            LoggingManager.instance.FinishLogging(true);
            // Create a reference to the file you want to upload
            var storage = FirebaseStorage.DefaultInstance;
            var csvRef = storage.GetReference($"/csvfiles/{LoggingManager.instance.getCSVFileName()}");
            //var filePath = LoggingManager.instance.getFilePath();
            //var filePath = Application.persistentDataPath + "/" + LoggingManager.instance.getCSVFileName();
            var filePath = PathToPersistentDataPathUriString(LoggingManager.instance.getCSVFileName());
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
                    progressText.text = "Finished upload";
                    StartCoroutine(restartBtn());
                    Debug.Log("Upload finished.");
                }
            });
        }
    }
}
