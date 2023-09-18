using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AC;
using System;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class escene1_3_Trigger : MonoBehaviour
{
    [SerializeField]
    GameObject pipa;
    [SerializeField]
    Hotspot cajas;
    [SerializeField]
    Conversation conv;

    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GVar escene1_2 = GlobalVariables.GetVariable("isEscena1_2Started");
        GVar hanoi = GlobalVariables.GetVariable("isHanoiGameBeated");

        if (escene1_2.GetValue().Equals("True"))
        {

            if (pipa != null) pipa.SetActive(true);
            Debug.Log(conv.GetOptionNameWithID(4));
            conv.TurnOptionOn(4);
            conv.SetOptionState(4, false, false);

            if (hanoi.GetValue().Equals("True"))
            {
                cajas.SetButtonState(cajas.GetUseButton(0), false);
            }
            else
            {

                cajas.SetButtonState(cajas.GetUseButton(0), true);
            }
        }
        else
        {
           // conv.TurnOptionOff(4);
        }
    }

}
