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
    public bool isJumping;
    public PersonState animationState;

    [Header("Shadows")]
    Color defaultShadowColor;
    private Vector2 defaultShadowScale;
    public float maxHeight;

    [Header("Click")]
    public float clickScaleAmount;
    
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
        spriteAnimator.transform.DOPunchScale(Vector3.one * clickScaleAmount, 0.2f);
    }
    void Update()
    {
        float shadowPercent = Mathf.Clamp(spriteHolder.transform.localPosition.y / maxHeight, 0, 1); 
        shadowSprite.color = Color.Lerp(defaultShadowColor, new Color(0, 0, 0, 0), shadowPercent);
        shadowSprite.transform.localScale = defaultShadowScale * (1 - shadowPercent);
        
        // Animation

        if (isJumping)
            animationState = PersonState.Jumping;
        
        spriteAnimator.SetInteger(StateString, (int)animationState);
        
    }
}
