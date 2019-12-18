using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {

    public class NULLArgument : IArgument {
        public IDataType EvaluateArgument() {
            return null;
        }
    }
}
