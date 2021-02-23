using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UploadBarController : MonoBehaviour
{
    private Slider progressBar;
    private long bytesUploaded = 0;

    private void Awake()
    {
        progressBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = bytesUploaded;
    }

    public void changeBytesUploaded(long  total_bytes) 
    {
        // Set progress to new uploaded bytes
        progressBar.value = total_bytes;
    }
}
