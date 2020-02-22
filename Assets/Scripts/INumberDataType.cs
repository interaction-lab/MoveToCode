using System;
using UnityEngine;

namespace MoveToCode {
    public abstract class INumberDataType : IDataType {
        public INumberDataType(CodeBlock cbIn) : base(cbIn) { }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is INumberDataType) {
                return Mathf.Approximately((float)Convert.ChangeType(GetValue(), GetCastType()), (float)Convert.ChangeType(otherVal.GetValue(), otherVal.GetCastType()));
            }
            throw new InvalidOperationException("Trying to compare a Number Type to a non Number Data Type");
        }
    }
}