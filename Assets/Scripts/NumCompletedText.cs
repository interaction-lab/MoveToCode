using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MoveToCode {
    public class NumCompletedText : MonoBehaviour {
        int numCompleted = 0;
        TextMeshProUGUI _textMesh;
        TextMeshProUGUI TXT {
            get {
                if (_textMesh == null) {
                    _textMesh = GetComponent<TextMeshProUGUI>();
                }
                return _textMesh;
            }

        }

        void Start(){
            ExerciseManager.instance.OnExerciseCorrect.AddListener(UpdateText);
            ClearText();
        }

        void UpdateText(){
            ++numCompleted;
            TXT.text = "Completed: " + numCompleted.ToString();;
        }

        void ClearText(){
            TXT.text = "Completed: 0";
        }
    }
}
