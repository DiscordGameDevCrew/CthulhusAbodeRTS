using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager _Instance;

    public Transform _CenterContent;
    public GameObject _UnitCard;

    public List<int> _IDRegistry = new List<int>();

    void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void AddUnitCard(Sprite s, int _id)
    {
        GameObject g = (GameObject)Instantiate(_UnitCard);
        g.transform.name = _id.ToString();
        g.transform.SetParent(_CenterContent, false);
        g.GetComponentInChildren<Image>().sprite = s;
    }

    public void RemoveUnitCard(int _id)
    {
        for (int i = 0; i < _CenterContent.childCount; i++)
        {
            if (_CenterContent.GetChild(i).transform.name == _id.ToString())
            {
                Destroy(_CenterContent.GetChild(i).gameObject);
                break;
            }
        }
    }

    public void ClearUnitCards()
    {
        int len = _CenterContent.childCount;
        for (int i = 0; i < len; i++)
        {
            Destroy(_CenterContent.GetChild(0).gameObject);
        }
    }

    public bool CheckRegistry(int id)
    {
        if (_IDRegistry.Contains(id))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddToRegistry(int id)
    {
        _IDRegistry.Add(id);
    }

    public void RemoveFromRegistry(int id)
    {
        _IDRegistry.Remove(id);
    }
}
