using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlightAnim : MonoBehaviour
{
    [SerializeField]
    public GameObject glowCampana_Do;
    [SerializeField]
    public GameObject glowCampana_Mi;
    [SerializeField]
    public GameObject glowCampana_Re;
    [SerializeField]
    public GameObject glowCampana_Sol;
    [SerializeField]
    public AudioSource silbidoSource;
    [SerializeField]
    public AudioClip silbidoAudio;

    public void animarMelodiaSilbido()
    {
        if (!silbidoSource.isPlaying)
        {
            silbidoSource.PlayOneShot(silbidoAudio);
            StartCoroutine(animacionMelodiaSilbido());
        }
    }

    private IEnumerator animacionMelodiaSilbido()
    {
        glowCampana_Do.GetComponent<Animator>().PlayInFixedTime("glowsonando",0,1);
        yield return new WaitForSeconds(1);
        glowCampana_Mi.GetComponent<Animator>().PlayInFixedTime("glowsonando", 0, 0.8f);
        yield return new WaitForSeconds(0.8f);
        glowCampana_Re.GetComponent<Animator>().PlayInFixedTime("glowsonando", 0, 0.9f);
        yield return new WaitForSeconds(0.9f);
        glowCampana_Sol.GetComponent<Animator>().PlayInFixedTime("glowsonando", 0, 1.3f);
        yield return new WaitForSeconds(1.3f);

    }
}
