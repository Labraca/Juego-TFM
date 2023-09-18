using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLManagerScript : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Debug.Log("Iniciando custom managers");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("CustomManagers")));
    }
}
