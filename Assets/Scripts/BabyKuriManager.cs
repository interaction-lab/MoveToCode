using UnityEngine;
namespace MoveToCode {
    public class BabyKuriManager : Singleton<BabyKuriManager> {

        #region members
        BabyVirtualKuriController kuriControllerBackingVar = null;
        public BabyVirtualKuriController kuriController {
            get {
                if (kuriControllerBackingVar == null) {
                    kuriControllerBackingVar = GetComponentInChildren<BabyVirtualKuriController>();
                }
                return kuriControllerBackingVar;
            }
        }
        BabyKuriTransformManager _bkTransformManager;
        public BabyKuriTransformManager BKTransformManager {
            get {
                if (_bkTransformManager == null) {
                    _bkTransformManager = GetComponent<BabyKuriTransformManager>();
                }
                return _bkTransformManager;
            }
        }

        #endregion

        #region unity
        #endregion

        #region public
        public void ResetKuri() {
            kuriController.ResetToOrigState();
        }
        public void ChangeKuriColor(Color color) {
            kuriController.SetColor(color);
        }
        #endregion

        #region private
        #endregion
    }
}
