using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class UISwitcher : MonoBehaviour {

    //TODO: Use default sprite if it is missing from the mod folder.

    #region Default UI Elements
    public Sprite dBAR_TOP, dBAR_BOTTOM, dMINIMAP_BORDER, dPADDING_L, dPADDING_R, dTICKER_BAR_CONTAINER, dTICKER_BAR_FG, dTICKER_BAR_HIGHLIGHT, dWINDOW_ABILITY, dWINDOW_CENTER;
    #endregion

    #region UI Images
    public List<Image> uiImages;
    private Image iBarTop, iBarBottom, iMinimapBorder, iPaddingL, iPaddingR, iTickerBarContainer, iTickerBarFG, iTickerBarHighlight, iWindowAbility, iWindowCenter;
    #endregion

    private string DEFAULT_NAME = "default";

    private Transform uiIngame;


    private void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        new List<Image>(10);
        uiIngame = GameObject.FindGameObjectWithTag("PlayerUI").transform;

        if(uiIngame == null)
        {
            Debug.LogError("Error: Could not find ingame UI!");
            return;
        }


        iBarTop = Utilities.FindChildRecursive(uiIngame, "Bar_Top").GetComponent<Image>();
        iBarBottom = Utilities.FindChildRecursive(uiIngame, "Bar_Bottom").GetComponent<Image>();
        iMinimapBorder = Utilities.FindChildRecursive(uiIngame, "Minimap_Border").GetComponent<Image>();
        iPaddingL = Utilities.FindChildRecursive(uiIngame, "Padding_L").GetComponent<Image>();
        iPaddingR = Utilities.FindChildRecursive(uiIngame, "Padding_R").GetComponent<Image>();
        iTickerBarContainer = Utilities.FindChildRecursive(uiIngame, "TickerBarContainer").GetComponent<Image>();
        iTickerBarFG = Utilities.FindChildRecursive(uiIngame, "TickerBarFG").GetComponent<Image>();
        iTickerBarHighlight = Utilities.FindChildRecursive(uiIngame, "TickerBarHighlight").GetComponent<Image>();
        iWindowAbility = Utilities.FindChildRecursive(uiIngame, "Window_Ability").GetComponent<Image>();
        iWindowCenter = Utilities.FindChildRecursive(uiIngame, "Window_Center").GetComponent<Image>();

        uiImages.Add(iBarTop);
        uiImages.Add(iBarBottom);
        uiImages.Add(iMinimapBorder);
        uiImages.Add(iPaddingL);
        uiImages.Add(iPaddingR);
        uiImages.Add(iTickerBarContainer);
        uiImages.Add(iTickerBarFG);
        uiImages.Add(iTickerBarHighlight);
        uiImages.Add(iWindowAbility);
        uiImages.Add(iWindowCenter);

        uiImages = uiImages.OrderBy(img=>img.gameObject.name).ToList();
    }

    public void SwitchUI(DirectoryInfo mod)
    {
        // Debug.Log(mod.Name);
        string path = Utilities.GetModFolder("Skins") + "/" + mod.Name;

        if(mod.Name == "Skin_Default")
        {
            SetUIElements(dBAR_TOP, dBAR_BOTTOM, dMINIMAP_BORDER, dPADDING_L, dPADDING_R, dTICKER_BAR_CONTAINER, dTICKER_BAR_FG, dTICKER_BAR_HIGHLIGHT, dWINDOW_ABILITY, dWINDOW_CENTER);
        }
        else
        {
            FileInfo[] f = mod.GetFiles();

            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < f.Length; i++)
            {
                // Debug.Log(f[i].Extension.ToLower());
                if (f[i].Extension.ToLower() == ".png")
                {
                    Debug.Log(f[i].Name);
                    sprites.Add(Utilities.LoadPNG(f[i].FullName));
                }
            }

            sprites = sprites.OrderBy(sp=>sp.name).ToList();

            if(sprites.Count < uiImages.Count)
            {
                //TODO: Revert to default
                Debug.LogError("Error: Skin folder " + mod.Name + " does not contain enough images.");
                return;
            }

            for (int i = 0; i < uiImages.Count; i++)
            {
                uiImages[i].sprite = sprites[i];
            }
        }

        Debug.Log("Skin Changed.");
    }

    private void SetUIElements(Sprite barTop, Sprite barBottom, Sprite minimapBorder, Sprite paddingL, Sprite paddingR, Sprite tickerContainer, Sprite tickerFG, Sprite tickerHL, Sprite windowAbility, Sprite windowCenter)
    {
        iBarTop.sprite = barTop;
        iBarBottom.sprite = barBottom;
        iMinimapBorder.sprite = minimapBorder;
        iPaddingL.sprite = paddingL;
        iPaddingR.sprite = paddingR;
        iTickerBarContainer.sprite = tickerContainer;
        iTickerBarFG.sprite = tickerFG;
        iTickerBarHighlight.sprite = tickerHL;
        iWindowAbility.sprite = windowAbility;
        iWindowCenter.sprite = windowCenter;
    }
}
