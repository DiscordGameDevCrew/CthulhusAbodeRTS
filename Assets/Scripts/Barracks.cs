using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Barracks : MonoBehaviour {

    public Transform waypoint;
    public GameObject[] unitList;
    private float radius;

	// Use this for initialization
	void Start () {
		if(waypoint == null)
        {
            waypoint = transform.Find("DestMarker");
        }

        StartCoroutine(BuildUnits());
	}
	
    IEnumerator BuildUnits()
    {
        while (true)
        {
            Vector3 point = Vector3.zero;
            /*while(Vector3.Distance(point, transform.position) <= radius)
            {
                point = Random.insideUnitSphere * (radius + 3);
            }*/

            // Get the direction vector of our waypoint
            point = (waypoint.position - transform.position).normalized;
            point *= 2;
            point += transform.position;

            GameObject newUnit = (GameObject)Instantiate(unitList[0], point, Quaternion.identity);
            newUnit.GetComponentInChildren<AIPath>().SetDest(waypoint.position);
            Camera.main.transform.GetComponent<SelectionScript>().army.Add(newUnit);
            yield return new WaitForSeconds(2.5f);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
