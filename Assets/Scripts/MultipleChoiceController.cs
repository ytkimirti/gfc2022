using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
public class MultipleChoiceController : MonoBehaviour
{
    public RectTransform rect;
    public static MultipleChoiceController main;
    public GameObject multipleChoicePrefab;
    public RectTransform buttonsHolder;

    [HideInInspector]
    public DialogueData currSelectedOption;

    public Bubble multipleChoiceBubble;

    private void Awake()
    {
        main = this;
        rect = GetComponent<RectTransform>();
    }

    public void ShowOptions(DialogueData parentDialogue)
    {
        if (!parentDialogue.isMultipleChoice)
            Debug.LogError("Some problem with the multiple selection bro...");

        foreach (DialogueData dialogueData in parentDialogue.choices)
        {
            GameObject go = Instantiate(multipleChoicePrefab, transform.position, Quaternion.identity, buttonsHolder);
            
            go.GetComponent<MultipleChoiceButton>().Init(dialogueData);
        }
    }

    public void DestroyOptions()
    {
        foreach (Transform child in buttonsHolder)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClick(DialogueData selectedOption)
    {
        currSelectedOption = selectedOption;
        multipleChoiceBubble.Talk(selectedOption);
        FadeOut();
    }

    [Button()]
    public void FadeIn()
    {
        rect.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
    }

    [Button()]
    public void FadeOut()
    {
        
        rect.DOAnchorPosY(-rect.sizeDelta.y, 0.6f).SetEase(Ease.OutQuart).OnComplete(DestroyOptions);
    }
}
