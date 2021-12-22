using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class ExerciseInformationSeekingActions : MonoBehaviour {
        public CodeBlock[] snapChild;
        public CodeBlock[] snapParent;
        public enum SNAPARGTYPES {
            REMOVE,
            NEXT,
            NESTED,
            CONDITIONAL,
            LEFTNUM,
            RIGHTNUM,
            LEFTCONDITIONAL,
            RIGHTCONDITIONAL,
            PRINTABLE,
            VALUE,
            VARIABLE
        }
        public SNAPARGTYPES[] snapArgs;

        public static GameObject goOfFocus;

        public virtual string DoISAAction() {
            int probRange = 10;
            string result = "";
            int count = 0;
            if (GetComponent<Exercise>() as FreePlayExercise != null) {
                probRange = 20;
            }
            while (result == "" && count < 5) {
                int rand = UnityEngine.Random.Range(0, probRange);
                if (rand < 4) {
                    result = SnapNextSnapISA();
                }
                else if (rand < 8) {
                    result = SpawnVariable();
                }
                else if (rand < 10) {
                    result = MenuManager.instance.FakePressPlay();
                    goOfFocus = MenuManager.instance.GetPlayButton().gameObject;
                }
                else {
                    result = BlockShelfManager.instance.FakePressRandomButton();
                }
                ++count;
            }
            return result;
        }

        string SpawnVariable() {
            string[] varNames = GetComponent<Exercise>().varNames;
            if (varNames.Length == 0) {
                return "";
            }
            string varToSpawn = varNames[UnityEngine.Random.Range(0, varNames.Length)];
            FakePressButton fpb = MemoryManager.instance.GetVariables()[varToSpawn].GetComponent<FakePressButton>();
            if (fpb == null) {
                fpb = MemoryManager.instance.GetVariables()[varToSpawn].gameObject.AddComponent<FakePressButton>();
            }
            return fpb.PressButton();
        }

        string SnapNextSnapISA() {
            Assert.IsTrue(snapChild.Length == snapParent.Length && snapParent.Length == snapArgs.Length);
            int index = FindNextSnapIndex();
            if (index == -1) {
                return "";
            }
            return PerformFakeSnapAction(index);
        }

        Dictionary<SNAPARGTYPES, Type> snapColCovertMap = new Dictionary<SNAPARGTYPES, Type>(){
            {SNAPARGTYPES.CONDITIONAL, typeof(ConditionalSnapCollider) },
            {SNAPARGTYPES.LEFTCONDITIONAL, typeof(LeftOfConditionalSnapCollider) },
            {SNAPARGTYPES.RIGHTCONDITIONAL, typeof(RightOfConditionalSnapCollider)},
             {SNAPARGTYPES.LEFTNUM, typeof(LeftNumberSnapCollider) },
            {SNAPARGTYPES.RIGHTNUM, typeof(RightNumberSnapCollider) },
            {SNAPARGTYPES.NESTED, typeof(NestedSnapCollider)},
             {SNAPARGTYPES.NEXT, typeof(NextSnapCollider) },
            {SNAPARGTYPES.PRINTABLE, typeof(PrintableSnapCollider) },
            {SNAPARGTYPES.VALUE, typeof(ValueSnapCollider)},
            {SNAPARGTYPES.VARIABLE, typeof(VariableSnapCollider)},
            {SNAPARGTYPES.REMOVE, null}
            };
        public Type ConvertToSnapColClass(SNAPARGTYPES sa) {
            return snapColCovertMap[sa];
        }

        private string PerformFakeSnapAction(int actionIndex) {
            Debug.Log(snapArgs[actionIndex].ToString());
            Type snapArgType = ConvertToSnapColClass(snapArgs[actionIndex]);
            if (snapArgs[actionIndex] != SNAPARGTYPES.REMOVE) {
                snapParent[actionIndex]
                    .GetSnapColliderGroup()
                    .SnapColliderSet[new KeyValuePair<Type, int>(snapArgType, 0)]
                    .DoSnapAction(snapChild[actionIndex], false);

                return string.Join("",
                                "Add ",
                                snapChild[actionIndex].name,
                                " from ",
                                snapParent[actionIndex].name,
                                " at ",
                                snapArgs[actionIndex].ToString());
            }
            else {
                string colliderName = snapChild[actionIndex].GetSnapColliderImAttachedTo().name;
                snapChild[actionIndex].RemoveFromParentSnapCollider(false);
                return string.Join("",
                                "Remove ",
                                snapChild[actionIndex].name,
                                " from ",
                                snapParent[actionIndex].name,
                                " at ",
                                colliderName);
            }
        }


        private int FindNextSnapIndex() {

            for (int actionIndex = 0; actionIndex < snapChild.Length; ++actionIndex) {
                Type snapArgType = ConvertToSnapColClass(snapArgs[actionIndex]);

                if (snapArgs[actionIndex] != SNAPARGTYPES.REMOVE) {
                    IArgument ia = snapParent[actionIndex].GetArgumentFromDict(new KeyValuePair<Type, int>(snapArgType, 0));
                    if (ia == null) {
                        return actionIndex;
                    }
                }
                else {
                    if (snapChild[actionIndex].FindParentCodeBlock() == snapParent[actionIndex]) {
                        return actionIndex;
                    }
                }
            }
            return -1;
        }
    }
}