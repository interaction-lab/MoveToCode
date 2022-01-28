using System;
using UnityEngine;
using UnityEssentials.Extensions;

namespace MoveToCode {
    public class ColorDataType : BasicDataType {
        public ColorDataType(CodeBlock cbIn) : base(cbIn) { }
        public ColorDataType(CodeBlock cbIn, Color valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(Color);
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is ColorDataType) {
                return (Color)(value) == (Color)(otherVal as ColorDataType).value;
            }
            throw new InvalidOperationException("Trying to compare a Number Type to a non Number Data Type");
        }

        public override string ToString() {
            return ExtColorToNames.FindColor((Color)value).Replace(" ", Environment.NewLine);
        }

    }
}

