using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class BubbleCell : MonoBehaviour
{
    [HideInInspector]
    public Bubble holder;
    public bool isEmoji = true;
    public bool isNewline = false;
    
    public bool isImage = false;
    
    [Range(0f, 3f)]
    public float displayTime = 0; //If left zero, it will be displayed with the default display time
    [HideIf("isEmoji")]
    public float cellWidth = 0;

    [Header("Animation")]
    public float fadeSpeed;

    public float fadeDistance;

    public float Transparency
    {
        get => spriteRenderer.color.a;
        set
        {
            Color c;
            spriteRenderer.color = new Color((c = spriteRenderer.color).r, c.g, c.b, value);
        }
    }

    public SpriteRenderer spriteRenderer;

    private void OnDrawGizmos()
    {
        if (isNewline)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + Vector3.right * displayTime, 0.4f);
        }
        if (displayTime <= 0)
            return;
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(transform.position, 0.12f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * displayTime);
    }

    private void Awake()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        if (cellWidth == 0 && !isEmoji)
        {
            cellWidth = transform.localScale.x;
        }
    }

    [Button()]
    public void FadeOut()
    {

            spriteRenderer.transform.DOLocalMoveX(fadeDistance, fadeSpeed)
                .From(0);
            spriteRenderer.DOFade(0, fadeSpeed)
                .From(1);
    }

    [Button()]
    public void FadeIn()
    {

            spriteRenderer.transform.DOLocalMoveX(0, fadeSpeed)
                .From(-fadeDistance);
            spriteRenderer.DOFade(1, fadeSpeed)
                .From(0);
    }

    void Update()
    {

    }
}
