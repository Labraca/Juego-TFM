using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    [SerializeField]
    public ActionList runnableActionList;
    public void ExitSceneNoAC()
    {
        AC.KickStarter.TurnOnAC();

        if (runnableActionList == null)
        {
            Debug.LogError("ActionList null");
            return;
        }

        runnableActionList.Interact();

    }
}
