using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class KeyboardController : MonoBehaviour
{
    public PersonDialog currDialog;
    public Bubble currTalkingBubble;
    public RectTransform rect;
    public Transform textAreaGrid;
    public Transform buttonAreaGrid;

    public static KeyboardController main;

    public void OnButtonPressed(EmojiButton button)
    {
        if (button.transform.parent == buttonAreaGrid)
            button.transform.SetParent(textAreaGrid, false);
        else
            button.transform.SetParent(buttonAreaGrid, false);
    }

    private void Awake()
    {
        main = this;
    }

    [Button()]
    public void FadeIn()
    {
        rect.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
        ClearTextArea();
    }

    [Button()]
    public void FadeOut()
    {
        currDialog = null;
        rect.DOAnchorPosY(-rect.sizeDelta.y * 2, 0.6f).SetEase(Ease.OutQuart);
    }

    public void WrongQuestion()
    {
        ClearTextArea();
    }

    public void ClearTextArea()
    {
        foreach (Transform c in textAreaGrid)
        {
            c.SetParent(buttonAreaGrid, true);
        }
        
    }

    public void Send()
    {
        if (textAreaGrid.childCount <= 0)
        {
            WrongQuestion();
            return;
        }

        Sprite[] selectedSprites =
            textAreaGrid.gameObject.GetComponentsInChildren<EmojiButton>().Select(x => x.image.sprite).ToArray();
        print($"Selected sprite count {selectedSprites.Length}");
        bool doesMatch = false;
        DialogueData selectedData = null;
        foreach (DialogueData data in currDialog.options)
        {
            Sprite[] sprites = data.cells.Select(x => x.spriteRenderer.sprite).ToArray();
            
            if (sprites.Length == 0)
                Debug.LogError("Length can't be zero");
            
            print($"Currently examined things {sprites.Length}");

            doesMatch = true;
            foreach (Sprite s in sprites)
            {
                if (!(selectedSprites.Contains(s)))
                {
                    doesMatch = false;
                    break;
                }
            }

            if (doesMatch)
            {
                selectedData = data;
                break;
            }
        }

        if (!doesMatch)
        {
            WrongQuestion();
            return;
        }

        print("Match!!!");
        
        // It does match!!!
        FadeOut();
        
        if (currTalkingBubble)
            currTalkingBubble.Talk(selectedData.next);
    }
}
