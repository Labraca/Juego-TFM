using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;
public class OnClickActionList : MonoBehaviour
{
    [SerializeField]public ActionList actionList;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // Construct a ray from the current touch coordinates
            Ray ray = new Ray(Vector3.zero, Vector3.zero);

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif (UNITY_IOS || UNITY_ANDROID)
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1);
            if (hit.collider != null && hit.collider.gameObject.name == gameObject.name)
            {
                actionList.Interact();
            }
        }
    }
}
