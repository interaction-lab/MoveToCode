using System;
using System.Collections.Generic;

namespace MoveToCode {

    /// <summary>
    /// Base abstract class for all internal "arguments" of codeblocks. All instructions and data types are treated as arguments so they can be placed as arguments of other codeblocks. E.g., a `IntCodeBlock` can be placed as a left or right argument to a `MathCodeBlock`
    /// </summary>
    public abstract class IArgument {

        /// <summary>
        /// Pointer to the `CodeBlock` container that this is internal to. This is needed within internal arguments as a backwards pointer. `Codeblock.cs` also has a `myInternalIArgument` which is a pointer the other direction.
        /// </summary>
        public CodeBlock MyCodeBlock { get; set; }

        /// <summary>
        /// Maps SnapCollider string to it's respective SnapCollider class. See `SnapCollider.cs` for more details.
        /// </summary>
        protected Dictionary<string, SnapCollider> argToSnapColliderDict = new Dictionary<string, SnapCollider>();

        /// <summary>
        /// For DataTypes, this is used to get a return value e.g., return the int of the `IntCodeBlock`. For instructions, it is called before running an IArgument. E.x. `MathInstruction.cs` evaluates the left and right arguments and stores their returns. If the left argument is another `MathInstruction`, it will evaulate the whole instruction before running `RunInstruction`.
        /// </summary>
        /// <returns>IDatatype of internal IArgument value e.g., 1 or "Hello World"</returns>
        public abstract IDataType EvaluateArgument();

        /// <summary>
        /// Gives description within python code syntax
        /// </summary>
        /// <returns>String to be printed as python code</returns>
        public abstract string DescriptiveInstructionToString();

        /// <summary>
        /// When the `Interpreter` restarts running code, any internal instruction state is reset. Ex. see `WhileInstruction.cs`
        /// </summary>
        public virtual void ResestInternalState() {
        }

        /// <summary>
        /// Returns point to codeblock
        /// </summary>
        /// <returns>Pointer to `CodeBlock` container.</returns>
        public CodeBlock GetCodeBlock() {
            return MyCodeBlock;
        }

        /// <summary>
        /// Constructor of `IArgument`
        /// </summary>
        /// <param name="cbIn">CodeBlock pointer to parent codeblock</param>
        public IArgument(CodeBlock cbIn) {
            MyCodeBlock = cbIn;
            ResestInternalState();
        }

        /// <summary>
        /// Adds snapcollider to `argToSnapCollider` dictionary.
        /// </summary>
        /// <param name="snapColDescIn">String key for type of snapcollider argument</param>
        /// <param name="snapCollider">Pointer to `SnapCollider`</param>
        internal void RegisterSnapCollider(string snapColDescIn, SnapCollider snapCollider) {
            GetArgToSnapColliderDict()[snapColDescIn] = snapCollider;
        }

        /// <summary>
        /// Returns argToSnapColliderDict
        /// </summary>
        /// <returns>argToSnapColliderDict</returns>
        public Dictionary<string, SnapCollider> GetArgToSnapColliderDict() {
            return argToSnapColliderDict;
        }

        /// <summary>
        /// Returns `IArgument` when looking up respective `SnapCollider` with index pair
        /// </summary>
        /// <param name="snapArgLookUp">Key, item1: type of `SnapCollider` of argument, item2: index of argument, used for `ArrayIndexInstruction`</param>
        /// <returns></returns>
        public IArgument GetArgument(KeyValuePair<Type,int> snapArgLookUp) {
            return MyCodeBlock.GetSnapColliderGroup().SnapColliderSet[snapArgLookUp]?.MyCodeBlockArg?.GetMyIArgument();
        }
    }
}