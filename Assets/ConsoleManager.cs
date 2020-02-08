using TMPro;
namespace MoveToCode {
    public class ConsoleManager : Singleton<ConsoleManager> {
        TextMeshProUGUI mainConsole, headerConsole;

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
            GetMainConsole().text = string.Join("", GetMainConsole().text, lineToAdd, "\n");
        }

        public string GetHeaderText() {
            return GetHeaderConsole().text;
        }

        public string GetMainText() {
            return GetMainConsole().text;
        }
    }
}
