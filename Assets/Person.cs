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
    public bool isMom;
    public bool isSecondTime;
    
    [Header("Talk")]
    public DialogueData dialogue;
    public DialogueData dialogue2;

    public bool dialogueListed;
    public DialogueData[] dialogueList;
    public KeyCode keyToSpeak;
    private int dialogueListIndex = 0;

    public EmojiBubble bubble;
    
    
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

    public void ShutUp()
    {
        isJumping = false;
    }

    public void Talk()
    {
        if (dialogueListed)
        {
            bubble.Talk(dialogueList[dialogueListIndex]);
            dialogueListIndex++;
        }
        else
        {
            if (dialogue2)
                bubble.Talk(isSecondTime ? dialogue2 : dialogue);
            else
                bubble.Talk(dialogue);    
        }
        
        isSecondTime = true;
        isJumping = false;
    }
    
    public void ClickEffect()
    {
        spriteAnimator.transform.DOPunchScale(Vector3.one * clickScaleAmount, 0.2f);
    }
    void Update()
    {
        if (Input.GetKeyDown(keyToSpeak))
        {
            Talk();
        }
        
        float shadowPercent = Mathf.Clamp(spriteHolder.transform.localPosition.y / maxHeight, 0, 1); 
        shadowSprite.color = Color.Lerp(defaultShadowColor, new Color(0, 0, 0, 0), shadowPercent);
        shadowSprite.transform.localScale = defaultShadowScale * (1 - shadowPercent);
        
        // Animation

        if (isJumping)
            animationState = PersonState.Jumping;
        else
            animationState = PersonState.Idle;
        
        spriteAnimator.SetInteger(StateString, (int)animationState);
    }
}
