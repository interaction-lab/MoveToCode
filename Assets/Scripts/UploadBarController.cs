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
        public long totalBytes = 0;

        private void Awake() {
            progressBar = gameObject.GetComponent<Slider>();
            changeBytesUploaded(0);
        }
        
        // Sets progress bar value to new total of uploaded bytes
        public void changeBytesUploaded(long total_bytes) {
            progressBar.value = total_bytes;
        }
    }

}