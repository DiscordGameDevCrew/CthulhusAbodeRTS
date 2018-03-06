using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour {

    public Sprite _UnitPortrait;
    public string _UnitName;
    public int _UnitID;

	// Use this for initialization
	void Start () {
        while (UIManager._Instance._IDRegistry.Contains(_UnitID))
        {
            _UnitID = Random.Range(-10000, 10000);
        }
        UIManager._Instance.AddToRegistry(_UnitID);
        transform.name = _UnitID.ToString();
	}
}
