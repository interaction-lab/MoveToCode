using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ForeachLoopInstruction : SingleControlFlowInstruction {
        private int currPosInArray;

        public ForeachLoopInstruction(CodeBlock cbIn) : base(cbIn) {
            currPosInArray = 0;
        }

        public override InstructionReturnValue RunInstruction() {
            if (!exitInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                exitInstructionAddedToStack = true;
            }
            EvaluateArgumentList();
            if (conditionIsTrue) {
                // put me on top of stack for when foreach loop ends
                currPosInArray++;
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            currPosInArray = 0;
            return null; // done with loop
        }

        public override void EvaluateArgumentList() {
            Variable d = GetArgumentAt(1) as Variable;
            if (d != null && d.GetMyData().GetType() == typeof(ArrayDataStructure)) {
                conditionIsTrue = currPosInArray < (d.GetMyData() as ArrayDataStructure).GetSize();
            }
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                },
                new List<Type> {
                    typeof(IDataType)
                },
                new List<Type> {
                    typeof(StandAloneInstruction)
                }
            };
        }

        public override string ToString() {
            return "foreach value in ";
        }
    }
}