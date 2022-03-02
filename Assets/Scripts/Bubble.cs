using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Bubble : MonoBehaviour
{

    public Vector2 parentScale;
    public bool isOnRight;

    [Space]
    public SpriteRenderer bubbleSprite;
    public float bubbleScaleMult = 1.610441f;
    public Vector2 bubbleBottomLeftPadding;
    public Vector2 bubbleTopRightPadding;

    
    public List<BubbleCell> currentCells;

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
        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity, cellHolderTrans);

        BubbleCell bc = go.GetComponent<BubbleCell>();
        
        currentCells.Add(bc);
    }

    public void DestroyCells()
    {
        foreach (BubbleCell cell in currentCells)
        {
            Destroy(cell.gameObject);
        }
    }

    void Update()
    {
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

        for (int i = 0; i < currentCells.Count; i++)
        {
            BubbleCell cell = currentCells[i];
            BubbleCell nextCell = null;
            if (i != currentCells.Count - 1)
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
