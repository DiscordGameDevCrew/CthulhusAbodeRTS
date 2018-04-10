using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

#region UnityZip Liscense
/*
* https://github.com/tsubaki/UnityZip
* https://code.google.com/p/ziparchive/
* 
* The MIT License (MIT)
* 
* Copyright (c) 2013 yamamura tatsuhiko
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy of
* this software and associated documentation files (the "Software"), to deal in
* the Software without restriction, including without limitation the rights to
* use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
* the Software, and to permit persons to whom the Software is furnished to do so,
* subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
* FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
* COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
* IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
* CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

public class ZipManager : MonoBehaviour {

    #region Singleton Stuff
    private static ZipManager _instance;
    public static ZipManager Instance { get { return _instance; } }
    #endregion

    private string modFolder;           // Where is our mods folder?
    private string installedDirectory;  // Where should installed mods go?

    private Dictionary<string, string> modTypes = new Dictionary<string, string>(); // Mod `prefix / subfolder` dictionary.

    List<FileInfo> modsToInstall = new List<FileInfo>();    // List of all currently uninstalled mods.

    void Awake()
    {
        #region Singleton Stuff
        if (_instance != null && _instance != this)
            Destroy(this);
        else
            _instance = this;
        #endregion

        // Set our mod folder.
        modFolder = Application.dataPath.Remove(Application.dataPath.IndexOf("/Assets")) + "/Mods";

        // Set up our mod types dictionary.
        // This basically links our mod prefix to the correct folder.
        // i.e. a mod named 'Skin_x' would go into the 'skins' directory.
        modTypes.Add("skin", "skins");
    }

    void Start () {
        // Load up our mod folder and scan for any new mods.
        // ScanModFolder();
	}
	
    public void ScanModFolder()
    {
        LoadModsToInstall();
    }

    public async Task ScanModFolderAsync()
    {
        LoadModsToInstall();
    }

    private void LoadModsToInstall()
    {
        // Load up the mods directory.
        Debug.Log("Scanning Mod Folder for Uninstalled Mods... (" + modFolder + ")");
        DirectoryInfo dir = new DirectoryInfo(modFolder);

        // Clear our 'Mods to Install' list. Then add every folder with the .zip file extention.
        //TODO: Make `LoadModsToInstall` run every time the window comes into focus. This way the player doesn't need to restart to get their new mods!
        modsToInstall.Clear();
        modsToInstall.AddRange(dir.GetFiles("*.zip"));

        if (modsToInstall.Count > 0)
            Debug.Log(String.Format("Scanning completed. Number of uninstalled mods: {0}.", modsToInstall.Count));
        else
            return;

        // Write every mod discovered to the console, and then begin decompressing.
        foreach (FileInfo f in modsToInstall)
        {
            Debug.Log("   - " + f.FullName);
        }

        foreach (FileInfo f in modsToInstall)
        {
            DecompressArchive(f);
        }
    }

    private void DecompressArchive(FileInfo fileToDecompress)
    {
        // Make sure our archive is actuall valid.
        if (fileToDecompress == null)
        {
            Debug.LogError("Archive missing or corrupt!");
            return;
        }

        // Get the mod type.
        //! ALL MODS SHOULD BE NAMED AS SUCH: ModIdentifier_ModName
        //! Mods can include underscores in their name, but the identifier MUST correspond to the dictionary.
        string modType = fileToDecompress.Name.Split('_')[0].ToLower();

        // If our dictionary doesn't contain the identifier, this is an unsupported mod type. We should return before something breaks.
        if (!modTypes.ContainsKey(modType))
        {
            Debug.LogError(string.Format("{0} is not a valid mod type! Type: {1}", fileToDecompress.Name, modType));
            return;
        }

        // modTypeFolder is just for the value of our dictionary key (the mod identifier).
        string modTypeFolder;
        modTypes.TryGetValue(modType, out modTypeFolder);

        if (string.IsNullOrEmpty(modTypeFolder))
        {
            Debug.LogError(string.Format("Unable to find folder of mod {0}.", modType));
            return;
        }

        // Here we go. Now we can finally begin to create our final directory. We merge our mod folder and our identifier folder.
        installedDirectory = string.Format("{0}/{1}", modFolder, modTypeFolder);
        //Debug.Log(installedDirectory);

        // Take the file name "xxx.zip", remove the extension, and append that to our directory.
        // We are also replacing all instances of spaces with underscores.
        // This is so that the Mod Manager can turn the folder name into a mod name if info.txt is missing.
        string modFolderName = fileToDecompress.Name.Remove(fileToDecompress.Name.IndexOf(fileToDecompress.Extension));
        modFolderName = modFolderName.Replace(" ", "_");

        // This is our final directory. We want to drop the contents of the zip into the respective folder.
        installedDirectory += "/" + modFolderName;

        // Create our mod folder. If for some reason the folder already exists we may run into some issues.
        //TODO: Handle conflicting mods.
        Debug.Log("Creating folder for mod... " + installedDirectory);
        System.IO.Directory.CreateDirectory(installedDirectory);

        // Decompress our archive.
        //! https://github.com/tsubaki/UnityZip
        Debug.Log("Decompressing archive and writing to folder... " + fileToDecompress.Name);
        ZipUtil.Unzip(fileToDecompress.FullName, installedDirectory);

        // The mod is installed. We *could* delete its archive. We can also just unzip our mods during gameplay.
        // This would likely lead to a longer load time on startup with more mods.
        //TODO: Decide. For now I am going to delete the archives because we're done with them.
        Debug.Log("Mod installed. Deleting archive...");
        File.Delete(fileToDecompress.FullName);
    }
}
