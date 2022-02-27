using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class CameraController : MonoBehaviour
{
    public Person currPerson;

    public Vector2 followOffset;
    
    public Vector2 targetPos;
    public float lerpSpeed;
    public Vector2 mousePos;
    public Camera cam;

    public static CameraController main;

    public LayerMask playerLayer;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        
    }

    public void FocusOnPerson(Person p)
    {
        currPerson = p;
        targetPos = (Vector2)currPerson.transform.position + followOffset;

        p.ShutUp();
        KeyboardController.main.FadeIn();
    }

    void LateUpdate()
    {
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        Collider2D[] cols = Physics2D.OverlapPointAll(mousePos, playerLayer);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (cols.Length > 0)
            {
                Person person = cols[0].gameObject.GetComponent<Person>();
                FocusOnPerson(person);
            }
        }

        targetPos += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * 4;
        
        transform.position = Vector2.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
