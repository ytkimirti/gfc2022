using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Special Emojis")]
    public Sprite waitEmoji;
    public Sprite newlineEmoji;
    public Sprite spaceEmoji;

    [Space]

    public Vector2 gameArea;

    public static GameManager main;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        Gizmos.DrawWireCube(Vector3.zero, gameArea * 2);
    }

    public Vector2 FindRandomPos()
    {
        return new Vector2(
            Random.Range(-gameArea.x, gameArea.x),
            Random.Range(-gameArea.y, gameArea.y)
        );
    }

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
    }
}
