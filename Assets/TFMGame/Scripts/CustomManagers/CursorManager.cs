using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        Cursor.visible = true;
        Screen.lockCursor = false;
        Cursor.visible = true;
#endif
    }
}
