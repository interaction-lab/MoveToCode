using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class PreSurveyButtonManager : MonoBehaviour {

        MoveToNextScene moveToNextScene;
        private void Awake() {
            moveToNextScene = FindObjectOfType<MoveToNextScene>() as MoveToNextScene;
            moveToNextScene.gameObject.SetActive(false);
            GetComponent<Button>().onClick.AddListener(EnableMoveToNextSceneButton);
        }

        void EnableMoveToNextSceneButton() {
            moveToNextScene.gameObject.SetActive(true);
        }

    }
}
