using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    public Vector2 parentScale;
    public bool isOnRight;

    [Space]
    public SpriteRenderer bubbleSprite;
    public float bubbleScaleMult = 1.610441f;
    public Vector2 bubbleBottomLeftPadding;
    public Vector2 bubbleTopRightPadding;

    
    public BubbleCell[] currentCells;

    public float scaleLerpSpeed;

    [Header("Cell Holder")]
    public float horizontalSpacing;
    public float verticalSpacing;
    public float cellHeight;
    public float maxBubbleWidth;

    private Vector2 currHolderScale;

    [Header("References")]
    public Transform cellHolderTrans;

    public void AddCell(GameObject prefab)
    {
                
    }

    void Update()
    {
        currentCells = GetComponentsInChildren<BubbleCell>();

        Vector2 newHolderScale = RepositionCells();

        currHolderScale = Vector3.Lerp(currHolderScale, (Vector3)newHolderScale + Vector3.forward, Time.deltaTime * scaleLerpSpeed);

        Vector2 newBubbleSize = (currHolderScale + bubbleBottomLeftPadding + bubbleTopRightPadding);
        
        // Scale the bubble sprite
        bubbleSprite.size = newBubbleSize * bubbleScaleMult;
        
        // Position the cell holder
        cellHolderTrans.localPosition = new Vector2(bubbleBottomLeftPadding.x, currHolderScale.y + bubbleBottomLeftPadding.y);
    }

    Vector2 RepositionCells()
    {
        Vector2 topLeftCorner = new Vector2(0, 0);

        Vector2 curr = topLeftCorner;
        
        float maxCurrWidth = curr.x;

        for (int i = 0; i < currentCells.Length; i++)
        {
            BubbleCell cell = currentCells[i];
            BubbleCell nextCell = null;
            if (i != currentCells.Length - 1)
                nextCell = currentCells[i + 1];

            cell.transform.localPosition = new Vector2(curr.x + cell.cellWidth / 2, curr.y - cellHeight / 2);

            maxCurrWidth = Mathf.Max(curr.x + cell.cellWidth, maxCurrWidth);

            curr.x += cell.cellWidth + horizontalSpacing;
            

            // Make a newline
            if (nextCell != null && curr.x + nextCell.cellWidth > maxBubbleWidth)
            {
                curr.x = topLeftCorner.x;
                curr.y -= cellHeight + verticalSpacing;
            }
        }
        
        return  new Vector2(maxCurrWidth, -(curr.y - cellHeight));
    }
}
