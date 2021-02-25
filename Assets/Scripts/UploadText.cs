using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace MoveToCode
{

    public class UploadText : MonoBehaviour
    {
        private TextMeshProUGUI progressText;
        private string status = "Upload not yet started...";
        private void Awake()
        {
            progressText = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
 
        void Update()
        {
            if (progressText != null)
            {
                progressText.text = status;
            }
        }
        

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
}
