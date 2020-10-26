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
        /// Returns My IArgument if in dict8ionary
        /// </summary>
        /// <param name="iARGIn">String representation of argument lookup</param>
        /// <returns>My IArgument</returns>
        public IArgument GetArgument(string iARGIn) {
            if (GetArgToSnapColliderDict().ContainsKey(iARGIn))
                return GetArgToSnapColliderDict()[iARGIn]?.GetMyCodeBlockArg()?.GetMyIArgument();
            return null;
        }

        public static Dictionary<string, HashSet<Type>> iArgCompatabilityDict =
            new Dictionary<string, HashSet<Type>> {
                { "Next", new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { "LeftOfConditional", new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { "RightOfConditional", new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { "ArrayElement", new HashSet<Type> { typeof(BasicDataType) } },
                { "Array", new HashSet<Type> { typeof(Variable) }  },
                { "ArrayDataStructure", new HashSet<Type> {  typeof(ArrayDataStructure) }  },
                { "Variable", new HashSet<Type> { typeof(Variable) , typeof(ArrayIndexInstruction) }  },
                { "Nested", new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { "Conditional", new HashSet<Type> {  typeof(ConditionalInstruction) }  },
                { "Value", new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  },
                { "Printable", new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  },
                { "LeftNumber", new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  },
                { "RightNumber", new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  }
            };


        public virtual HashSet<Type> GetArgCompatibility(string argDescription) {
            if (iArgCompatabilityDict.ContainsKey(argDescription))
                return iArgCompatabilityDict[argDescription];
            
            return null;
        }


    }
}