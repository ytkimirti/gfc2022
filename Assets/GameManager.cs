using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Sprite[] emojis;

    public static GameManager main;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        
    }

    public Sprite FindEmoji(string str)
    {
        foreach (var s in emojis)
        {
            if (s.name == str)
            {
                return s;
            }
        }

        return null;
        
    }
}
