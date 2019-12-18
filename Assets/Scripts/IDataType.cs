namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public dynamic value;

        public IDataType EvaluateArgument() {
            return this;
        }

        public dynamic GetValue() {
            return value;
        }
        public void SetValue(dynamic valIn) {
            value = valIn;
        }
    }
}