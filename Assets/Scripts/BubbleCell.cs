using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCell : MonoBehaviour
{
    public bool isNewline = false;
    public float displayTime = 0; //If left zero, it will be displayed with the default display time
    public float cellWidth;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        if (cellWidth == 0)
        {
            cellWidth = transform.localScale.x;
        }
    }

    public void FadeOut()
    {
        
    }

    public void FadeIn()
    {

    }

    void Update()
    {
        cellWidth = transform.localScale.x;
        
    }
}
