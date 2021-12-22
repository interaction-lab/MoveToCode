using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    /// <summary>
    /// Class used to make static keys of look up of arguments
    /// </summary>
    public class CommonSCKeys : MonoBehaviour {
        public static KeyValuePair<Type, int> Next = new KeyValuePair<Type, int>(
                    typeof(NextSnapCollider),
                    0);
        public static KeyValuePair<Type, int> Nested = new KeyValuePair<Type, int>(
                    typeof(NestedSnapCollider),
                    0);
        public static KeyValuePair<Type, int> LeftConditional = new KeyValuePair<Type, int>(
                    typeof(LeftOfConditionalSnapCollider),
                    0);
        public static KeyValuePair<Type, int> RightConditional = new KeyValuePair<Type, int>(
                    typeof(RightOfConditionalSnapCollider),
                    0);
        public static KeyValuePair<Type, int> Array = new KeyValuePair<Type, int>(
                    typeof(ArraySnapCollider),
                    0);
        public static KeyValuePair<Type, int> Conditional = new KeyValuePair<Type, int>(
                    typeof(ConditionalSnapCollider),
                    0);
        public static KeyValuePair<Type, int> LeftNumber = new KeyValuePair<Type, int>(
                    typeof(LeftNumberSnapCollider),
                    0);
        public static KeyValuePair<Type, int> RightNumber = new KeyValuePair<Type, int>(
                    typeof(RightNumberSnapCollider),
                    0);
        public static KeyValuePair<Type, int> Printable = new KeyValuePair<Type, int>(
                    typeof(PrintableSnapCollider),
                    0);
        public static KeyValuePair<Type, int> Value = new KeyValuePair<Type, int>(
                    typeof(ValueSnapCollider),
                    0);
        public static KeyValuePair<Type, int> Variable = new KeyValuePair<Type, int>(
                    typeof(VariableSnapCollider),
                    0);
    }
}
