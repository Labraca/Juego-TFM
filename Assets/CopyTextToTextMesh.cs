using UnityEngine;

using System.Collections;

using AC;

using UnityEngine.UI;

using TMPro;

public class CopyTextToTextMesh : MonoBehaviour
{
    [SerializeField]
    public Text textToCopyFrom;
    [SerializeField]
    public TextMeshProUGUI textMeshToCopyTo;





    private void Update()

    {

        textMeshToCopyTo.text = textToCopyFrom.text;

    }



}
