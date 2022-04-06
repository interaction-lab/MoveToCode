using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeConnector : MonoBehaviour {
        #region members
        public MazePiece.CONNECTDIR connectionDir;
        MazePiece mazePiece;
        Rigidbody rb;
        Collider collider;
        MeshRenderer _meshRenderer;
        public MazeConnector connectedConnector = null;
        MeshRenderer MeshRend {
            get {
                if (_meshRenderer == null) {
                    _meshRenderer = GetComponent<MeshRenderer>();
                }
                return _meshRenderer;
            }
        }
        #endregion

        #region unity
        private void OnEnable() {
            mazePiece = transform.parent.GetComponent<MazePiece>();
            mazePiece.RegisterConnector(connectionDir, this);
            if (rb == null) {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
                collider = GetComponent<Collider>();
                collider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other) {
            MazeConnector otherMazeConnector = other.gameObject.GetComponent<MazeConnector>();
            if (otherMazeConnector != null) {
                otherMazeConnector.Connect(this);
            }
        }

        private void OnTriggerExit(Collider other) {
            MazeConnector otherMazeConnector = other.gameObject.GetComponent<MazeConnector>();
            if (otherMazeConnector == connectedConnector) {
                otherMazeConnector.Disconnect();
            }
        }
        #endregion

        #region public
        internal void TurnOnConnector() {
            MeshRend.enabled = true;
            collider.enabled = true;
        }

        internal void TurnOffConnector() {
            MeshRend.enabled = false;
            collider.enabled = false;
        }

        internal void Connect(MazeConnector other) {
            if (connectedConnector != null) {
                connectedConnector.Disconnect();
            }
            connectedConnector = other;
            mazePiece.transform.parent = connectedConnector.transform;
        }

        internal void Disconnect() {
            MazeConnector mc = connectedConnector;
            connectedConnector = null;
            if (mc == this) {
                mc.Disconnect();
            }
            if (mazePiece.transform.parent == mc.transform) {
                mazePiece.transform.parent = MazeManager.instance.transform;
            }
        }

        #endregion

        #region private
        #endregion
    }
}
