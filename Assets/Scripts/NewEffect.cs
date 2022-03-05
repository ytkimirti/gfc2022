using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class NewEffect : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem particle;
    public static NewEffect main;
    public bool isOpen;
    public Image image;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        
    }

    [Button()]
    public void Open()
    {
        Open(image.sprite);
    }
    public void Open(Sprite sprite)
    {
        image.sprite = sprite;
        isOpen = true;
        animator.SetTrigger("Open");
        Invoke("OnLand", 1f);
    }

    [Button()]
    public void Close()
    {
        isOpen = false;
        animator.SetTrigger("Close");
        particle.Stop();
    }

    public void OnLand()
    {
        particle.Play();
    }

    void Update()
    {
        
    }
}
