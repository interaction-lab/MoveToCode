using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace MoveToCode
{
    public class Block2TextConsoleManager : Singleton<Block2TextConsoleManager>
    {
        StandAloneInstruction curInstruction;

        void Awake() {
            ClearConsole();
        }

        public void UpdateConsoleOnSnap() {//every time a block is snapped, the console is updated with code/text     
            Block2TextConsoleManager.instance.ClearConsole();
            curInstruction = StartCodeBlock.instance.GetMyInternalIArgument() as StandAloneInstruction;
            while (curInstruction != null){
                Block2TextConsoleManager.instance.AddLine(curInstruction?.DescriptiveInstructionToString());
                curInstruction = curInstruction.GetNextInstruction();
            }

        }

            /* <------copied from ConsoleManager------>*/
        TextMeshProUGUI mainConsole, headerConsole;
        public static string finishedString = "Code Finished";

        public TextMeshProUGUI GetHeaderConsole() {
            if (headerConsole == null) {
                headerConsole = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            }
            return headerConsole;
        }

        public TextMeshProUGUI GetMainConsole() {
            if (mainConsole == null) {
                mainConsole = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }
            return mainConsole;
        }

        public void ClearConsole() {
            GetMainConsole().text = "";
        }

        public void SetHeaderText(string t) {
            GetHeaderConsole().text = t;
        }

        public void AddLine(string lineToAdd){
            GetMainConsole().text = string.Join("", GetMainConsole().text, lineToAdd, "\n");
        }

        public string GetHeaderText() {
            return GetHeaderConsole().text;
        }

        public string GetCleanedMainText() {
            return GetMainText().Replace(finishedString, "").ReplaceFirst(StartInstruction.startString, "").Replace("\n", "");
        }

        public string GetMainText() {
            return GetMainConsole().text;
        }

        public void AddFinishLine() {
            AddLine(finishedString);
        }
    }
}
