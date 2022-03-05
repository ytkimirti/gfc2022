using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonDialog : MonoBehaviour
{
    public bool isSpeakerDialogue;
    public DialogueData helloDialogue;
    public DialogueData[] options;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var option in options)
        {
            RecurseGizmos(null, option);
        }
    }

    void RecurseGizmos(DialogueData last ,DialogueData node)
    {
        if (!node)
            return;
        if (last)
            Gizmos.DrawLine(node.transform.position, last.transform.position);
        Gizmos.DrawWireSphere(node.transform.position, 0.4f);
        RecurseGizmos(node,node.next);
    }
}
