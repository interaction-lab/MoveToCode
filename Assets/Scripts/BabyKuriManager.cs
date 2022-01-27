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
            kuriController.ResetOrigPosAndRot();
        }
        #endregion

        #region private
        #endregion
    }
}
