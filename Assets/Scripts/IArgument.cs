namespace MoveToCode {
    public abstract class IArgument {
        public abstract IDataType EvaluateArgument();
        public abstract CodeBlock GetCodeBlock();
        public abstract void SetCodeBlock(CodeBlock codeBlock);
        public virtual void ResestInternalState() { }
        public abstract int GetNumArguments();
    }
}