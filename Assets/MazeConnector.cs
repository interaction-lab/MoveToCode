using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeConnector : MonoBehaviour {
        #region members
        public MazePiece.CONNECTDIR connectionDir;
        MazePiece _mazePiece;
        public MazePiece MyMazePiece {
            get {
                if (_mazePiece == null) {
                    _mazePiece = transform.parent.GetComponent<MazePiece>();
                    if (_mazePiece == null) {
                        Debug.LogError("MazeConnector: MyMazePiece is null");
                    }
                }
                return _mazePiece;
            }
            set {
                _mazePiece = value;
            }
        }
        Rigidbody rb;
        Collider mcolider;
        MeshRenderer _meshRenderer;
        MeshRenderer MeshRend {
            get {
                if (_meshRenderer == null) {
                    _meshRenderer = GetComponent<MeshRenderer>();
                }
                return _meshRenderer;
            }
        }
        MazeManager _mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (_mazeManager == null) {
                    _mazeManager = MazeManager.instance;
                }
                return _mazeManager;
            }
        }
        public Connection MyConnection = null;
        Color origColor;
        #endregion

        #region unity
        private void OnEnable() {
            SetUpMazeConnector();
        }

        private void OnTriggerEnter(Collider other) {
            MazeConnector otherMazeConnector = other.gameObject.GetComponent<MazeConnector>();
            if (otherMazeConnector != null) {
                Debug.Log("OnTriggerEnter: " + otherMazeConnector.transform.name);
                RequestAndConnect(otherMazeConnector);
            }
        }
        private void OnTriggerExit(Collider other) {
            if (!IsConnected()) {
                return;
            }
            MazeConnector otherMazeConnector = other.gameObject.GetComponent<MazeConnector>();
            if (otherMazeConnector?.MyConnection == MyConnection) {
                MyConnection.RequestDisconnect();
            }
        }
        #endregion

        #region public
        internal void TurnOnConnector() {
            MeshRend.enabled = true;
            mcolider.enabled = true;
        }

        internal void TurnOffConnector() {
            MeshRend.enabled = false;
            mcolider.enabled = false;
        }

        public bool IsConnected() {
            return MyConnection != null;
        }

        internal void Disconnect() {
            MyConnection = null;
            MyMazePiece.transform.parent = MazeManagerInstance.transform;
            SetColor(origColor);
        }
        internal void SetColor(Color c) {
            MeshRend.material.color = c;
        }
        #endregion

        #region private
        private void SetUpMazeConnector() {
            MyMazePiece.RegisterConnector(connectionDir, this);
            origColor = MeshRend.material.color;
            if (rb == null) {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
                mcolider = GetComponent<Collider>();
                mcolider.isTrigger = true;
            }
        }
        private void RequestAndConnect(MazeConnector otherMazeConnector) {
            MyConnection = MazeManagerInstance.RequestConnection(this, otherMazeConnector);
            if (MyConnection != null) {
                MyConnection.Connect(this, otherMazeConnector);
            }
        }
        #endregion
    }
}