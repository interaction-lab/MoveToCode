using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
namespace MoveToCode {
    public abstract class SingleControlFlowCodeBlock : ControlFlowCodeBlock {
        public override int GetBlockVerticalSize() {
            return 3;
        }

        public override Vector3 GetSnapToParentPosition() {
            return new Vector3(0.25f, 0, 0);
        }

        protected override void SetupMeshOutline() {
            if (outlineMaterial == null) {
                outlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineCodeBlockMaterial) as Material;
            }
            if (meshOutline == null) {
                meshOutline = gameObject.AddComponent(typeof(MeshOutlineHierarchy)) as MeshOutlineHierarchy;
                meshOutline.OutlineMaterial = outlineMaterial;
                meshOutline.OutlineWidth = 0.05f;
            }
        }
    }
}