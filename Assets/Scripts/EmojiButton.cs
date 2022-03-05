using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiButton : MonoBehaviour
{
    public Vector2 currPos;
    public float lerpSpeed;
    public RectTransform actualButton;
    public RectTransform myRect;
    public Image image;
    void Start()
    {
        currPos = transform.position;
    }

    public void OnClick()
    {
        KeyboardController.main.OnButtonPressed(this);
    }
    
    void LateUpdate()
    {
        currPos =
            Vector2.Lerp(currPos, myRect.transform.position, Time.deltaTime * lerpSpeed);

        actualButton.transform.position = currPos;


    }
}
