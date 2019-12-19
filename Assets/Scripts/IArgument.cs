namespace MoveToCode {
    public interface IArgument {
        IDataType EvaluateArgument();
        CodeBlock GetCodeBlock();
        void SetCodeBlock(CodeBlock codeBlock);
    }
}