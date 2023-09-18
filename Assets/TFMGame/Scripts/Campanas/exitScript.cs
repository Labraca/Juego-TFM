using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class exitScript : MonoBehaviour
{
    public void ExitPuzzle()
    {
        AC.KickStarter.TurnOnAC();

        SceneChanger sc = AC.KickStarter.sceneChanger;
        int prevScn = sc.GetPreviousSceneIndex();

        sc.ChangeScene(prevScn, true);
    }
}
