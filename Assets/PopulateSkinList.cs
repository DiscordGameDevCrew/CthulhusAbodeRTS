using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.IO;

public class PopulateSkinList : MonoBehaviour {

    public Transform entryList;     // Where do our mod entries go?
    public GameObject entryPrefab;  // What is our prefab?
    private string path;            // Path to mod folder.
    private string defaultInfo = "Unknown\nUnknown\nSkin Mod";  // Default entry. Will be used if author does not include info.txt
    private int infoLength = 3; // # of entries into the description block (Author & Description)

    public List<Transform> children;

	public void PopulateSkins()
    {
        ScanFolder();
    }

    private async void ScanFolder()
    {
        // Wooo async!
        // Scans our mod folder, and when that is done we create our mod entries.
        await ZipManager.Instance.ScanModFolderAsync();
        await CreateEntries();
        SortChildrenAlpha(entryList);
    }

    private async Task CreateEntries()
    {
        // Clean up our old entries.
        foreach (Transform child in entryList)
        {
            Destroy(child.gameObject);
        }

        // Get the path of our skins folder. Then get all subfolders (mods) in that directory.
        //TODO: MAKE MODULAR! That way we can reuse this script for all mods.
        path = Utilities.GetModFolder("Skins");
        DirectoryInfo dir = new DirectoryInfo(path);
        DirectoryInfo[] mods;
        mods = dir.GetDirectories();

        foreach (DirectoryInfo mod in mods)
        {
            // Create an entry for the mod. Get references to the necessary text components.
            GameObject entry = (GameObject)Instantiate(entryPrefab, entryList);
            Text descriptionBlock = Utilities.FindChildRecursive(entry.transform, "Info").GetComponentInChildren<Text>();
            Text modTitle = Utilities.FindChildRecursive(entry.transform, "EntryNameText").GetComponent<Text>();
            string modName;

            entry.GetComponentInChildren<ModInfo>()._mod = mod;

            if(descriptionBlock != null && modTitle != null)
            {
                // If our mod has an info.txt we can just read it and have CreateDescriptionText do the heavy lifting for us.
                // If it doesn't, we'll have to brute force it a bit with default values.
                if (mod.GetFiles("info.txt").Length > 0)
                {
                    // Read our info.txt file and then pass it to CreateDescriptionText.
                    string modInfo = File.ReadAllText(mod.FullName + "/info.txt");
                    modName = CreateDescriptionText(modInfo, 0);
                    modName = modName.Replace(@"\", string.Empty);
                    modTitle.text = modName;
                    entry.name = modName;
                    descriptionBlock.text = CreateDescriptionText(modInfo, 1);
                }
                else
                {
                    // Create a name out of the folder title and use the default description values for everything else.
                    string defaultName = mod.Name;
                    defaultName = defaultName.Replace("_", " ");
                    defaultName = defaultName.Replace(@"\", string.Empty);
                    defaultName = defaultName.Replace("Skin ", string.Empty);
                    entry.name = defaultName;
                    descriptionBlock.text = CreateDescriptionText(defaultInfo, 1);
                    modTitle.text = defaultName;
                }
            }
            else
            {
                Debug.LogError("Error writing mod info.");
                return;
            }
        }        
    }

    private void SortChildrenAlpha(Transform root)
    {
        children = new List<Transform>();
        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            children.Add(child);
        }

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetParent(null);  // Become batman
        }

        if (children.Count > 1)
        {
            children.Sort(CompareTransform);

            for (int i = 0; i < children.Count; i++)
            {
                children[i].SetParent(root);
            }

            for (int i = 0; i < children.Count; i++)
            {
                Char[] c = children[i].name.ToCharArray();
                if (children[i].name == "Default Skin\r")
                {
                    children[i].SetSiblingIndex(0);
                }
            }
            //children[0].SetParent(root);
        }
    }

    private static int CompareTransform(Transform a, Transform b)
    {
        string aa = a.name;
        string bb = b.name;
        aa = aa.Replace(@"\", string.Empty);
        bb = bb.Replace(@"\", string.Empty);
        return aa.CompareTo(bb);
    }
    public string CreateDescriptionText(string input, int ret)
    {
        // Ret determines what we want to return.
        // 0 Returns the mod title.
        // 1 Returns the mod description.

        string content = input;
        string[] entries = new string[infoLength];

        entries = content.Split('\n');
        // entries[0]: Author
        // entries[1]: Mod Title
        // entries[2]: Mod Description

        if(entries.Length < infoLength)
        {
            Debug.LogError("Error: info.txt does not have all required fields!");
            return "ERROR IN info.txt REQUIRED FIELDS MISSING.";
        }

        switch (ret)
        {
            case 0:
                string title = entries[1];
                //title = title.TrimStart("Skin".ToCharArray());
                return entries[1];
            case 1:
                string author = "Author: ";
                author += entries[0];

                string description = "Description: ";
                description += entries[2];

                return string.Format("{0}\n{1}", author, description);
            default:
                return "ERROR IN RET. INVALID INDEX.";
        }
    }
}
