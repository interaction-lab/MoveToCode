using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoveToCode
{
    public class UploadBarController : MonoBehaviour
    {
        private Slider progressBar;
        private long bytesUploaded = 0;
        public long totalBytes = 0;

        private void Awake()
        {
            progressBar = gameObject.GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            progressBar.value = bytesUploaded;
        }

        public void changeBytesUploaded(long total_bytes)
        {
            // Set progress to new uploaded bytes
            bytesUploaded = total_bytes;
        }
    }

}