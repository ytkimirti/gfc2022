using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class NewsController : MonoBehaviour
{
    public DialogueData testData;
    public Animator animator;
    public GameObject emojiPrefab;
    public Transform gridTrans;
    public AudioSource newsAudio;
    private DialogueData currDialogue;

    private float firstCameraScale;

    public static NewsController main;

    private void Awake()
    {
        main = this;
    }

    [Button()]
    public void Test()
    {
        StartNews(testData);
    }

    public void StartNews(DialogueData data)
    {
        currDialogue = data;
        print("Starting news");
        ClearAll();
        animator.SetTrigger("Start");
        newsAudio.volume = 1;
        newsAudio.Play();

        foreach (Sprite s in data.cells.Select(x => x.spriteRenderer.sprite))
        {
            GameObject go = Instantiate(emojiPrefab, transform.position, Quaternion.identity, gridTrans);
            go.GetComponent<Image>().sprite = s;
        }

        firstCameraScale = CameraController.main.cam.orthographicSize;
        CameraController.main.cam.DOOrthoSize(3, 2f);
        Invoke("Default", 10f);
    }
    public void Default()
    {
        if (currDialogue.eventAfterDone != null)
            currDialogue.eventAfterDone.Invoke();
        newsAudio.DOFade(0, 2f).OnComplete(() => newsAudio.Stop());
        CameraController.main.cam.DOOrthoSize(firstCameraScale, 2f);
    }

    [Button()]
    public void ClearAll()
    {
        foreach (Transform t in gridTrans)
        {
            Destroy(t.gameObject);
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
