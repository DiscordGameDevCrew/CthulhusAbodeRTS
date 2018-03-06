using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//! For issuing orders to units.
public class OrderScript : MonoBehaviour {

    SelectionScript sScript;
    AIPath[] selectedUnits;

    Transform t;

    int numPoints = 20;

	// Use this for initialization
	void Start () {
        sScript = GetComponent<SelectionScript>();
        t = new GameObject().transform;
    }
	
	// Update is called once per frame
	void Update () {
        selectedUnits = sScript.selectedUnits.ToArray();

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                //foreach (AIPath unit in selectedUnits)
                //{
                //    unit.SetDest(hit.point);
                //}
                Vector3[] dests = Utilities.GetBoxFormation(selectedUnits.Length, 4, -1, 1.5f, 1.5f);

                //Debug.Log(dests.Length + "   " + selectedUnits.Length);
                for (int i = 0; i < selectedUnits.Length; i++)
                {
                    AIPath unit = selectedUnits[i];
                    unit.isStopped = false;
                    Vector3 d = hit.point + dests[i];
                    //Debug.Log(dests[i]);
                    unit.SetDest(d);
                    if (unit.transform.GetComponentInChildren<Animator>().GetBool("IsMoving") == false)
                    {
                        //unit.transform.GetComponentInChildren<Animator>().SetBool("IsMoving", true);
                        //unit.transform.GetComponentInChildren<Animator>().SetTrigger("Update");
                    }    
                }
                // t.position = hit.point;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (AIPath unit in selectedUnits)
            {
                unit.isStopped = true;

                unit.transform.GetComponentInChildren<Animator>().SetBool("IsMoving", false);
                unit.transform.GetComponentInChildren<Animator>().SetTrigger("Update");
            }
        }
    }

    /*void OnDrawGizmos()
    {
            return;

        List<Vector3> points = new List<Vector3>();

        float circleSpeed = 1;
        float forwardSpeed = -1;
        float circleSize = 1;
        float circleGrowSpeed = 0.1f;
        int numUnits = 20;

        points.Add(t.position);
        for (int pointNum = 1; pointNum < numUnits - 1; pointNum++)
        {
            float x = Mathf.Sin(pointNum * circleSpeed) * circleSize;
            float y = Mathf.Cos(pointNum * circleSpeed) * circleSize;
            circleSize += circleGrowSpeed;

            Vector3 finalV = new Vector3(x, 0, y);
            points.Add(finalV);
        }

        Gizmos.color = Color.yellow;
        foreach (Vector3 v in points)
        {
            Gizmos.DrawSphere(t.position + v, 0.1f);
        }
    }*/
}
