using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class StringDataType : IDataType {
        public StringDataType(string valIn) {
            SetValue(valIn);
        }

    }
}
