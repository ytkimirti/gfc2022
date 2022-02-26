using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EmojiBubble : MonoBehaviour
{
    public string textToShow;

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

    [Space]
    public SpriteRenderer bubbleSprite;

    public Transform bubbleHolder;
    
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
        bubbleSprite.DOFade(1, startAnimationSpeed).From(0);
        bubbleSprite.transform.DOLocalMoveY(0, startAnimationSpeed).From(startAnimationHeight);
    }
    
    [Button]
    public void FadeOut()
    {
        bubbleSprite.DOFade(0, startAnimationSpeed);
        bubbleSprite.transform.DOLocalMoveY(startAnimationHeight, startAnimationSpeed);
    }
    
    [Button]
    public void TestText()
    {
        StartCoroutine(ShowText(textToShow));
    }

    IEnumerator ShowText(string text)
    {
        int x = 0;
        int y = 0;
        int maxX = 0;
        
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == 'n')
            {
                y++;
                x = 0;
                continue;
            }

            if (text[i] == ' ')
            {
                yield return new WaitForSeconds(spaceWaitDelay);
                continue;
            }
            if (x > 5)
            {
                x = 0;
                y++;
            }

            currRowCount = y;

            maxX = Mathf.Max(maxX, x);

            Vector2 pos = new Vector2(emojiScale.x * x, emojiScale.y * y * -1);;
                
            currScale = defaultScale + new Vector2(emojiScale.x * maxX, emojiScale.y * y);

            AddEmoji(pos, text[i].ToString());

            x++;

            yield return new WaitForSeconds(emojiShowDelay);
        }
    }

    [Button()]
    public void AddTestEmoji()
    {
        AddEmoji(Vector2.zero, "a");
    }

    void AddEmoji(Vector2 pos, string emojiName)
    {
        Sprite sprite = GameManager.main.FindEmoji(emojiName);
        
        GameObject emojiGo = Instantiate(emojiPrefab, pos, Quaternion.identity, emojiHolder);

        emojiGo.transform.localPosition = pos + Vector2.left * emojiShowOffset;

        SpriteRenderer emojiSpriteRenderer = emojiGo.GetComponent<SpriteRenderer>();

        emojiSpriteRenderer.sprite = sprite;
        
        emojiSpriteRenderer.DOFade(1, emojiShowSpeed).From(0);
        emojiSpriteRenderer.transform.DOLocalMove(pos, emojiShowSpeed);
    }
}
