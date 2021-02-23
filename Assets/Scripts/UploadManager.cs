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
using UnityEngine.UI;

namespace MoveToCode {
    public class UploadManager : MonoBehaviour
    {
        // ADDED this for progress bar
        private UploadBarController progressBar;
        long total_bytes = 0;
        long transferred_bytes = 0;
        //private UploadText uploadState;
        private Text uploadStatus;
        bool uploadStarted = false;
        bool uploadFinished = false;

        // initializes progress bar and progress text
        private void Awake()
        {
            // ADDED bottom stuff
            progressBar = GameObject.Find("progressSlider").GetComponent<UploadBarController>();
            //uploadState = GameObject.Find("uploadText").GetComponent<UploadText>();
            uploadStatus = GameObject.Find("loser").GetComponent<Text>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
// ADDED this..
        private void Update()
        {
            // update progress bar
            if (total_bytes != 0) {
                // change progress bar appropriately (total value is out of 100)
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
            //uploadState.startUploading();
            uploadStatus.text = "Started uploading...";

            // ADDED this
            uploadStarted = true;
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
                    uploadFinished = true;
                    uploadStatus.text = "Upload finished...";
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
    }

}
