using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDraw : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawIcon(transform.position, "Light Gizmo.tiff", true);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawIcon(transform.position, "Light Gizmo.tiff", true);
    }
}
