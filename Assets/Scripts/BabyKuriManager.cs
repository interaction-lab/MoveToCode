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
