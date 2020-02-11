using System;
using UnityEngine;

namespace MoveToCode {
    public class INumberDataType : IDataType {
        public INumberDataType(CodeBlock cbIn) : base(cbIn) { }
        public INumberDataType(CodeBlock cbIn, dynamic valIn) : base(cbIn) { SetValue(valIn); }
        public INumberDataType(dynamic valIn) : base(null) { SetValue(valIn); }
        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is INumberDataType) {
                return Mathf.Approximately((float)GetValue(), (float)otherVal.GetValue());
            }
            throw new InvalidOperationException("Trying to compare a Number Type to a non Number Data Type");
        }
    }
}