using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SelectionScript : MonoBehaviour {
    
    //TODO: Change so that instead of checking one point, it checks for multiple points per unit. That way we can select the unit by having the box intersect at any point in the bounding box
    //TODO: EX. search for (0,0,0) (.5,0,0) (1,0,0) (-.5,0,0) (-1,0,0) etc.

    bool isSelecting = false;
    bool boxSelect = false;
    Vector3 mousePos1;

    public List<GameObject> army = new List<GameObject>();
    public List<AIPath> selectedUnits = new List<AIPath>();

    Vector3 lastMousePos;
    float mouseMoveDist;

    public LayerMask selectionLM;

    UIManager uiManager;

	// Use this for initialization
	void Start () {
        uiManager = UIManager._Instance;
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            uiManager.ClearUnitCards();
            selectedUnits.Clear();
            mousePos1 = Input.mousePosition;
        }else if (Input.GetMouseButtonUp(0))
        {
            if (Utilities.GetScreenRect(mousePos1, Input.mousePosition).size.magnitude < .5f)
            {
                //Debug.Log("Clicked");
                foreach (GameObject go in army)
                {
                    go.GetComponentInChildren<Projector>().enabled = false;
                    uiManager.RemoveUnitCard(go.GetComponentInChildren<UnitData>()._UnitID);
                }
                Click();
            }
            isSelecting = false;
            boxSelect = false;
            mouseMoveDist = 0;
        }

        if (isSelecting)
        {
            mouseMoveDist = Vector3.Distance(lastMousePos, Input.mousePosition);

            if (mouseMoveDist > 1)
                boxSelect = true;

            if(boxSelect)
                SelectArmy();
        }

        lastMousePos = Input.mousePosition;
    }

    void OnGUI()
    {
        if(isSelecting && boxSelect)
        {
            // Create a rect from both mouse positions
            var rect = Utilities.GetScreenRect(mousePos1, Input.mousePosition);
            Utilities.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utilities.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
            //Debug.Log(rect.size);
            //Debug.Log(rect.size.magnitude);
        }
    }

    private void Click()
    {
        if (army.Count == 0)
            return;

        Ray selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity, selectionLM))
        {
            hit.transform.GetComponentInChildren<Projector>().enabled = true;
            uiManager.AddUnitCard(hit.transform.gameObject.GetComponentInChildren<UnitData>()._UnitPortrait, hit.transform.gameObject.GetComponentInChildren<UnitData>()._UnitID);
            selectedUnits.Add(hit.transform.GetComponent<AIPath>());
        }
    }

    private void SelectArmy()
    {
        if (army.Count == 0)
            return;

        foreach (GameObject go in army)
        {
            if (IsWithinSelectionBoundsVector(go.transform.position))
            {
                if (!selectedUnits.Contains(go.GetComponent<AIPath>()))
                {
                    go.GetComponentInChildren<Projector>().enabled = true;
                    uiManager.AddUnitCard(go.GetComponentInChildren<UnitData>()._UnitPortrait, go.GetComponentInChildren<UnitData>()._UnitID);
                    selectedUnits.Add(go.GetComponent<AIPath>());
                }
            }
            else
            {
                selectedUnits.Remove(go.GetComponent<AIPath>());
                uiManager.RemoveUnitCard(go.GetComponentInChildren<UnitData>()._UnitID);
                go.GetComponentInChildren<Projector>().enabled = false;
            }
        }
    }

    private Vector3[] GetUnitSelectionBounds(Vector3 center, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        points.Add(center);
        points.Add(new Vector3(center.x + radius, center.y, center.z));
        points.Add(new Vector3(center.x + radius, center.y, center.z + radius));
        points.Add(new Vector3(center.x + radius, center.y, center.z - radius));

        points.Add(new Vector3(center.x - radius, center.y, center.z));
        points.Add(new Vector3(center.x - radius, center.y, center.z + radius));
        points.Add(new Vector3(center.x - radius, center.y, center.z - radius));

        points.Add(new Vector3(center.x, center.y, center.z + radius));
        points.Add(new Vector3(center.x, center.y, center.z - radius));


        return points.ToArray();
    }

    public bool IsWithinSelectionBounds(GameObject go)
    {
        if (!isSelecting)
        {
            Debug.Log("Not selecting...");
            return false;
        }

        Camera camera = Camera.main;
        Bounds viewportBounds = Utilities.GetViewportBounds(camera, mousePos1, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(go.transform.position));
    }

    public bool IsWithinSelectionBoundsVector(Vector3 v3)
    {
        if (!isSelecting)
        {
            Debug.Log("Not selecting...");
            return false;
        }

        Camera camera = Camera.main;
        Bounds viewportBounds = Utilities.GetViewportBounds(camera, mousePos1, Input.mousePosition);

        bool t = false;
        foreach (Vector3 v in GetUnitSelectionBounds(v3, 0.5f))
        {
            if(viewportBounds.Contains(camera.WorldToViewportPoint(v)))
            {
                t = true;
                break;
            }
        }

        return t;
    }
}
