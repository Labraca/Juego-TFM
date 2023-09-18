using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play_sound : MonoBehaviour
{
    [SerializeField]
    AudioSource aS;
    
    public void play_audio(AudioClip audioClip)
    {
        aS.PlayOneShot(audioClip);
    }


}
