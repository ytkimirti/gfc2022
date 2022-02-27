using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class KeyboardController : MonoBehaviour
{
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [Button()]
    public void FadeIn()
    {
        rect.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
    }

    [Button()]
    public void FadeOut()
    {
        
        rect.DOAnchorPosY(-rect.sizeDelta.y * 2, 0.6f).SetEase(Ease.OutQuart);
    }

    public void Send()
    {
        FadeOut();
        if (CameraController.main.currPerson)
            CameraController.main.currPerson.Talk();
    }
}
