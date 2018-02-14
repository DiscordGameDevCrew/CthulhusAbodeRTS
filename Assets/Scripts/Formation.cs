using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Formation : MonoBehaviour {

    public Transform[] positions;
    public List<GameObject> units = new List<GameObject>();

    public Vector3[] test;
	// Use this for initialization
	void Start () {
        test = Utilities.GetBoxFormation(10, 5, 2, -1, -1);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetComponent<AIPath>().destination != positions[i].position)
                units[i].GetComponent<AIPath>().SetDest(positions[i].position);
            else if (Vector3.Distance(units[i].transform.position, positions[i].position) <= 0.15f)
                units[i].transform.rotation = Quaternion.Slerp(units[i].transform.rotation, positions[i].rotation, Time.deltaTime * 4f);
            //units[i].GetComponent<AIPath>().destRot = positions[i].rotation;
        }
    }
}
