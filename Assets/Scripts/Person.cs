using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using Random = UnityEngine.Random;

[System.Serializable]
public enum PersonState
{
    Idle,
    Jumping,
    Running,
    Walking
}
public class Person : MonoBehaviour
{
    public bool isTalkable;

    [ShowIf("isTalkable")]
    public PersonDialog personDialog;
    public bool isConvincable = false;

    [ShowIf("isConvincable")]
    public ConvincableDialog convincableDialog;
    [Space]
    public bool isStaticDialogue;
    [ShowIf("isStaticDialogue")]
    public DialogueData staticDialogue;

    [Header("State")]
    public bool isFocused;
    public bool isJumping;
    public bool isWalking;
    public bool isRunning;
    public PersonState animationState;

    [Header("Running")]
    public float runningSpeed;
    public float walkingSpeed;
    public float runningSpeedRandomness;

    [Space]
    public float waitTime;
    public float waitTimeRandomness;
    private float movementTimer;
    public Vector2 currTargetPos;

    [Header("Shadows")]
    private Color defaultShadowColor;
    private Vector2 defaultShadowScale;
    public float maxHeight;

    [Header("Click")]
    public float clickScaleAmount;
    
    [Header("References")]
    public Transform spriteHolder;
    public Bubble bubble;

    public Animator spriteAnimator;
    public SpriteRenderer sprite;
    public SpriteRenderer shadowSprite;
    private static readonly int StateString = Animator.StringToHash("state");

    void Start()
    {
        defaultShadowColor = shadowSprite.color;
        defaultShadowScale = shadowSprite.transform.localScale;
        runningSpeed += Random.Range(-runningSpeedRandomness, runningSpeedRandomness);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (!isRunning && !isWalking)
            return;
        
        Gizmos.DrawWireSphere(currTargetPos, 0.3f);
        Gizmos.DrawLine(transform.position, currTargetPos);
    }

    public void ShutUp()
    {
        isJumping = false;
    }

    public void Click()
    {
        ClickEffect();
        if (isTalkable)
        {
            KeyboardController.main.currDialog = personDialog;
            KeyboardController.main.currTalkingBubble = bubble;
            KeyboardController.main.FadeIn();
            if (personDialog.helloDialogue)
                bubble.Talk(personDialog.helloDialogue);
        }
        else if (isStaticDialogue)
        {
            bubble.Talk(staticDialogue);
        }
        else if (isConvincable)
        {
            bubble.Talk(convincableDialog);
        }
    }

    public void ClickEffect()
    {
        spriteAnimator.transform.DOPunchScale(Vector3.one * clickScaleAmount, 0.2f);
    }
    void Update()
    {
        isFocused = CameraController.main.currPerson == this;

        //TODO: Add if it's not focused and bubble is open, close that bubble 
        
        // Animation
        if (isRunning)
            animationState = PersonState.Running;
        else if (isWalking)
            animationState = PersonState.Walking;
        else if (isJumping)
            animationState = PersonState.Jumping;
        else
            animationState = PersonState.Idle;
        
        // Running
        if (!isFocused && (isRunning || isWalking))
        {
            if (Vector2.Distance(transform.position, currTargetPos) < 0.5f)
            {
                movementTimer -= Time.deltaTime;
                // If it's running, ignore the wait timer
                if (movementTimer <= 0 || isRunning)
                {
                    // Waiting animation
                    animationState = PersonState.Idle;
                    movementTimer = waitTime + Random.Range(-waitTimeRandomness, waitTimeRandomness);
                    // Find a new target position
                    currTargetPos = GameManager.main.FindRandomPos();
                }
            }
            else
            {
                float speed = isRunning ? runningSpeed : walkingSpeed;
                // Keep on running...
                transform.position = Vector2.MoveTowards(transform.position, currTargetPos,
                    Time.deltaTime * speed);
            }
        }
        
        float shadowPercent = Mathf.Clamp(spriteHolder.transform.localPosition.y / maxHeight, 0, 1); 
        shadowSprite.color = Color.Lerp(defaultShadowColor, new Color(0, 0, 0, 0), shadowPercent);
        shadowSprite.transform.localScale = defaultShadowScale * (1 - shadowPercent);

        spriteAnimator.SetInteger(StateString, (int)animationState);
    }
}
