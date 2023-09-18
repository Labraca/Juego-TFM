using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class PathfindingDisabler : MonoBehaviour
{
    public bool canPathfindOnClick = false;
    public void PathDenier()
    {
        if(!canPathfindOnClick)
            AC.KickStarter.player.EndPath();
    }
}
