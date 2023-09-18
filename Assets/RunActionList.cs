using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunActionList : MonoBehaviour
{
    [SerializeField]
    public ActionList runnableActionList;

    public void RunActionlistObject()
    {
        if (runnableActionList == null)
        {
            Debug.LogError("ActionList null");
            return;
        }

        runnableActionList.Interact();
    }
    public void RunActionListObject(ActionList actionList)
    {
        if (actionList == null)
        {
            Debug.LogError("ActionList null");
            return;
        }

        actionList.Interact();
    }
    public void RunActionListObject(GameObject actionList)
    {
        if (actionList == null) {
            Debug.LogError("ActionList null");
            return;
        }
        if (actionList.GetComponent<ActionList>() == null)
        {
            Debug.LogError(actionList.name+" no es un ActionList");
        }

        actionList.GetComponent<ActionList>().Interact();
    }
}
