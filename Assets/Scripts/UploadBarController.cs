using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoveToCode
{
    /// <summary>
    /// UploadBarController class keeps track of the file uploading progress
    /// and visually displays the changes of bytes uploaded to Firebase. 
    /// </summary>
    public class UploadBarController : MonoBehaviour
    {
        private Slider progressBar;
        private long bytesUploaded = 0;
        public long totalBytes = 0;

        private void Awake() {
            progressBar = gameObject.GetComponent<Slider>();
        }

        void Update() {
            progressBar.value = bytesUploaded;
        }

        // Sets progress bar value to new total of uploaded bytes
        public void changeBytesUploaded(long total_bytes) {
            bytesUploaded = total_bytes;
        }
    }

}