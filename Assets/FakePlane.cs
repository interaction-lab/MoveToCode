using UnityEngine;

namespace MoveToCode {
    public class FakePlane : MonoBehaviour {
       void Awake(){
           #if !UNITY_EDITOR
              Destroy(gameObject);
           #endif
       }
    }
}
