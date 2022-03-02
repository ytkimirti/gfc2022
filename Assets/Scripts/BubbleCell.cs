using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCell : MonoBehaviour
{
    public bool isNewline = false;
    public float displayTime = 0; //If left zero, it will be displayed with the default display time
    public float cellWidth;
    void Start()
    {
        if (cellWidth == 0)
        {
            cellWidth = transform.localScale.x;
        }
    }

    void Update()
    {
        cellWidth = transform.localScale.x;
        
    }
}
