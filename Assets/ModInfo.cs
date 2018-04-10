using UnityEngine;
using System.IO;

public class ModInfo : MonoBehaviour {

    private DirectoryInfo mod;
    public DirectoryInfo _mod { get { return mod; } set { mod = value;} }

    public void SwitchUI()
    {
        //! YIKES
        //TODO: FIX ME.
        ZipManager.Instance.gameObject.GetComponent<UISwitcher>().SwitchUI(_mod);
    }
}
