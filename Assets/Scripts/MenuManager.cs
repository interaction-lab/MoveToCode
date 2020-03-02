using Microsoft.MixedReality.Toolkit.UI;

namespace MoveToCode {
    public class MenuManager : Singleton<MenuManager> {
        PressableButtonHoloLens2 playButton;

        public PressableButtonHoloLens2 GetPlayButton() {
            if (playButton == null) {
                playButton = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<PressableButtonHoloLens2>();
            }
            return playButton;
        }

        public string FakePressPlay() {
            FakePressButton fakePressButton = GetPlayButton().GetComponent<FakePressButton>();
            if (fakePressButton == null) {
                fakePressButton = GetPlayButton().gameObject.AddComponent<FakePressButton>();
            }
            return fakePressButton.PressButton();
        }
    }
}
