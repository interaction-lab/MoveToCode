using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UploadText : MonoBehaviour
{
    private Text progressText;
    private string status = "Upload not yet started...";
    private void Awake()
    {
        progressText = GetComponent<Text>();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        progressText.text = status;
    }
    */ 

    // Started uploading process
    public void startUploading() 
    {
        progressText.text = "Started uploading...";
    }

    // Finished uploading 
    public void finishUploading()
    {
        progressText.text = "Successfully uploaded file!";
    }
}
