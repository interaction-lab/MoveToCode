using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace MoveToCode {
    public class SolMazeCheckMark : Singleton<SolMazeCheckMark> {
        #region members
        public Sprite greenCheckImg, redXimg;
        Image _img;
        Image IMG {
            get {
                if (_img == null) {
                    _img = GetComponent<Image>();
                }
                return _img;
            }
        }
        public UnityEvent OnMazeCorrect, OnMazeIncorrect;
        public TextMeshProUGUI mazesMatchText;
        #endregion

        #region unity
        private void Awake() {
            ToggleCheckMark();
            Interpreter.instance.OnCodeReset.AddListener(ToggleCheckMark);
        }
        #endregion

        #region public
        public void ToggleCheckMark() {
            // wait for end of frame to check for solution using a coroutine
            StartCoroutine(ToggleCheckMarkCoroutine());
        }

        private IEnumerator ToggleCheckMarkCoroutine() {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame(); // hack for some race conditions
            bool toggle = MazeManager.instance.IsSameAsSolutionMaze();
            if (toggle) {
                IMG.sprite = greenCheckImg;
                AudioManager.instance.PlayButtonClick();
                OnMazeCorrect.Invoke();
            }
            else {
                IMG.sprite = redXimg;
                AudioManager.instance.PlayReleaseClick();
                OnMazeIncorrect.Invoke();
            }
        }

        public void SetFreePlayText() {
            mazesMatchText.text = "Free\nPlay!";
        }
        #endregion

        #region private
        #endregion
    }
}
