using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleChoiceButton : MonoBehaviour
{
    public Transform gridTrans;
    private DialogueData currData;

    public void Init(DialogueData data)
    {
        currData = data;
        foreach (BubbleCell bubbleCell in data.cells)
        {
            Instantiate(bubbleCell.gameObject, transform.position, Quaternion.identity, gridTrans);
        }
    }

    public void OnClick()
    {
        MultipleChoiceController.main.OnClick(currData);
    }

    void Update()
    {
        
    }
}
