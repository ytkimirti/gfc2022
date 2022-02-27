using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using System.Linq;

public class EmojiBubble : MonoBehaviour
{
    public bool isOpen;
    public bool isShowing;
    public DialogueData dialogueToShow;

    public Vector2 currScale;
    public Vector2 defaultScale;
    public float scaleLerpSpeed;

    [Space]

    public int maxEmojisPerRow = 5;

    public int currRowCount;

    public float bubbleScaleMult = 1.610441f;

    [Space]

    public Vector2 emojiScale;
    
    [Space]
    
    public float emojiShowDelay;
    public float emojiShowSpeed;
    public float emojiShowOffset;
    
    [Space]
    
    public float spaceWaitDelay;

    [Space]

    public float startAnimationSpeed;
    public float startAnimationHeight;
    
    public GameObject emojiPrefab;
    public Transform emojiHolder;
    public Transform bubbleHeightHolder;
    private List<SpriteRenderer> currEmojis = new List<SpriteRenderer>();

    [Space]
    public SpriteRenderer bubbleSprite;

    public Transform bubbleHolder;

    private IEnumerator showerEnumerator;
    
    void Start()
    {
        currScale = bubbleSprite.size / bubbleScaleMult;
        defaultScale = currScale;

        FadeIn();
    }
    void Update()
    {
        bubbleSprite.size = Vector2.Lerp(bubbleSprite.size, bubbleScaleMult * currScale, Time.deltaTime * scaleLerpSpeed);

        float targetY = Mathf.Lerp(bubbleHolder.transform.localPosition.y, currRowCount * emojiScale.y,
            Time.deltaTime * scaleLerpSpeed);
        bubbleHolder.transform.localPosition = Vector3.up * targetY; 
    }

    [Button]
    public void FadeIn()
    {
        isOpen = true;
        bubbleSprite.DOFade(1, startAnimationSpeed).From(0);
        bubbleHeightHolder.transform.DOLocalMoveY(0, startAnimationSpeed).From(startAnimationHeight);
    }

    public void DestroyAllEmojis()
    {
        foreach (SpriteRenderer child in currEmojis)
        {
            Destroy(child.gameObject);
        }

        currEmojis = new List<SpriteRenderer>();
    }

    [Button]
    public void CloseBubble()
    {
        isOpen = false;
        StopShowing();
        FadeOut();
    }
    
    void FadeOut()
    {
        bubbleSprite.DOFade(0, startAnimationSpeed).OnComplete(OnFadeOutComplete);
        bubbleHeightHolder.transform.DOLocalMoveY(startAnimationHeight, startAnimationSpeed);

        if (currEmojis.Count > 0)
        {
            DOTween.To(() => bubbleSprite.color.a, (value) =>
            {
                foreach (SpriteRenderer sprite in currEmojis)
                {
                    sprite.color = new Color(1, 1, 1, value);
                }
            }, 0, startAnimationSpeed);
        }
    }
    void OnFadeOutComplete()
    {
        DestroyAllEmojis();
        currScale = defaultScale;
        bubbleSprite.size = currScale * bubbleScaleMult;
        currRowCount = 0;
    }
    
    [Button]
    public void TestText()
    {
        ShowText(dialogueToShow.sprites.Select(x => x.sprite).ToArray());        
    }

    public void ShowText(Sprite[] text)
    {
        if (showerEnumerator != null)
            StopCoroutine(showerEnumerator);
        showerEnumerator = ShowTextEnum(text);
        StartCoroutine(showerEnumerator);
        isShowing = true;
    }

    public void StopShowing()
    {
        isShowing = false;
        if (showerEnumerator != null)
            StopCoroutine(showerEnumerator);
    }

    IEnumerator ShowTextEnum(Sprite[] text)
    {
        int x = 0;
        int y = 0;
        int maxX = 0;

        if (currEmojis.Count > 0)
        {
            FadeOut();
            isOpen = false;
            yield return new WaitForSeconds(startAnimationSpeed * 1.3f);
        }
        if (!isOpen)
        {
            FadeIn();
            yield return new WaitForSeconds(startAnimationSpeed * 1.3f);
        }
        
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == GameManager.main.newlineEmoji)
            {
                y++;
                x = 0;
                continue;
            }
            else if (text[i] == GameManager.main.waitEmoji)
            {
                yield return new WaitForSeconds(spaceWaitDelay);
                continue;
            }


            currRowCount = y;

            maxX = Mathf.Max(maxX, x);

            Vector2 pos = new Vector2(emojiScale.x * x, emojiScale.y * y * -1);;
                
            currScale = defaultScale + new Vector2(emojiScale.x * maxX, emojiScale.y * y);

            Sprite sprite = text[i];

            if (sprite == GameManager.main.spaceEmoji)
                sprite = null;
            
            AddEmoji(pos, sprite);

            x++;
            if (x > 5)
            {
                x = 0;
                y++;
            }

            yield return new WaitForSeconds(emojiShowDelay);
        }
    }


    void AddEmoji(Vector2 pos, Sprite sprite)
    {
        GameObject emojiGo = Instantiate(emojiPrefab, pos, Quaternion.identity, emojiHolder);

        emojiGo.transform.localPosition = pos + Vector2.left * emojiShowOffset;

        SpriteRenderer emojiSpriteRenderer = emojiGo.GetComponent<SpriteRenderer>();

        currEmojis.Add(emojiSpriteRenderer);
        
        emojiSpriteRenderer.sprite = sprite;
        
        emojiSpriteRenderer.DOFade(1, emojiShowSpeed).From(0);
        emojiSpriteRenderer.transform.DOLocalMove(pos, emojiShowSpeed);
    }
}
