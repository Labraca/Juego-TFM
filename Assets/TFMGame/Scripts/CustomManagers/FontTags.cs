using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FontTags
{
    public int _h1 = 3;
    public int _h2 = 2;
    public int _normal = 0;
    public int _small = -2;

    public enum mFontTags
    {
        h1,
        h2,
        normal,
        small
    };

    private mFontTags selected = mFontTags.normal;

    public mFontTags GetSelected()
    {
        return selected;
    }

    public void SetSelected(mFontTags value)
    {
        selected = value;
    }

    public int GetSizeOffset()
    {
        switch (selected)
        {
            case mFontTags.h1:
                return _h1;
            case mFontTags.h2:
                return _h2;
            case mFontTags.normal:
                return _normal;
            case mFontTags.small:
                return _small;
            default:
                return _normal;
        }
    }
}
