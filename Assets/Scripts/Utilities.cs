using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class Utilities{


    #region Box Selecting
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
        // float x = 0;
        // float y = 0;
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
    #endregion


    public static void CopyTo(this Stream input, Stream output)
    {
        byte[] buffer = new byte[16 * 1024];
        int bytesRead;

        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, bytesRead);
        }
    }

    public static bool StringArrayContains(string[] arr, string value)
    {
        foreach (string s in arr)
        {
            if (s.ToLower().Contains(value.ToLower()))
            {
                if(s.Length == value.Length)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static string GetModFolder(string modType)
    {
        return string.Format("{0}/Mods/{1}",
            Application.dataPath.Remove(Application.dataPath.IndexOf("/Assets")),
            modType);
    }

    public static Transform FindChildRecursive(Transform start, string name)
    {
        // Breadth-First Search

        var result = start.Find(name);
        if (result != null)
            return result;
        foreach(Transform child in start)
        {
            result = Utilities.FindChildRecursive(child, name);
            if (result != null)
                return result;
        }

        // Debug.LogError(string.Format("Unable to find child {0} in parent {1}!", name, start.name));
        return null;
    }

    public static Sprite LoadPNG(string path)
    {
        Sprite s = null;
        Texture2D tex = null;
        byte[] data;

        if (File.Exists(path))
        {
            data = File.ReadAllBytes(path);
            tex = new Texture2D(1, 1, TextureFormat.RGB24, false);
            tex.LoadImage(data);        // LoadImage will automatically resize the Texture2d.

            s = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            s.name = tex.name;
        }

        return s;
    }

    public static void SortChildrenAlpha(Transform root)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in root)
        {
            Debug.LogWarning(child);
            children.Add(child);
            child.SetParent(null);  // Become batman
        }

        Debug.Log(children.Count);

        if(children.Count > 2)
        {
            Debug.Log("bbbbbbbb");
            children.Sort((Transform t1, Transform t2) => { Debug.Log(t1.name + " " + t2.name); return t1.name.CompareTo(t2.name); });
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name != "Default Skin")
                {
                    children[i].SetParent(root);
                    children.Remove(children[i]);
                }
            }

            children[0].SetParent(root);
        }
        else
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name != "Default Skin")
                {
                    children[i].SetParent(root);
                    children.Remove(children[i]);
                }
            }
            if(children.Count > 0)
                children[0].SetParent(root);
        }
    }
}
