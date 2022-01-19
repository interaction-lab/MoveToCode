using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MoveSnapCollider : SnapCollider {
        public override Vector3 SnapPosition {
            get {
                if (MyCodeBlock as SetVariableCodeBlock != null) {
                    return new Vector3(0, 0, 0);
                }
                return transform.localPosition;
            }
        }

        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(MoveDataType) };
        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(MoveSnapCollider),
                    0),
                this);
        }
    }
}

