using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }


    public void MoveMe() {
        transform.localPosition += Vector3.up;
    }

    // Update is called once per frame
    void Update() {

    }
}
