using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour {

    bool draggingCamera = false;
    Vector3 lastMousePos;

    float scrollMod = 5;

	
	void Update () {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // At which point does the mouse ray intersect y=0?
        if (mouseRay.direction.y >= 0)
        {
            Debug.LogError("Error in mouse ray direction");
            return;
        }

        float rayLength = mouseRay.origin.y / mouseRay.direction.y;
        Vector3 hitPos = mouseRay.origin - (mouseRay.direction * rayLength);

        if (Input.GetMouseButtonDown(2))
        {
            draggingCamera = true;

            lastMousePos = hitPos;
        }
        else if(Input.GetMouseButtonUp(2))
        {
            draggingCamera = false;
        }

        if (draggingCamera)
        {
            Vector3 diff = lastMousePos - hitPos;
            Camera.main.transform.Translate(diff, Space.World);

            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            // At which point does the mouse ray intersect y=0?
            if (mouseRay.direction.y >= 0)
            {
                Debug.LogError("Error in mouse ray direction");
                return;
            }

            rayLength = mouseRay.origin.y / mouseRay.direction.y;
            lastMousePos = hitPos = mouseRay.origin - (mouseRay.direction * rayLength);
        }

        float scrollAmt = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollAmt) > 0.01f)
        {
            Vector3 dir = Vector3.down * scrollMod;

            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, Camera.main.transform.position + dir * scrollAmt, scrollMod * Time.deltaTime);

            Camera.main.transform.Translate(dir * scrollAmt, Space.World);
        }
	}
}
