using System;
using UnityEngine;

namespace MoveToCode {
    public abstract class BasicDataType : IDataType {

        public BasicDataType(CodeBlock cbIn) : base(cbIn) { }

        public override string ToString() {
            return Convert.ChangeType(GetValue(), GetCastType()).ToString();
        }

        public override string DescriptiveInstructionToString() {
            return ToString();
        }


        public T CastObject<T>(object input) {
            return (T)input;
        }

        public T ConvertObject<T>(object input) {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString().Replace(System.Environment.NewLine," ") + "\"", // replacing \n is for colors and kind of hacky but whateva
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"val\": \"" + GetValue().ToString().Replace(System.Environment.NewLine," ") + "\"}"
            });
        }
    }
}