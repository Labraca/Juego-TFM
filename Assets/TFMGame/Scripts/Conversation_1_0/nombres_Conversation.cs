using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using AC;

public class nombres_Conversation : MonoBehaviour
{

    [SerializeField]
    public AC.Conversation mConversation;
    [SerializeField]
    public TextAsset mNombres;
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.OnStartConversation += StartConversation;
    }

    void OnDisable()
    {
        EventManager.OnStartConversation -= StartConversation;
    }

    void StartConversation(Conversation conversation)
    {
        if (conversation.Equals(mConversation))
        {
            string nombresFile = mNombres.text;
            string[] nombres = Regex.Split(nombresFile, "\n");
            int[] idArray = mConversation.GetIDArray();
            mConversation.options.Clear();
            var added = new ArrayList();
            
            for (int i = 0; i < 4; i++)
            {
                int j = Random.Range(0, nombres.Length - 1);

                if (!added.Contains(j))
                {
                    ButtonDialog newOption = new ButtonDialog(idArray);
                    newOption.label = nombres[j];
                    newOption.conversationAction = ConversationAction.RunOtherConversation;
                    mConversation.options.Add(newOption);
                    added.Add(j);
                }

            }
            added.Clear();
        }


    }

    
}
