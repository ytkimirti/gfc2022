using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class EmojiParticle : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.color = Color.clear;
    }

    [Button()]
    public void FadeIn()
    {
        spriteRenderer.DOFade(1, 0.5f);
        spriteRenderer.transform.DOMoveY(0, 0.5f).From(-0.4f);
    }
    [Button()]
    public void FadeOut()
    {
        spriteRenderer.DOFade(0, 0.5f);
        spriteRenderer.transform.DOMoveY(-0.4f, 0.5f);
    }
}
