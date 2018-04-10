using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StabilityColor : MonoBehaviour {

    public Color32 criticalColor, lowColor, moderateColor, neutralColor, highColor, fullColor;
    private Image _fill;

	// Use this for initialization
	void Start () {
        _fill = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        int x = (int)(_fill.fillAmount*100);

        if (x <= 19)
        {
            _fill.color = criticalColor;
        }
        else if (x > 19 && x <= 39)
        {
            _fill.color = lowColor;
        }
        else if (x > 39 && x <= 59)
        {
            _fill.color = moderateColor;
        }
        else if (x > 59 && x <= 79)
        {
            _fill.color = neutralColor;
        }
        else if (x > 79 && x <= 99)
        {
            _fill.color = highColor;
        }
        else if (x == 100)
        {
            _fill.color = fullColor;
        }
	}
}
