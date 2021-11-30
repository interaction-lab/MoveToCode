using UnityEngine;

namespace MoveToCode {
    public class UrlButton : MonoBehaviour {
        public void OpenPreSurveyURL(){
            OpenURL("https://usc.qualtrics.com/jfe/form/SV_exQl4oNMqBuT6gm");
        }

        public void OpenPostSurveyURL(){
            OpenURL("https://usc.qualtrics.com/jfe/form/SV_exQl4oNMqBuT6gm");
        }

        private void OpenURL(string URL) {
            Application.OpenURL(string.Join("", 
            URL, 
            "?", 
            UserIDManager.EmbeddedDataForID, 
            "=", 
            UserIDManager.PlayerId));
        }
    }
}

