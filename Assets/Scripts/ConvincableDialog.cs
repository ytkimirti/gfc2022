using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvincableDialog : MonoBehaviour
{
    public DialogueData firstNode;

    private void OnDrawGizmos()
    {
        if (!firstNode)
            return;
        RecurseDrawGizmos(new List<DialogueData>(),null, firstNode);
    }

    void RecurseDrawGizmos(List<DialogueData> visitedNodes ,DialogueData lastNode, DialogueData node)
    {
        if (node == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(firstNode.transform.position, 0.5f);
        if (lastNode != null)
        {
            Gizmos.DrawLine(lastNode.transform.position, node.transform.position);
            for (int i = 0; i < 6; i++)
            {
                Gizmos.DrawWireSphere(Vector3.Lerp(lastNode.transform.position, node.transform.position, 0.93f - i * 0.03f), 0.01f + i * 0.02f);
            }
        }   
        if (visitedNodes.Contains(node))
            return;
        visitedNodes.Add(node);
        if (node.isConnected)
            RecurseDrawGizmos(visitedNodes,node, node.next);
        else if (node.isMultipleChoice)
            foreach (var n in node.choices)
                RecurseDrawGizmos(visitedNodes,node ,n);
    }

    void FindChilds()
    {
            
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
