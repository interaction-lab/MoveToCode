using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        private bool uploadStarted = false;
        private bool uploadFinished = false;

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
            
            
            // if we started uploading file, then change status on the text
            if (uploadStarted) {
                // status.startUploading();
                progressText.text = "Started upload";
                uploadStarted = false; // reset boolean value
            }
            // if we finished uploading file, then change status on the text
            if (uploadFinished) {
                //status.finishUploading();
                progressText.text = "Finished upload";
                uploadFinished = false;
            }
            
            
        }

        // ADDED THIS
        public void UploadLog() {
            // flag start of uploading
            uploadStarted = true;
            // finish the logging upload
            LoggingManager.instance.FinishLogging(true);
            // Create a reference to the file you want to upload
            var storage = FirebaseStorage.DefaultInstance;
            var csvRef = storage.GetReference($"/csvfiles/{LoggingManager.instance.getCSVFileName()}");
            // Start uploading a file
            var task = csvRef.PutFileAsync(LoggingManager.instance.getFilePath(), null,
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
                    // flag end of upload
                    uploadFinished = true;
                    Debug.Log("Upload finished.");
                }
            });
        }
    }
}
