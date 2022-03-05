using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class DialogueData : MonoBehaviour
{
    public List<BubbleCell> cells;
    public bool isConnected;
    public bool isMultipleChoice;
    public UnityEvent eventAfterDone;

    public bool secondTimeShowFast = true;

    [ShowIf("isConnected")]
    public DialogueData next;

    [ShowIf("isMultipleChoice")]
    public DialogueData[] choices;

    public bool isUnlockable;

    [ShowIf("isUnlockable")]
    public Sprite unlockedEmoji;
    void Start()
    {
        var childs = GetComponentsInChildren<BubbleCell>().ToList();
        
        childs.Sort((a, b) =>
        {
            float diff = 0;
            if (Mathf.Abs(a.transform.position.y - b.transform.position.y) > 0.3f)
                diff = b.transform.position.y - a.transform.position.y;
            else
                diff = a.transform.position.x - b.transform.position.x;
            return (int) (diff * 10000);
        });

        cells = childs;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        Gizmos.color = Color.yellow;

        if (!isUnlockable)
            return;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawWireSphere(transform.position, 0.4f);

    }

    void Update()
    {
        
    }
}
