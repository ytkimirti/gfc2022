using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Special Emojis")]
    public Sprite waitEmoji;
    public Sprite newlineEmoji;
    public Sprite spaceEmoji;

    public static GameManager main;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
    }
}
