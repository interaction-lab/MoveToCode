using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    /// <summary>
    /// ExitGame class sets the upload file button and progress bar to be active 
    /// and visible to the user when they want to end their session.
    /// </summary>
    public class ExitGame : MonoBehaviour {
        public Button uploadBtn;
        public Slider progressBar;

        void Start() {
            uploadBtn.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
        }

        /// <summary>
        /// When the user clicks the exit button, the upload file and progress bar will
        /// appear and be interactable.
        /// </summary>
        public void ToggleUploadActive() {
            bool a = uploadBtn.gameObject.activeSelf;
            uploadBtn.gameObject.SetActive(!a);
            progressBar.gameObject.SetActive(!a);
        }
    }
}

