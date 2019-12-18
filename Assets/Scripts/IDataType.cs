namespace MoveToCode {
    public abstract class IDataType {
        public dynamic value;
        public dynamic GetValue() {
            return value;
        }
        public abstract void SetValue(dynamic valIn);
    }
}