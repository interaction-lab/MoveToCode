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

        public string DoISAAction() {
            // press play button
            MenuManager.instance.FakePressPlay();
            goOfFocus = MenuManager.instance.GetPlayButton().gameObject;
            // snap action
            //SnapNextFromTo();
            // move start block action
            // make new variable
            //SpawnVariable();

            // spawn new codeblock
            // TODO: Freeplay exercise must be done first
            return "";
        }

        string SpawnVariable() {
            string[] varNames = GetComponent<Exercise>().varNames;
            if (varNames.Length == 0) {
                return "";
            }
            string varToSpawn = varNames[Random.Range(0, varNames.Length)];
            FakePressButton fpb = MemoryManager.instance.GetVariables()[varToSpawn].GetComponent<FakePressButton>();
            if (fpb == null) {
                fpb = MemoryManager.instance.GetVariables()[varToSpawn].gameObject.AddComponent<FakePressButton>();
            }
            fpb.PressButton();
            return string.Join("", "Var Made ", varToSpawn);
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
                return string.Join("",
                                "Add ",
                                snapChild[actionIndex].name,
                                " from ",
                                snapParent[actionIndex].name,
                                " at ",
                                snapArgIndex.ToString());
            }
            else {
                int childIndex = GetChildArgPos(actionIndex);
                snapParent[actionIndex].SetArgumentBlockAt(null, childIndex, false);
                return string.Join("",
                                "Remove ",
                                snapChild[actionIndex].name,
                                " from ",
                                snapParent[actionIndex].name,
                                " at ",
                                childIndex.ToString());
            }
        }

        private int GetChildArgPos(int index) {
            return snapParent[index].GetPositionOfArgument(snapChild[index].GetMyInternalIArgument());
        }

        private int FindNextSnapIndex() {
            for (int actionIndex = 0; actionIndex < snapChild.Length; ++actionIndex) {
                if (withAction[actionIndex] != SNAPACTIONS.REMOVE) {
                    if (GetChildArgPos(actionIndex) == -1) {
                        return actionIndex;
                    }
                }
                else {
                    if (GetChildArgPos(actionIndex) != -1) {
                        return actionIndex;
                    }
                }

            }
            return -1;
        }
    }
}
