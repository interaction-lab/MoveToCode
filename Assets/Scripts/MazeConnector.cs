using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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

        public MazePiece ConnectedMP {
            get {
                if (MyConnection == null) {
                    return null;
                }
                return MyConnection.GetConnectedPiece(this);
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
        public bool IsAnchored {
            get {
                return MyMazePiece.IsAnchored;
            }
        }

        public bool On {
            get {
                return MeshRend.enabled;
            }
        }

        public static bool HitThisFrame {
            get;
            set;
        }

        SwitchModeButton smb;
        SwitchModeButton ModeButton {
            get {
                if (smb == null) {
                    smb = SwitchModeButton.instance;
                }
                return smb;
            }
        }

        #endregion

        #region unity
        private void Awake() {
            SetUpMazeConnector();
        }

        private void Update() {
            // this is where I would do a jank double check to see if the connection is still populated
            if (MyConnection != null && MyConnection.IsBroke()) {
                MyConnection.RequestDisconnect();
            }
        }

        public void RemoveConnection() {
            if (MyConnection != null) {
                MyConnection.RequestDisconnect();
                MazeManagerInstance.RemoveAllRequests(this);
            }
        }

        private void OnTriggerEnter(Collider other) {
            MazeConnector otherMazeConnector = other.gameObject.GetComponent<MazeConnector>();
            if ((otherMazeConnector != null &&
                otherMazeConnector.MyMazePiece != MyMazePiece &&
                IsSameMazePieceTypeRegardingSolutionVsRealMazePieces(otherMazeConnector.MyMazePiece)) &&
                On &&
                otherMazeConnector.On) {
                // make a check here to see if my connection is all messed up
                if (MyConnection != null && MyConnection.IsBroke()) {
                    MyConnection.RequestDisconnect();
                }

                AddRequestAndAttemptConnect(otherMazeConnector);
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
                StartCoroutine(HitForOneFrame());
            }
        }
        private void OnTriggerExit(Collider other) {
            MazeConnector otherMazeConnector = other.gameObject.GetComponent<MazeConnector>();
            if ((otherMazeConnector != null &&
                IsSameMazePieceTypeRegardingSolutionVsRealMazePieces(otherMazeConnector.MyMazePiece))) { // maybe just never care about being on vs off here
                RemoveRequestAndAttemptConnect(otherMazeConnector);
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
            }
        }


        #endregion

        #region public
        internal void TurnOnConnector() {
            MeshRend.enabled = true;
        }

        internal void TurnOffConnector() {
            MeshRend.enabled = false;
        }

        public bool IsSameMazePieceTypeRegardingSolutionVsRealMazePieces(MazePiece otherMP) { // super jank, but it works to change the difference in solution maze pieces and generic maze pieces
            bool iamsol = (MyMazePiece as SolMazePiece) != null;
            bool theyaresol = (otherMP as SolMazePiece) != null;
            return iamsol == theyaresol;
        }
        public bool IsConnected() {
            return MyConnection != null;
        }

        internal void AnchorNeighboringPiece() {
            Assert.IsTrue(IsAnchored);
            if (IsConnected()) {
                MyConnection.AnchorNeighboringPiece();
            }
        }

        internal void Disconnect() {
            MyConnection = null; // this can only happen after it is fully broken
            SetColor(origColor);
            AttemptNewConnection();
        }

        internal void AnchorTo(MazeConnector anchorConnector) {
            Vector3 Upward = anchorConnector.MyMazePiece.transform.up;

            MyMazePiece.transform.position = anchorConnector.MyMazePiece.transform.position;
            MyMazePiece.transform.rotation = anchorConnector.MyMazePiece.transform.rotation;
            float angle = GetAngleOfConnectorRelativeToForward(anchorConnector);
            MyMazePiece.transform.RotateAround(MyMazePiece.transform.position, Upward, angle);
            MyMazePiece.transform.position = anchorConnector.transform.position;
            Vector3 myRealtivePos = transform.position - MyMazePiece.transform.position;
            MyMazePiece.transform.position -= myRealtivePos;

            MyMazePiece.SnapConnections();
            AudioManager.instance?.PlaySoundAtObject(gameObject, AudioManager.snapAudioClip);
        }

        WaitForEndOfFrame wfe = new WaitForEndOfFrame();
        IEnumerator HitForOneFrame() {
            HitThisFrame = true;
            yield return wfe;
            HitThisFrame = false;
        }

        private float GetAngleOfConnectorRelativeToForward(MazeConnector anchorMC) {
            float angle = 0;
            switch (connectionDir) {
                case MazePiece.CONNECTDIR.North:
                    angle = 180;
                    break;
                case MazePiece.CONNECTDIR.East:
                    angle = 90;
                    break;
                case MazePiece.CONNECTDIR.South:
                    angle = 0;
                    break;
                case MazePiece.CONNECTDIR.West:
                    angle = 270;
                    break;
            }
            switch (anchorMC.connectionDir) { // parent
                case MazePiece.CONNECTDIR.North:
                    angle += 0;
                    break;
                case MazePiece.CONNECTDIR.East:
                    angle += 90;
                    break;
                case MazePiece.CONNECTDIR.South:
                    angle += 180;
                    break;
                case MazePiece.CONNECTDIR.West:
                    angle += 270;
                    break;
            }

            return angle % 360;
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
        private void AddRequestAndAttemptConnect(MazeConnector otherMazeConnector) {
            MazeManagerInstance.AddConnectionRequest(this, otherMazeConnector);
            AttemptNewConnection();
        }
        private void RemoveRequestAndAttemptConnect(MazeConnector otherMazeConnector) {
            MazeManagerInstance.RemoveRequest(this, otherMazeConnector);
            if (MyConnection != null &&
                (otherMazeConnector == null ||
                otherMazeConnector?.MyConnection == MyConnection)) {
                MyConnection.RequestDisconnect();
            }
        }

        private void AttemptNewConnection() {
            if (MyConnection == null) {
                MazeManagerInstance.SetUpConnection(this);
            }
            else { // already have a connection

            }


            if (!(MyMazePiece is SolMazePiece)) {
                MazeManagerInstance.LogMaze();
            }
        }
        #endregion
    }
}
