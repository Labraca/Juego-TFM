using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshSizeController : MonoBehaviour
{
    private TextMesh _targetText;
    private int _currentSize;
    private FontTags mFontTag;

    public FontTags.mFontTags tagSelected = FontTags.mFontTags.normal;


    private void Awake()
    {
        _targetText = GetComponent<TextMesh>();
        if (_targetText == null)
            throw new NullReferenceException();

        _targetText.fontSize = PlayerPrefs.GetInt("fontTam");
        _currentSize = _targetText.fontSize;
        mFontTag.SetSelected(tagSelected);
    }

    public void UpdateSize(int NewFontTam, FontTags _sizeTags)
    {
        if (_currentSize == NewFontTam || _targetText == null)
            return;

        _sizeTags.SetSelected(mFontTag.GetSelected());
        mFontTag = _sizeTags;

        _currentSize = NewFontTam + mFontTag.GetSizeOffset();
        _targetText.fontSize = _currentSize;
    }

    private void OnEnable()
    {
        AccesibilityManager.OnSizeChanged += UpdateSize;

        UpdateSize(PlayerPrefs.GetInt("fontTam"), mFontTag);
    }

    private void OnDisable()
    {
        AccesibilityManager.OnSizeChanged -= UpdateSize;
        UpdateSize(PlayerPrefs.GetInt("fontTam"), mFontTag);
    }
}
