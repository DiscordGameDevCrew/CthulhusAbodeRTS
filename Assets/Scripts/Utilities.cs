using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities{

    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if(_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }


    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        Utilities.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Utilities.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Utilities.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Utilities.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate Corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Bounds GetViewportBounds(Camera cam, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = cam.nearClipPlane;
        max.z = cam.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static Vector3[] GetBoxFormation(int numUnits, int width, int depth, float spaceX, float spaceY)
    {
        List<Vector3> vec = new List<Vector3>();
        float x = 0;
        float y = 0;
        int newDepth = Mathf.CeilToInt((float)numUnits / (float)width);
        int unitCount = numUnits;

        Vector3 v = Vector3.zero;

        for (int iy = 0; iy < newDepth; iy++)
        {
            v.z = -iy;
            v.x = 0;
            for (int ix = 0; ix < width; ix++)
            {
                if(unitCount == 0)
                {
                    break;
                }
                
                if (ix % 2 == 0)
                {
                    v.x += ix;
                }
                else
                {
                    v.x -= ix;
                }

                vec.Add(v);

                unitCount--;
            }
        }

        return vec.ToArray();
    }
}
