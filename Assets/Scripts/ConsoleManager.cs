using TMPro;
namespace MoveToCode {
    public class ConsoleManager : Singleton<ConsoleManager> {
        TextMeshProUGUI mainConsole, headerConsole;
        public static string finishedString = "Code Finished";

        void Awake() {
            ClearConsole();
        }

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

        public void AddLine(string lineToAdd) {
            // reroute to kuri textmanager
            // KuriTextManager.instance.Addline(lineToAdd); -> old, now just not doing anythign for this
            // GetMainConsole().text = string.Join("", GetMainConsole().text, lineToAdd, "\n"); // vestigial but maybe useful later, rerouting everything to have kuri say it for now
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
