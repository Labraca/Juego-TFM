using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AC;
public class OnClickSkipSpeech : MonoBehaviour
{
    public void SkipSpeechOnClick()
    {
        KickStarter.playerInput.SimulateInputButton("SkipSpeech");
    }
}
