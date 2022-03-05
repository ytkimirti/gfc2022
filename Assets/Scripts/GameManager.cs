using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool godmode;
    [Header("Special Emojis")]
    public Sprite waitEmoji;
    public Sprite newlineEmoji;
    public Sprite spaceEmoji;
    public Person[] peopleToRun;
    public Person child;
    public Person cookerMain;
    public DialogueData cookerFinalTalk;
    public Person speakerPerson;

    [Space]
    public Transform[] finalPositions;

    public DialogueData finalSurprise;

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
        print($"{Screen.width} {Screen.height}");
    }

    public void MoveAllToTarget(Person person)
    {
        Person[] ps = FindObjectsOfType<Person>();

        foreach (Person p in ps)
        {
            if (person == p)
                continue;
            p.isRunning = true;
            p.isFocused = false;
            p.findNewPosAfterReach = false;
            p.currTargetPos = (Vector2)person.transform.position + Random.insideUnitCircle * 0.6f;
        }
        Invoke("Close", 2f);
    }
    
    public void FinalTalk(Person cooker)
    {
        Invoke("FinalFinal", 3f);
    }

    public void FinalFinal()
    {
        Person[] ps = FindObjectsOfType<Person>();

        for (int i = 0; i < ps.Length; i++)
        {
            Person p = ps[i];
            if (p == cookerMain)
                continue;
            p.isRunning = true;
            p.isFocused = false;
            p.findNewPosAfterReach = false;
            p.currTargetPos = finalPositions[i].position;
        }
        Invoke("FinalMakeTalk", 3f);
    }

    [Button()]
    public void SurpriseEveryone()
    {
        print("Surprise talk");
        Person[] ps = FindObjectsOfType<Person>();

        foreach (Person p in peopleToRun)
        {
            if (p == cookerMain)
                continue;
            p.isRunning = false;
            p.isFocused = false;
            p.findNewPosAfterReach = false;
            p.bubble.Talk(finalSurprise);
        }
    }

    public void FinalMakeTalk()
    {
        cookerMain.bubble.MakeFinalTalk(cookerFinalTalk);
    }

    public void Close()
    {
        Fader.main.FadeIn();
        Invoke("RestartLevel", Fader.main.fadeSpeed);
    }

    public void OnPanic()
    {
        Person[] ps = FindObjectsOfType<Person>();

        foreach (Person p in peopleToRun)
        {
            if (p == speakerPerson)
                continue;
            p.isRunning = true;
            p.isFocused = false;
            p.findNewPosAfterReach = true;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            PlayerPrefs.DeleteAll();
        Time.timeScale = godmode ? (Input.GetKey(KeyCode.F) ? 10f : 1) : 1;
    }
}
