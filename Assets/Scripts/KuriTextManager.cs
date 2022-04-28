﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class KuriTextManager : Singleton<KuriTextManager> {
        public class TextCommand {
            public COMMANDS commandType;
            public PRIORITY priority;
            public string text;
            public TextCommand(COMMANDS cIn, PRIORITY pIn, string tIn) {
                commandType = cIn;
                priority = pIn;
                text = tIn;
            }
            public override string ToString() {
                return string.Join("", "C: ", commandType.ToString(), " P: ", priority.ToString(), " T: ", text.Replace("\n", ""));
            }
        }

        float textTypingTime = 0.075f;
        static string textLogCol = "KuriDialogue";
        public enum COMMANDS {
            add,
            erase
        }
        public enum PRIORITY {
            high,
            low
        }

        public TextCommand CurTextCommand { get; set; } = null;
        public bool IsTalking {
            get {
                return CurTextCommand != null &&
                       CurTextCommand.commandType == COMMANDS.add;
            }
        }
        TextMeshProUGUI kuriTextMesh;
        Queue<TextCommand> commandQueue;
        Queue<TextCommand> highPriorityCommands;
        AudioSource audioSource;
        AudioClip computerNoiseClip;
        int curCommandNum, ticketCommandNum;

        public string CurText {
            get {
                return kuriTextMesh.text;
            }
        }

        void Setup() {
            commandQueue = new Queue<TextCommand>();
            highPriorityCommands = new Queue<TextCommand>();
            kuriTextMesh = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            curCommandNum = 0;
            ticketCommandNum = curCommandNum;
            LoggingManager.instance.AddLogColumn(textLogCol, "");
            computerNoiseClip = Resources.Load<AudioClip>(ResourcePathConstants.ComputerNoises);
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = computerNoiseClip;
            audioSource.spatialBlend = 1;
            audioSource.loop = true;
            audioSource.volume = 0.05f;
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
            CurTextCommand = commandQueue.Peek();
            commandQueue.Dequeue();

            LoggingManager.instance.UpdateLogColumn(textLogCol, CurTextCommand.ToString());

            if (CurTextCommand.commandType == COMMANDS.add) {
                if (CurTextCommand.priority == PRIORITY.high) {
                    highPriorityCommands.Enqueue(CurTextCommand);
                }
                audioSource.Play();
                foreach (char letter in CurTextCommand.text) {
                    kuriTextMesh.text += letter;
                    yield return new WaitForSeconds(textTypingTime);
                }
                audioSource.Pause();
            }
            else if (CurTextCommand.commandType == COMMANDS.erase) {
                kuriTextMesh.text = "";
                if (CurTextCommand.priority == PRIORITY.low) {
                    foreach (TextCommand txtCmd in highPriorityCommands) {
                        kuriTextMesh.text += txtCmd.text;
                    }
                }
                else {
                    highPriorityCommands.Clear();
                }
            }
            ++curCommandNum;
            CurTextCommand = null; // used for check of not talking
        }

        public enum TYPEOFAFFECT {
            Encouragement,
            Congratulation
        }

        Dictionary<TYPEOFAFFECT, string[]> positiveAffectiveDialogue = new Dictionary<TYPEOFAFFECT, string[]>
        {
            {
                TYPEOFAFFECT.Congratulation,
                new string[] {
                    "Well done!",
                    "Good job!",
                    "Great!",
                    "Excellent!",
                    "Very good!"
                }
            },
            {
                TYPEOFAFFECT.Encouragement,
                new string[] {
                    "You can do this!",
                    "Go ahead!",
                    "Don't give up!",
                    "Keey trying!"
                }

            }
        };

        public void SayRandomPositiveAffect(TYPEOFAFFECT toa) {
            Addline(positiveAffectiveDialogue[toa][UnityEngine.Random.Range(0, positiveAffectiveDialogue[toa].Length)]);
        }
    }
}