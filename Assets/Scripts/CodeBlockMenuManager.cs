using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;
namespace MoveToCode {
    public class CodeBlockMenuManager : Singleton<CodeBlockMenuManager> {

        private CodeBlockFamily currentCodeBlockFamily = null;

        public void SetFamily(CodeBlockFamily family) {
            HidePreviousActive();
            currentCodeBlockFamily = family;
            ShowCurrentActive();
        }

        public void HidePreviousActive() {
            currentCodeBlockFamily?.HideFamily();
        }

        private void ShowCurrentActive() {
            currentCodeBlockFamily?.ShowFamily();
        }

        public void TurnMenuOn() {
            gameObject.SetActive(true);
        }

        private void OnEnable() {
            GetComponent<ObjectManipulator>().RemoveTwoHandedScaling();
        }

        public void TurnMenuOff() {
            gameObject.SetActive(false);
        }
    }
}