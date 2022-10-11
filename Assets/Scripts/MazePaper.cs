using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazePaper : Singleton<MazePaper> {
        #region members
        SpriteRenderer sr;
        SpriteRenderer MySpriteRend {
            get {
                if (sr == null) {
                    sr = GetComponent<SpriteRenderer>();
                }
                return sr;
            }
        }
        bool initialized = false;
        public static Dictionary<string, Sprite> spriteDict = null;
        #endregion

        #region unity
        private void OnEnable() {
            if (!initialized) {
                TurnOff();
                InitSpriteDict();
            }
        }
        #endregion

        #region public
        public void TurnOn(string mpName) {
            MySpriteRend.sprite = spriteDict[mpName];
            if (mpName == "BKMazePiece") {
                transform.localRotation = Quaternion.Euler(0, 0, 180); // special case this piece because it is actually backward :/
            }
            else {
                transform.localRotation = Quaternion.identity;
            }
            MySpriteRend.enabled = true;
        }

        public void TurnOff() {
            MySpriteRend.enabled = false;
        }
        #endregion

        #region private
        void InitSpriteDict() {
            // meh whatever good enough for now
            spriteDict = new Dictionary<string, Sprite>();
            spriteDict.Add("BKMazePiece", Resources.Load<Sprite>(ResourcePathConstants.BKMazePieceSprite));
            spriteDict.Add("goal", Resources.Load<Sprite>(ResourcePathConstants.GoalPieceSprite));
            spriteDict.Add("hall_1", Resources.Load<Sprite>(ResourcePathConstants.Hall1PieceSprite));
            spriteDict.Add("hall_2", Resources.Load<Sprite>(ResourcePathConstants.Hall2PieceSprite));
            spriteDict.Add("turnnw_1", Resources.Load<Sprite>(ResourcePathConstants.TurnNW1PieceSprite));
            spriteDict.Add("turnnw_2", Resources.Load<Sprite>(ResourcePathConstants.TurnNW2PieceSprite));
        }
        #endregion
    }
}
