using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class KuriTextManager : Singleton<KuriTextManager> {
        public MeshRenderer[] chestLights;
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
                return string.Join("", "C: ", commandType.ToString(), " P: ", priority.ToString(), " T: ", text.Replace("\n", ""));
            }
        }

        float textTypingTime = 0.05f;
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
        AudioSource audioSource;
        AudioClip computerNoiseClip;
        public Material ledOnMaterial, ledOffMaterial;
        int curCommandNum, ticketCommandNum;

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
            ledOnMaterial = Resources.Load<Material>(ResourcePathConstants.locLedOnMaterial) as Material;
            ledOffMaterial = Resources.Load<Material>(ResourcePathConstants.locLedOffMaterial) as Material;
            chestLights = GetComponentsInChildren<MeshRenderer>();
        }

        public bool IsGlowing(){
            return chestLights[0].material == ledOnMaterial;
        }

        public void ToggleGlow(bool turnOn){
            foreach(MeshRenderer led in chestLights)
            {
                led.material = turnOn ? ledOnMaterial : ledOffMaterial;
            }
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

            LoggingManager.instance.UpdateLogColumn(textLogCol, processTuple.ToString());

            if (processTuple.commandType == COMMANDS.add) {
                if (processTuple.priority == PRIORITY.high) {
                    highPriorityCommands.Enqueue(processTuple);
                }
                audioSource.Play();
                foreach (char letter in processTuple.text) {
                    kuriTextMesh.text += letter;
                    ToggleGlow(!IsGlowing());
                    yield return new WaitForSeconds(textTypingTime);
                }
                audioSource.Pause();
                ToggleGlow(false);
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


            ++curCommandNum;
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
                    "Keep trying!"
                }

            }
        };

        public void SayRandomPositiveAffect(TYPEOFAFFECT toa) {
            Addline(positiveAffectiveDialogue[toa][UnityEngine.Random.Range(0, positiveAffectiveDialogue[toa].Length)]);
        }
    }
}
