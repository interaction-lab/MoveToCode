using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class TrashButton : MonoBehaviour {
        #region members
        MazePiece mazePiece;
        public MazePiece MyMazePiece {
            get {
                if (mazePiece == null) {
                    mazePiece = transform.parent.GetComponentInParent<MazePiece>(); // flimsy
                }
                return mazePiece;
            }
        }
        Button trashButton;
        public Button MyTrashButton {
            get {
                if (trashButton == null) {
                    trashButton = GetComponent<Button>();
                }
                return trashButton;
            }
        }
        ARTrackBehavior _aRTrackBehavior;
        ARTrackBehavior arTrackBehavior {
            get {
                if (_aRTrackBehavior == null) {
                    _aRTrackBehavior = MyMazePiece.GetComponent<ARTrackBehavior>();
                }
                return _aRTrackBehavior;
            }
        }
        bool isTrashButtonPressed = false;
        #endregion

        #region unity
        void Start() {
            MyTrashButton.onClick.AddListener(OnTrashButtonClick);
            arTrackBehavior.OnImgStartedTracking.AddListener(ToggleState);
            arTrackBehavior.OnImgStoppedTracking.AddListener(ToggleState);
            MazeManager.instance.OnMazeLocked.AddListener(ToggleState);
        }
        #endregion
        #region public
        #endregion
        #region private
        void OnTrashButtonClick() {
            MyMazePiece.DisableMyPiece();
            isTrashButtonPressed = true;
        }

        void ToggleState() {
            if(arTrackBehavior.IsTracking){
                isTrashButtonPressed = false;
            }
            if (arTrackBehavior.IsTracking || MazeManager.instance.IsLocked) {
                gameObject.SetActive(false);
            }
            else {
                if(!isTrashButtonPressed) {
                    gameObject.SetActive(true);
                }
            }
        }
        #endregion
    }
}
