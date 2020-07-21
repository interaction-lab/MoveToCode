using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ForeachLoopInstruction : SingleControlFlowInstruction {
        private int currIdxInArray;

        public ForeachLoopInstruction(CodeBlock cbIn) : base(cbIn) {
            currIdxInArray = 0;
        }

        public override int GetNumArguments() {
            return 4;
        }

        public override InstructionReturnValue RunInstruction() {
            if (!exitInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                exitInstructionAddedToStack = true;
            }
            EvaluateArgumentList();
            if (conditionIsTrue) {
                // put me on top of stack for when foreach loop ends
                currIdxInArray++;
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            currIdxInArray = 0;
            return null; // done with loop
        }

        public override void EvaluateArgumentList() {
            Variable leftArg = GetArgumentAt(1) as Variable;
            Variable rightArg = GetArgumentAt(2) as Variable;
            if (rightArg != null && rightArg.GetMyData().GetType() == typeof(ArrayDataStructure)) {
                conditionIsTrue = currIdxInArray < (rightArg.GetMyData() as ArrayDataStructure).GetSize();
                if(conditionIsTrue) {
                    leftArg.SetValue(((rightArg.GetMyData() as ArrayDataStructure).GetValue() as IDataType[])[currIdxInArray]);
                }
            }
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                },
                new List<Type> {
                    typeof(Variable)
                },
                new List<Type> {
                    typeof(Variable)
                },
                new List<Type> {
                    typeof(StandAloneInstruction)
                }
            };
        }

        public override string ToString() {
            return "foreach \nvar ";
        }
    }
}