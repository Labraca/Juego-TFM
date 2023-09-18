using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextSizeController : MonoBehaviour
{
    private Text _targetText;
    private int _currentSize;
    private FontTags mFontTag = new FontTags();
    private RectTransform parentRect;
    private bool TooWide = false; 
    public FontTags.mFontTags tagSelected = FontTags.mFontTags.normal;



    
    private void Awake()
    {
        _targetText = GetComponent<Text>();
        if (_targetText == null)
            throw new NullReferenceException();

        _targetText.fontSize = PlayerPrefs.GetInt("fontTam");
        _currentSize = _targetText.fontSize;
        parentRect = GetComponent<RectTransform>();

        mFontTag.SetSelected(tagSelected);
    }
   /* private void Start()
    {
        int limit = _currentSize/2;
        while (IsText2Wide())
        {
            TooWide = true;

            _currentSize = _currentSize - 1;
            _targetText.fontSize = _currentSize;
            if (limit >= _currentSize)
                break;
        }
    }
    */
    public void UpdateSize(int NewFontTam,FontTags _sizeTags)
    {
        if (_currentSize == NewFontTam || _targetText == null || TooWide)
            return;

        _sizeTags.SetSelected(mFontTag.GetSelected());
        mFontTag = _sizeTags;

        _currentSize = NewFontTam + mFontTag.GetSizeOffset();
        _targetText.fontSize = _currentSize;
    }

    private void OnEnable()
    {
        AccesibilityManager.OnSizeChanged += UpdateSize;

        UpdateSize(PlayerPrefs.GetInt("fontTam"),mFontTag);
    }

    private void OnDisable()
    {
        AccesibilityManager.OnSizeChanged -= UpdateSize;
        UpdateSize(PlayerPrefs.GetInt("fontTam"),mFontTag);
    }

    private bool IsText2Wide()
    {
        float textWidth = LayoutUtility.GetPreferredWidth(_targetText.rectTransform); //T$$anonymous$$s is the width the text would LIKE to be
        float parentWidth = parentRect.rect.width; //T$$anonymous$$s is the actual width of the text's parent container
        return (textWidth > parentWidth); //is the text too wide?  Stop when the next character could be too wide
    }
}

