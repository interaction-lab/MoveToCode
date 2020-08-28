using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class ExerciseInformationSeekingActions : MonoBehaviour {
        public CodeBlock[] snapChild;
        public CodeBlock[] snapParent;
        public enum SNAPACTIONS {
            REMOVE,
            SNAPATZERO,
            SNAPATONE,
            SNAPATTWO,
            SNAPATTHREE
        }
        public SNAPACTIONS[] withAction;

        public static GameObject goOfFocus;

        public virtual string DoISAAction() {
            int probRange = 10;
            string result = "";
            int count = 0;
            if (GetComponent<Exercise>() as FreePlayExercise != null) {
                probRange = 20;
            }
            while (result == "" && count < 5) {
                int rand = Random.Range(0, probRange);
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
                    result = FreePlayMenuManager.instance.FakePressRandomButton();
                }
                ++count;
            }

            return result;
        }

        string SpawnVariable() {
            string[] varNames = GetComponent<Exercise>().GetExerciseInternals().GetVarNames();
            if (varNames.Length == 0) {
                return "";
            }
            string varToSpawn = varNames[Random.Range(0, varNames.Length)];
            FakePressButton fpb = MemoryManager.instance.GetVariables()[varToSpawn].GetComponent<FakePressButton>();
            if (fpb == null) {
                fpb = MemoryManager.instance.GetVariables()[varToSpawn].gameObject.AddComponent<FakePressButton>();
            }
            return fpb.PressButton();
        }

        string SnapNextSnapISA() {
            Assert.IsTrue(snapChild.Length == snapParent.Length && snapParent.Length == withAction.Length);
            int index = FindNextSnapIndex();
            if (index == -1) {
                return "";
            }
            return PerformFakeSnapAction(index);
        }

        public int ConvertSnapActionToIntPos(SNAPACTIONS sa) {
            return (int)sa - 1;
        }

        private string PerformFakeSnapAction(int actionIndex) {
            int snapArgIndex = ConvertSnapActionToIntPos(withAction[actionIndex]);
            if (withAction[actionIndex] != SNAPACTIONS.REMOVE) {
                snapParent[actionIndex].GetSnapColliders().GetSnapColliderAtPos(snapArgIndex).DoSnapAction(snapParent[actionIndex], snapChild[actionIndex], false);
                return string.Join("", "Add ", snapChild[actionIndex].name,
                                    " from ", snapParent[actionIndex].name,
                                     " at ", snapArgIndex.ToString());
            }
            else {
                IARG childArg = GetChildArg(actionIndex);
                
                snapParent[actionIndex].SetArg(childArg, null, false);
                return string.Join("","Remove ", snapChild[actionIndex].name,
                                      " from ", snapParent[actionIndex].name,
                                      " at ", childArg.ToString());
            }
        }

        private IARG GetChildArg(int index) {
            return snapParent[index].GetArgDescriptionOfArg(snapChild[index].GetMyIArgument());
        }

        private int FindNextSnapIndex() {
            for (int actionIndex = 0; actionIndex < snapChild.Length; ++actionIndex) {
                if (withAction[actionIndex] != SNAPACTIONS.REMOVE) {
                    if (GetChildArg(actionIndex) == IARG.NotFound) {
                        return actionIndex;
                    }
                }
                else {
                    if (GetChildArg(actionIndex) != IARG.NotFound) {
                        return actionIndex;
                    }
                }

            }
            return -1;
        }
    }
}
