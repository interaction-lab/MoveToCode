using System;
using UnityEngine;

namespace MoveToCode {
    public abstract class INumberDataType : IDataType {
        public INumberDataType(CodeBlock cbIn) : base(cbIn) { }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is INumberDataType) {
                return Mathf.Approximately(TurnValToFloat(this), TurnValToFloat(otherVal));
            }
            throw new InvalidOperationException("Trying to compare a Number Type to a non Number Data Type");
        }

        public static float TurnValToFloat(IDataType dIn) {
            if (dIn.GetCastType() == typeof(int)) {
                return (int)dIn.GetValue();
            }
            else {
                return (float)dIn.GetValue();
            }
        }
    }
}