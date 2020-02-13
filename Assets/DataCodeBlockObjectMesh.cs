using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;
namespace MoveToCode {
    public class DataCodeBlockObjectMesh : CodeBlockObjectMesh {
        Transform top;

        public override void SetUpObject() {
            top = transform.GetChild(0);
        }
        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() { top.gameObject.AddComponent<MeshOutline>() };
        }

        public override float GetBlockHorizontalSize() {
            return top.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return transform.localScale.y;
        }

        public override void ResizeObjectMesh() {
            return;//curenntly does nothing
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero;
        }
    }
}
