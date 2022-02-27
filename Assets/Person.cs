using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

[System.Serializable]
public enum PersonState
{
    Idle,
    Jumping
}
public class Person : MonoBehaviour
{
    [Header("State")]
    public PersonState animationState;

    [Header("Shadows")]
    Color defaultShadowColor;
    private Vector2 defaultShadowScale;
    public float maxHeight;
    
    [Header("References")]
    public Transform spriteHolder;

    public Animator spriteAnimator;
    public SpriteRenderer sprite;
    public SpriteRenderer shadowSprite;
    private static readonly int StateString = Animator.StringToHash("state");

    void Start()
    {
        defaultShadowColor = shadowSprite.color;
        defaultShadowScale = shadowSprite.transform.localScale;
    }

    public void ClickEffect()
    {
        
    }
    void Update()
    {
        float shadowPercent = Mathf.Clamp(spriteHolder.transform.localPosition.y / maxHeight, 0, 1); 
        shadowSprite.color = Color.Lerp(defaultShadowColor, new Color(0, 0, 0, 0), shadowPercent);
        shadowSprite.transform.localScale = defaultShadowScale * (1 - shadowPercent);
        spriteAnimator.SetInteger(StateString, (int)animationState);
    }
}
