using System;
using System.Collections;
using Firebase.Storage;
using UnityEngine;

namespace MoveToCode
{
    public class UploadFile : MonoBehaviour
    {
        public void StartUpload(TextAsset csvFile)
        {
            StartCoroutine(UploadCoroutine(csvFile));
        }

        private IEnumerator UploadCoroutine(TextAsset csvFile)
        {
            //  throw new System.NotImplementedException();
            var storage = FirebaseStorage.DefaultInstance;
            var csvReference = storage.GetReference($"/csvfiles/{Guid.NewGuid()}.csv");
            var bytes = csvFile.bytes;
            var uploadTask = csvReference.PutBytesAsync(bytes);
            yield return new WaitUntil(() => uploadTask.IsCompleted);

            if (uploadTask.Exception != null)
            {
                Debug.LogError($"Failed to upload because {uploadTask.Exception}");
                yield break;
            }

            // checks if uploaded correctly
            var getUrlTask = csvReference.GetDownloadUrlAsync();
            yield return new WaitUntil(() => getUrlTask.IsCompleted);

            if (getUrlTask.Exception != null)
            {
                Debug.LogError($"Failed to get a download url with {getUrlTask.Exception}");
                yield break;

            }

            Debug.Log($"Download from {getUrlTask.Result}");

        }


        /*
        // Path to the file located on disk
        string localFile = "...";

        // Create a reference to the file you want to upload
        var storageRef = FirebaseStorage.DefaultInstance;
        StorageReference csvRef = storageRef.Child("/csvfiles/{Guid.NewGuid()}.csv");

        // Upload the file to the path csvfiles folder
        csvRef.PutFileAsync(localFile)
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

