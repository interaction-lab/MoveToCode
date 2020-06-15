namespace MoveToCode {

    public class ArrayCodeBlock : DataCodeBlock {
        
        public enum ARRAYTYPE {
            INT,
            FLOAT,
            STRING,
            CHAR,
            BOOL
        }

        public int arraySize = 3;
        public ARRAYTYPE at;

        public void SetArrayType(ARRAYTYPE arrType) {
            at = arrType;
        }

        protected override void SetMyBlockInternalArg() {
            switch (at) {
                case ARRAYTYPE.INT:
                    myBlockInternalArg = new ArrayDataStructure(this, arraySize, typeof(IntDataType));
                    break;
                case ARRAYTYPE.FLOAT:
                    myBlockInternalArg = new ArrayDataStructure(this, arraySize, typeof(FloatDataType));
                    break;
                case ARRAYTYPE.STRING:
                    myBlockInternalArg = new ArrayDataStructure(this, arraySize, typeof(StringDataType));
                    break;
                case ARRAYTYPE.CHAR:
                    myBlockInternalArg = new ArrayDataStructure(this, arraySize, typeof(CharDataType));
                    break;
                case ARRAYTYPE.BOOL:
                    myBlockInternalArg = new ArrayDataStructure(this, arraySize, typeof(BoolDataType));
                    break;
            }
        }
    }
}