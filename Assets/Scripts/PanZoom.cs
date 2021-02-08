using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
	Vector3 touchFirst;

    // Update is called once per frame
    void Update()
    {
        
    	if(Input.GetMouseButtonDown(0)){ //starts at the very beggining of the touch
            touchFirst = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0)){ //continues panning during drag
            Vector3 direction = touchFirst - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        
    }
}
