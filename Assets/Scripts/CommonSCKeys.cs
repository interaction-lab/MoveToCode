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
                    typeof(SnapColliderNext),
                    0);
        public static KeyValuePair<Type, int> Nested = new KeyValuePair<Type, int>(
                    typeof(SnapColliderNested),
                    0);
        public static KeyValuePair<Type, int> LeftConditional = new KeyValuePair<Type, int>(
                    typeof(SnapColliderLeftOfConditional),
                    0);
        public static KeyValuePair<Type, int> RightConditional = new KeyValuePair<Type, int>(
                    typeof(SnapColliderRightOfConditional),
                    0);
        public static KeyValuePair<Type, int> Array = new KeyValuePair<Type, int>(
                    typeof(SnapColliderArray),
                    0);
        public static KeyValuePair<Type, int> Conditional = new KeyValuePair<Type, int>(
                    typeof(SnapColliderConditional),
                    0);
        public static KeyValuePair<Type, int> LeftNumber = new KeyValuePair<Type, int>(
                    typeof(SnapColliderLeftNumber),
                    0);
        public static KeyValuePair<Type, int> RightNumber = new KeyValuePair<Type, int>(
                    typeof(SnapColliderRightNumber),
                    0);
        public static KeyValuePair<Type, int> Printable = new KeyValuePair<Type, int>(
                    typeof(SnapColliderPrintable),
                    0);
        public static KeyValuePair<Type, int> Value = new KeyValuePair<Type, int>(
                    typeof(SnapColliderValue),
                    0);
        public static KeyValuePair<Type, int> Variable = new KeyValuePair<Type, int>(
                    typeof(SnapColliderVariable),
                    0);
    }
}
