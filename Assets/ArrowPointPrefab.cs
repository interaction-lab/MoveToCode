using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrowPointPrefab : MonoBehaviour {
        #region members
        UIArrow _outerArrow, _innerArrow;
        UIArrow outerArrow {
            get {
                if (_outerArrow == null) {
                    _outerArrow = transform.Find(UIArrow.OuterArrowName).GetComponent<UIArrow>();
                }
                return _outerArrow;
            }
        }
        UIArrow innerArrow {
            get {
                if (_innerArrow == null) {
                    _innerArrow = transform.Find(UIArrow.InnerArrowName).GetComponent<UIArrow>();
                }
                return _innerArrow;
            }
        }

        BehindYouText _behindYouText;
        BehindYouText behindYouText {
            get {
                if (_behindYouText == null) {
                    _behindYouText = GetComponentInChildren<BehindYouText>();
                }
                return _behindYouText;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        
        #endregion

        #region private
        #endregion
    }
}
