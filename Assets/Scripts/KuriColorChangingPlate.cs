using UnityEngine;

namespace MoveToCode {
    public class KuriColorChangingPlate : MonoBehaviour {
        #region members
        MeshRenderer _meshRend;
        public MeshRenderer MeshRend {
            get {
                if (_meshRend == null) {
                    _meshRend = GetComponent<MeshRenderer>();
                }
                return _meshRend;
            }
        }
        #endregion
    }
}
