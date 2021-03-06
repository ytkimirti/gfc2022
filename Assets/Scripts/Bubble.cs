using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using NaughtyAttributes;
using UnityEngine.Serialization;
using UnityEngineInternal;

public class Bubble : MonoBehaviour
{
    public bool isFinished = false;
    public bool isOpen;
    public bool isClosingTransition;
    public bool closeAfterFinishing;
    private bool isFastDialogue; // Go over it fast
    [Space]
    public Vector2 parentScale;

    public bool isEnd;

    public bool isAutoRotation;
    [HideIf("isAutoRotation")]
    public bool isOnRight;

    [Space]
    public SpriteRenderer bubbleSprite;
    public float bubbleScaleMult = 1.610441f;
    public Vector2 bubbleBottomLeftPadding;
    public Vector2 bubbleTopRightPadding;
    public Vector2 defaultBubbleSize;
    
    public List<BubbleCell> currentCells;

    public float scaleLerpSpeed;

    [Header("Cell Holder")]
    public float horizontalSpacing;
    public float verticalSpacing;
    public float cellHeight;
    public float defaultCellWidth;
    public float maxBubbleWidth;
    private float defaultSpriteScale;

    private Vector2 currHolderScale;

    [FormerlySerializedAs("defaultScreenTime")]
    [Header("Animation Values")]
    public float defaultDisplayTime;
    [Space]
    public float bubbleFadeHeight;
    public float bubbleFadeSpeed;

    [Header("Talking")]
    public bool isTalking;
    public BubbleCell[] currDialogue;
    private int currDialogueIndex;
    private float talkTimer;

    [Header("References")]
    public Transform bubbleParentHolderTrans;
    public Transform cellHolderTrans;
    public Transform bubbleFadeHolderTrans;

    private Sprite waitForUnlock;

    private void Start()
    {
        defaultSpriteScale = bubbleSprite.transform.localScale.x;
        DisableBubble();
    }

    public void MakeFinalTalk(DialogueData data)
    {
        isEnd = true;
        Talk(data);
    }

    public void InstantiateCell(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity, cellHolderTrans);

        BubbleCell bc = go.GetComponent<BubbleCell>();

        AddCell(bc);
    }
    public void AddCell(BubbleCell bc)
    {
        if (bc.isEmoji)
            bc.cellWidth = defaultCellWidth;
        
        bc.FadeIn();
        
        currentCells.Add(bc);        
    }

    public void DisableBubble()
    {
        isClosingTransition = false;
        bubbleSprite.gameObject.SetActive(false);
        foreach (BubbleCell cell in currentCells)
        {
            Destroy(cell.gameObject);
        }

        currentCells = new List<BubbleCell>();
    }

    [Button]
    public void FadeIn()
    {
        if (isOpen)
            return;
        isOpen = true;
        currHolderScale = defaultBubbleSize;
        bubbleFadeHolderTrans.DOKill();
        bubbleSprite.DOKill();
        bubbleSprite.gameObject.SetActive(true);
        bubbleSprite.DOFade(1, bubbleFadeSpeed).From(0);
        bubbleFadeHolderTrans.DOLocalMoveY(0, bubbleFadeSpeed).SetEase(Ease.OutQuart)
            .From(bubbleFadeHeight)
            .OnComplete(() => isClosingTransition = false);
    }

    [Button]
    public void FadeOut()
    {
        if (!isOpen)
            return;
        isFastDialogue = false;
        currDialogue = new BubbleCell[0];
        currDialogueIndex = 0;
        isOpen = false;
        bubbleFadeHolderTrans.DOKill();
        bubbleSprite.DOKill();
        isClosingTransition = true;
        bubbleFadeHolderTrans.DOLocalMoveY(bubbleFadeHeight, bubbleFadeSpeed).SetEase(Ease.OutQuart).From(0);
        DOTween.To(() => bubbleSprite.color.a, (val) => SetTransparency(val), 0, bubbleFadeSpeed)
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

    public void Talk(ConvincableDialog dialog)
    {
        StartCoroutine(ConvincableDialogEnum(dialog));
    }

    IEnumerator ConvincableDialogEnum(ConvincableDialog dialog)
    {
        DialogueData node = dialog.firstNode;

        while (node != null)
        {
            Talk(node);

            if (node.isMultipleChoice)
            {
                MultipleChoiceController.main.ShowOptions(node);
                while (MultipleChoiceController.main.currSelectedOption == null)
                    yield return new WaitForEndOfFrame();
                node = MultipleChoiceController.main.currSelectedOption;
                MultipleChoiceController.main.currSelectedOption = null;
            }

            while (MultipleChoiceController.main.multipleChoiceBubble.isOpen)
                yield return new WaitForEndOfFrame();
            
            FadeOut();

            while (isClosingTransition)
                yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);
    }
    public void Talk(DialogueData dialoge)
    {
        // if (dialoge.secondTimeShowFast)
        // {
        //     if (PlayerPrefs.HasKey(dialoge.name))
        //     {
        //         isFastDialogue = true;
        //     }
        //     else
        //     {
        //         PlayerPrefs.SetInt(dialoge.name,1);
        //     }    
        // }
        //
        print($"Talking dialog {dialoge.gameObject.name}");
        if (dialoge.isUnlockable)
        {
            print("It has an unlockable");
            waitForUnlock = dialoge.unlockedEmoji;
        }
        Talk(dialoge.cells.ToArray());
    }
    
    public void Talk(BubbleCell[] dialogue)
    {
        if (isOpen)
        {
            FadeOut();
        }
        currDialogue = dialogue;
        currDialogueIndex = 0;
    }

    void UpdateTalking()
    {
        isFinished = false;
        // If there are still things to talk
        if (currDialogue.Length != 0)
        {
            if (!isOpen && !isClosingTransition)
            {
                FadeIn();
            }
            if (isOpen)
            {
                // If this is fully ready, here is the talking
                talkTimer -= Time.deltaTime;
                if (talkTimer <= 0)
                {
                    if (currDialogueIndex == currDialogue.Length)
                    {
                        if (closeAfterFinishing)
                        {
                            FadeOut();
                        }
                        if (waitForUnlock != null)
                            KeyboardController.main.AddNewButton(waitForUnlock);
                        if (isEnd)
                        {
                            isEnd = false;
                            GameManager.main.SurpriseEveryone();
                        }
                        isFinished = true;
                        return;
                    }
                    // Add another emoji from list
                    BubbleCell cell = currDialogue[currDialogueIndex];
                    InstantiateCell(cell.gameObject);
                    talkTimer = cell.displayTime == 0 ? defaultDisplayTime : cell.displayTime;
                    if (isFastDialogue)
                        talkTimer = defaultDisplayTime * 0.6f;
                    currDialogueIndex++;
                    if (closeAfterFinishing && currDialogueIndex == currDialogue.Length)
                        talkTimer += 2f;
                }
            }
        }
    }

    void Update()
    {
        if (isAutoRotation)
            isOnRight = CameraController.main.transform.position.x > transform.position.x;
        
        UpdateTalking();
        if (isOpen)
        {
            Vector2 newHolderScale = RepositionCells();

            bubbleParentHolderTrans.transform.localPosition =
                new Vector2((isOnRight ? 1 : -1) * parentScale.x, parentScale.y);
            
            currHolderScale = Vector3.Lerp(currHolderScale, (Vector3) newHolderScale + Vector3.forward,
                Time.deltaTime * scaleLerpSpeed);

            Vector2 newBubbleSize = (currHolderScale + bubbleBottomLeftPadding + bubbleTopRightPadding);

            // Scale the bubble sprite
            bubbleSprite.size = (newBubbleSize * bubbleScaleMult);

            bubbleSprite.transform.localScale = new Vector3(isOnRight ? defaultSpriteScale : -defaultSpriteScale,
                defaultSpriteScale, 1);

            // Position the cell holder
            cellHolderTrans.localPosition =
                new Vector2(isOnRight ? bubbleBottomLeftPadding.x : -bubbleBottomLeftPadding.x - currHolderScale.x,
                    currHolderScale.y + bubbleBottomLeftPadding.y);
        }
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
            if (nextCell != null && ((cell.isNewline) || (curr.x + nextCell.cellWidth > maxBubbleWidth)))
            {
                curr.x = topLeftCorner.x;
                curr.y -= cellHeight + verticalSpacing;
            }
        }
        
        return  new Vector2(maxCurrWidth, -(curr.y - cellHeight));
    }
}
