using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{
    public bool isOpen;
    public PersonDialog currDialog;
    public Bubble currTalkingBubble;
    public RectTransform rect;
    public Transform textAreaGrid;
    public Animator textAreaAnimator;
    public Transform buttonAreaGrid;

    [Header("References")]
    public GameObject buttonPrefab;

    public List<Sprite> addedButtons;

    public static KeyboardController main;

    public void OnButtonPressed(EmojiButton button)
    {
        if (button.transform.parent == buttonAreaGrid)
            button.transform.SetParent(textAreaGrid, true);
        else
            button.transform.SetParent(buttonAreaGrid, true);
        button.currPos = button.transform.position;
    }

    public void AddNewButton(Sprite sprite)
    {
        if (addedButtons.Contains(sprite))
            return;
        addedButtons.Add(sprite);
        NewEffect.main.Open(sprite);
        
        GameObject go = Instantiate(buttonPrefab, buttonAreaGrid.position, Quaternion.identity, buttonAreaGrid);
        EmojiButton button = go.GetComponent<EmojiButton>();

        button.image.sprite = sprite;
    }

    private void Awake()
    {
        main = this;
    }

    [Button()]
    public void FadeIn()
    {
        isOpen = true;
        rect.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
        ClearTextArea();
    }

    [Button()]
    public void FadeOut()
    {
        isOpen = false;
        currDialog = null;
        rect.DOAnchorPosY(-rect.sizeDelta.y * 2, 0.6f).SetEase(Ease.OutQuart);
    }

    public void WrongQuestion()
    {
        ClearTextArea();
        print("No match!!");
        textAreaAnimator.SetTrigger("Shake");
    }

    [Button()]
    public void ClearTextArea()
    {
        int count = textAreaGrid.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = textAreaGrid.GetChild(0);
            child.SetParent(buttonAreaGrid, true);
            child.GetComponent<EmojiButton>().currPos = child.transform.position;
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

            if (sprites.Length != selectedSprites.Length)
                continue;
            
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

        print($"Curr dialog is {currDialog.name}");
        if (currDialog.isSpeakerDialogue)
        {
            NewsController.main.StartNews(selectedData.next);
        }
        else if (currTalkingBubble)
            currTalkingBubble.Talk(selectedData.next);
        
        FadeOut();
    }
}
