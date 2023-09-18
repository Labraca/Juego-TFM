using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class Tecla : MonoBehaviour
{
    Animator _animator;
    AudioSource _campanaAudio;
    Animator _campanaAnimator;
    Animator _campanaGlowAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _campanaAnimator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        _campanaAudio = GetComponentInChildren<AudioSource>();
        _campanaGlowAnimator = _campanaAnimator.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        if (_campanaAnimator == null)
            Debug.LogError("No se ha encontrado el animador de campanas");
        if(_campanaGlowAnimator == null)
            Debug.LogError("No se ha encontrado el animador de campanas");

        //GetAnimatorStateInfo(_campanaAnimator.gameObject);
    }

    void Update()
    {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        //Dependiendo de la acción con el ratón se ejecuta la animacion de presionar tecla y hacer sonar la campana o no
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D overlaped = Physics2D.OverlapPoint(wp);
            if (overlaped == null)
            {
                Debug.LogWarning("Ningun collider encontrado");
                return;
            }
            if (GetComponent<Collider2D>() == overlaped )
            {

                _animator.SetBool("isTeclaPlayed", true);
                _campanaAnimator.Play("Sonando",0,0f);
                _campanaAudio.PlayOneShot(_campanaAudio.clip);
                _campanaGlowAnimator.Play("glowsonando",0,0f);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D overlaped = Physics2D.OverlapPoint(wp);
            if (overlaped == null)
            {
                Debug.LogWarning("Ningun collider encontrado");
                return;
            }
            if (GetComponent<Collider2D>() != overlaped)
            {
                _animator.SetBool("isTeclaPlayed", false);
            }
            else if (!_animator.GetBool("isTeclaPlayed"))
            {
                _animator.SetBool("isTeclaPlayed", true);
                _campanaAnimator.Play("Sonando", 0,0f);
                _campanaAudio.PlayOneShot(_campanaAudio.clip);
                _campanaGlowAnimator.Play("glowsonando", 0, 0f);
            }
        }
        if (Input.GetMouseButtonUp(0))
            _animator.SetBool("isTeclaPlayed", false);
        
#endif
        //El código de abajo hace lo mismo que el de arriba pero con el ratón en caso de jugarse en movil
#if (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            Collider2D overlaped = Physics2D.OverlapPoint(touchPos);
            Debug.Log("Si que lo detecta");
            if (overlaped == null)
            {
                Debug.LogWarning("Ningun collider encontrado");
                return;
            }
            switch (touch.phase)
                    {
                    case TouchPhase.Began:
                        if (GetComponent<Collider2D>() == overlaped)
                        {
                            _animator.SetBool("isTeclaPlayed", true);
                            _campanaAnimator.Play("sonando",0);
                            _campanaAudio.PlayOneShot(_campanaAudio.clip);
                            _campanaGlowAnimator.Play("glowsonando", 0, 0f);
                        }
                        break;
                    case TouchPhase.Moved:
                        if (GetComponent<Collider2D>() != overlaped)
                        {
                            _animator.SetBool("isTeclaPlayed", false);
                        }
                        else if (!_animator.GetBool("isTeclaPlayed"))
                        {
                            _animator.SetBool("isTeclaPlayed", true);
                            _campanaAnimator.Play("sonando",0);
                            _campanaAudio.PlayOneShot(_campanaAudio.clip);
                            _campanaGlowAnimator.Play("glowsonando", 0, 0f);
                        }
                        break;
                    case TouchPhase.Ended:
                        _animator.SetBool("isTeclaPlayed", false);
                        break;
                    }
                }
        
#endif

    }
/*
    public List<AnimatorState> GetAnimatorStateInfo(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] acLayers = ac.layers;
        List<AnimatorState> allStates = new List<AnimatorState>();
        foreach (AnimatorControllerLayer i in acLayers)
        {
            ChildAnimatorState[] animStates = i.stateMachine.states;
            foreach (ChildAnimatorState j in animStates)
            {
                allStates.Add(j.state);
                Debug.Log("Found a state called " + j.state.name + " with a speed of " + j.state.speed + " with a length of " + j.state.motion.averageDuration);
            }
        }
        return allStates;
    }
    */
}
