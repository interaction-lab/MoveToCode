using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class SingleRightArgObjectMesh : CodeBlockObjectMesh {
        protected Transform top, argRight;
        protected Vector3 origScaleArgRight;
        protected Vector3 origPositionArgRight;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            argRight = transform.GetChild(1);
            origScaleArgRight = argRight.localScale;
            origPositionArgRight = argRight.localPosition;
        }
        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                argRight.gameObject.AddComponent<MeshOutline>()
                };
        }

        public override float GetBlockHorizontalSize() {
            return 1 /*top*/ + argRight.localScale.x * 0.5f /*argright*/;
        }

        public override float GetBlockVerticalSize() {
            return 0.5f + FindChainSize(GetMyCodeBlock().GetArgumentFromDict(CommonSCKeys.Next));
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero; // todo later
        }
    }
}
