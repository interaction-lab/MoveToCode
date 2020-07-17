using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Shelf : Singleton<Shelf> {
        public bool blockInContactWithShelf;

        private void OnTriggerEnter(Collider blockCol) {
            if (blockCol.GetComponentInParent<CodeBlock>() != null && !blockCol.GetComponentInParent<CodeBlock>().GetIsMenuBlock()) {
                blockCol.GetComponentInParent<CloneOnDrag>().SetBlockStillInMenu(true);
            }
        }

        private void OnTriggerExit(Collider blockCol) {
            if (blockCol.GetComponentInParent<CodeBlock>() != null && !blockCol.GetComponentInParent<CodeBlock>().GetIsMenuBlock()) {
                blockCol.GetComponentInParent<CloneOnDrag>().SetBlockStillInMenu(false);
            }
        }
    }
}


