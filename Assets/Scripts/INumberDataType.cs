using System;
using UnityEngine;

namespace MoveToCode {
    public class INumberDataType : IDataType {
        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is INumberDataType) {
                return Mathf.Approximately((float)GetValue(), (float)otherVal.GetValue());
            }
            throw new InvalidOperationException("Trying to compare a Number Type to a non Number Data Type");
        }
    }
}