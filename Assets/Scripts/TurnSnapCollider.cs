using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class TurnSnapCollider : SnapCollider {
        public override Vector3 SnapPosition {
            get {
                if (MyCodeBlock as SetVariableCodeBlock != null) {
                    return new Vector3(0, 0, 0);
                }
                return transform.localPosition;
            }
        }

        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(TurnDataType) };
        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(TurnSnapCollider),
                    0),
                this);
        }
    }
}

