using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using NaughtyAttributes;
using UnityEditor;

public class Bubble : MonoBehaviour
{
    public bool isOpen = false;
    [Space]
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

    [Header("Animation Values")]
    public float defaultScreenTime;
    [Space]
    public float cellFadeDistance;
    public float cellFadeSpeed;
    public float bubbleFadeHeight;
    public float bubbleFadeSpeed;
    
    

    [Header("References")]
    public Transform cellHolderTrans;
    public Transform bubbleFadeHolderTrans;

    public void AddCell(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity, cellHolderTrans);

        BubbleCell bc = go.GetComponent<BubbleCell>();
        
        currentCells.Add(bc);
    }

    public void DisableBubble()
    {
        bubbleSprite.gameObject.SetActive(false);
        foreach (BubbleCell cell in currentCells)
        {
            Destroy(cell.gameObject);
        }
    }

    [Button]
    public void FadeIn()
    {
        if (isOpen)
            return;
        isOpen = true;
        bubbleSprite.gameObject.SetActive(true);
        bubbleSprite.DOFade(1, bubbleFadeSpeed).From(0);
        bubbleFadeHolderTrans.DOLocalMoveY(0, bubbleFadeSpeed).SetEase(Ease.OutQuart)
            .From(bubbleFadeHeight);
    }

    [Button]
    public void FadeOut()
    {
        if (!isOpen)
            return;
        isOpen = false;
        bubbleFadeHolderTrans.DOLocalMoveY(0, bubbleFadeSpeed).SetEase(Ease.OutQuart);
        DOTween.To(() => bubbleSprite.color.a, SetTransparency, 0, bubbleFadeSpeed)
            .SetEase(Ease.OutQuart)
            .OnComplete(DisableBubble);
    }

    void SetTransparency(float value)
    {
        Color color;
        bubbleSprite.color = new Color((color = bubbleSprite.color).r, color.g, color.b, value);
        foreach (BubbleCell c in currentCells)
        {
            c.Transparency = value;
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
