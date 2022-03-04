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

    private void Awake()
    {
        main = this;
        rect = GetComponent<RectTransform>();
    }

    // public void ShowOptions(Func onPressed, )
    // {
    //     
    // }

    [Button()]
    public void FadeIn()
    {
        rect.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
    }

    [Button()]
    public void FadeOut()
    {
        
        rect.DOAnchorPosY(-rect.sizeDelta.y, 0.6f).SetEase(Ease.OutQuart);
    }
}
