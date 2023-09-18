using AC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesibilityManager : MonoBehaviour
{
    public static int fontTam;
    public delegate void SizeChange(int NewfontTam, FontTags sizesTag);
    public static event SizeChange OnSizeChanged;

    public string defaultTam = "Normal";
    public static string targetFontTam;

    [SerializeField]
    public int normalFontSize = 19;
    [SerializeField]
    public int pequenoFontSize = 16;
    [SerializeField]
    public int grandeFontSize = 22;
    [SerializeField]
    public int extraGrandeFontSize = 24;

    public FontTags mFontTags;

    private void Awake()
    {
        //Obtener la variable de tamaño de textos
        if (!PlayerPrefs.HasKey("fontTam"))
            initPrefs();

        fontTam = PlayerPrefs.GetInt("fontTam");
        //targetFontTam = fontTam;
        OnSizeChanged?.Invoke(fontTam,mFontTags);

        EventManager.OnMenuElementClick += mOnMenuElementClick;
        
    }

    private void mOnMenuElementClick(Menu _menu, MenuElement _element, int _slot, int buttonPressed)
    {
        if (!((_menu.title.Equals("SubtitulosOpciones")|_menu.title.Equals("Options")) && _element.title.Equals("fontTamButton")))
            return;

        int newFontTam = ParserFontLabel2Size(_element.GetLabel(_slot,0).Split(':')[1].Trim());
        
        if (fontTam != newFontTam)
        {
            
           // targetFontTam = newFontTam;
            fontTam = newFontTam;
            OnSizeChanged?.Invoke(fontTam,mFontTags);

            PlayerPrefs.SetInt("fontTam", fontTam);
            PlayerPrefs.Save();
        }
            
        
    }

    private void initPrefs()
    {
        PlayerPrefs.SetInt("fontTam",ParserFontLabel2Size(defaultTam));
        PlayerPrefs.Save(); 
    }

    private int ParserFontLabel2Size(string _label)
    {
        
        switch (_label)
        {
            case "Pequeña":
                return pequenoFontSize;
            case "Normal":
                return normalFontSize;
            case "Grande":
                return grandeFontSize;
            case "Extragrande":
                return extraGrandeFontSize;
            default:
                throw new Exception("No existe ese tamaño");
        }
    }
}
