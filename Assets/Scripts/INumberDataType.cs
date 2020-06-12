using System;
using UnityEngine;

namespace MoveToCode {
    public abstract class INumberDataType : BasicDataType {
        public INumberDataType(CodeBlock cbIn) : base(cbIn) { }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is INumberDataType) {
                return Mathf.Approximately(TurnValToFloat(this), TurnValToFloat((otherVal as INumberDataType)));
            }
            throw new InvalidOperationException("Trying to compare a Number Type to a non Number Data Type");
        }

        public static float TurnValToFloat(BasicDataType dIn) {
            return Convert.ToSingle(dIn.GetValue());
        }
    }
}