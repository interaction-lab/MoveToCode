using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class Shelf : Singleton<Shelf> {
        MeshOutline outline;

        private void Awake() {
            outline = GetComponent<MeshOutline>();
            outline.enabled = false;
        }

        private void OnTriggerStay(Collider blockCol) {
            if (blockCol.GetComponentInParent<CodeBlock>() != null && !blockCol.GetComponentInParent<CodeBlock>().GetIsMenuBlock()) {
                blockCol.GetComponentInParent<CloneOnDrag>().SetBlockStillInMenu(true);
                EnableShelfOutline();
            }
            else {
                blockCol.GetComponentInParent<CloneOnDrag>()?.SetBlockStillInMenu(false);
            }
        }

        private void OnTriggerExit(Collider blockCol) {
            if (blockCol.GetComponentInParent<CodeBlock>() != null && !blockCol.GetComponentInParent<CodeBlock>().GetIsMenuBlock()) {
                blockCol.GetComponentInParent<CloneOnDrag>().SetBlockStillInMenu(false);
            }
            DisableShelfOutline();
        }

        public void EnableShelfOutline() {
            outline.enabled = true;
        }

        public void DisableShelfOutline() {
            outline.enabled = false;
        }
    }
}


