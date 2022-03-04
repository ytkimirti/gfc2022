using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleChoiceButton : MonoBehaviour
{
    public Transform gridTrans;
    private DialogueData currData;
    public float buttonWidth = 0.8f;

    public void Init(DialogueData data)
    {
        currData = data;
        int i = 0;
        foreach (BubbleCell bubbleCell in data.cells)
        {
            
            GameObject go = Instantiate(bubbleCell.gameObject, transform.position, Quaternion.identity, null);
            
            go.transform.SetParent(gridTrans, true);

            float offset = -buttonWidth + (i + 1) * ((buttonWidth * 2) / (data.cells.Count + 1));
            
            go.transform.localPosition = Vector2.right * offset;
            
            go.GetComponent<BubbleCell>().GoOverUI();
            i++;
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
