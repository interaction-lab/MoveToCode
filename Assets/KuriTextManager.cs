using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class KuriTextManager : Singleton<KuriTextManager> {
        struct TextCommand {
            public COMMANDS commandType;
            public PRIORITY priority;
            public string text;
            public TextCommand(COMMANDS cIn, PRIORITY pIn, string tIn) {
                commandType = cIn;
                priority = pIn;
                text = tIn;
            }
            public override string ToString() {
                return string.Join("", "C: ", commandType.ToString(), " P: ", priority.ToString(), " T: ", text);
            }
        }

        public float textTypingTime = 0.05f;
        static string textLogCol = "KuriDialogue";
        enum COMMANDS {
            add,
            erase
        }
        public enum PRIORITY {
            high,
            low
        }

        TextMeshProUGUI kuriTextMesh;
        Queue<TextCommand> commandQueue;
        Queue<TextCommand> highPriorityCommands;
        int curCommandNum, ticketCommandNum;

        void Setup() {
            commandQueue = new Queue<TextCommand>();
            highPriorityCommands = new Queue<TextCommand>();
            kuriTextMesh = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            curCommandNum = 0;
            ticketCommandNum = curCommandNum;
            LoggingManager.instance.AddLogColumn(textLogCol, "");
        }

        Queue<TextCommand> GetCommandQueue() {
            if (commandQueue == null) {
                Setup();
            }
            return commandQueue;
        }

        Queue<TextCommand> GetHighPriorityQueue() {
            if (commandQueue == null) {
                Setup();
            }
            return commandQueue;
        }

        public void Addline(string lIn = "", PRIORITY pIn = PRIORITY.low) {
            GetCommandQueue().Enqueue(new TextCommand(COMMANDS.add, pIn, string.Join("", lIn, "\n")));
            StartCoroutine(ProcessText(ticketCommandNum++));
        }

        public void Clear(PRIORITY pIn = PRIORITY.low) {
            GetCommandQueue().Enqueue(new TextCommand(COMMANDS.erase, pIn, ""));
            StartCoroutine(ProcessText(ticketCommandNum++));
        }

        IEnumerator ProcessText(int myCommandNum) {
            yield return new WaitUntil(() => curCommandNum == myCommandNum);

            TextCommand processTuple = commandQueue.Peek();
            commandQueue.Dequeue();
            if (processTuple.commandType == COMMANDS.add) {
                if (processTuple.priority == PRIORITY.high) {
                    highPriorityCommands.Enqueue(processTuple);
                }
                foreach (char letter in processTuple.text) {
                    kuriTextMesh.text += letter;
                    yield return new WaitForSeconds(textTypingTime);
                }
            }
            else if (processTuple.commandType == COMMANDS.erase) {
                kuriTextMesh.text = "";
                if (processTuple.priority == PRIORITY.low) {
                    foreach (TextCommand txtCmd in highPriorityCommands) {
                        kuriTextMesh.text += txtCmd.text;
                    }
                }
                else {
                    highPriorityCommands.Clear();
                }
            }
            LoggingManager.instance.UpdateLogColumn(textLogCol, processTuple.ToString());

            ++curCommandNum;
        }

    }
}