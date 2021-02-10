using System;
using System.Collections;
using Firebase.Storage;
using UnityEngine;

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
}

