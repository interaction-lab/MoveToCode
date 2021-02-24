using System.Collections.Generic;
using System.IO;
using UnityEngine;

using System.Threading.Tasks;
using System.Threading;
using Firebase.Storage;
using Firebase.Extensions;
using System;
using UnityEngine.UI;

namespace MoveToCode {
    public class UploadManager : MonoBehaviour
    {
        // ADDED this for progress bar
        private UploadBarController progressBar;
        public long total_bytes = 0;
        public long transferred_bytes = 0;

        // initializes progress bar and progress text
        private void Awake()
        {
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
            /*
            // if we started uploading file, then change status on the text
            if (uploadStarted) {
                uploadState.startUploading();
                uploadStarted = false; // reset boolean value
            }
            // if we finished uploading file, then change status on the text
            if (uploadFinished) {
                uploadState.finishUploading();
                uploadFinished = false;
            }
            */
        }

        // ADDED THIS
        public void UploadLog() {
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
                    Debug.Log("Upload finished.");
                }
            });
        }
    }
}
