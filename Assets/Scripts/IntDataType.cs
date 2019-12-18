using System;

namespace MoveToCode {
    public class IntDataType : IDataType, IArgument {

        public IntDataType(int valIn) {
            SetValue(valIn);
        }

        public IDataType EvaluateArgument() {
            return this;
        }

        public override void SetValue(dynamic valIn) {
            value = Convert.ToInt32(valIn);
        }
    }
}
