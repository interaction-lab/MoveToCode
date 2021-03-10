using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoveToCode
{
    /// <summary>
    /// progressBarController class keeps track of the file uploading progress
    /// and visually displays the changes of bytes uploaded to Firebase. 
    /// </summary>
    public class progressBarController : MonoBehaviour {
        private Slider progressBar;
        public long totalBytes = 0;

        private void Awake() {
            progressBar = gameObject.GetComponent<Slider>();
            ChangeBytesUploaded(0);
        }

        /// <summary>
        /// Sets progress bar value to new total of uploaded bytes for CSV file
        /// </summary>
        public void ChangeBytesUploaded(long totalBytes) {
            progressBar.value = totalBytes;
        }
    }
}