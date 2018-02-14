using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstantiator : MonoBehaviour {

    public GameObject testObject;
    public int numCopies;
    int i = 0;
	void Start () {
        StartCoroutine(Spawn());
	}

    IEnumerator Spawn()
    {
        for (int i = 0; i < numCopies; i++)
        {
            Vector3 circle = Random.insideUnitCircle * 5;
            Vector3 point = new Vector3(circle.x, 0, circle.y);
            point += transform.position;
            GameObject go = (GameObject)Instantiate(testObject, point, Quaternion.identity);
            go.transform.name = "Mesh: " + i.ToString();
            i++;
            yield return new WaitForSeconds(0.1f);
        }    
    }
}
