using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using AC;
public class DiffObject: MonoBehaviour
{
    public SpriteRenderer _marca;
    public bool isActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        _marca = _marca.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isActive)
            return;
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
            if (GetComponent<Collider2D>() == overlaped)
            {
                Debug.LogWarning("Marcado");
                _marca.enabled = true;
                isActive = true;
            }
        }
        

#endif
        //El código de abajo hace lo mismo que el de arriba pero con el tactil en caso de jugarse en movil
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
            if (touch.phase == TouchPhase.Began)
            {
                if (GetComponent<Collider2D>() == overlaped)
                {
                    _marca.enabled = true;
                    isActive = true;
                }
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
