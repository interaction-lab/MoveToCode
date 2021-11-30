
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoveToCode {
    public class MoveToNextScene : MonoBehaviour {
        public void MoveToNextSceneInBuildOrder(){
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

