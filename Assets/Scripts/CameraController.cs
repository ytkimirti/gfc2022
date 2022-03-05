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
    private float focusTimer;
    private Vector2 focusedPosition;

    private Vector2 memMousePos;

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
        focusedPosition = targetPos;

        p.Click();
        focusTimer = 0.2f;
        // KeyboardController.main.FadeIn();
    }

    void LateUpdate()
    {
        if (currPerson && Vector2.Distance(targetPos, focusedPosition) > 1f &&
            !KeyboardController.main.isOpen)
        {
            focusTimer -= Time.deltaTime;
            if (focusTimer <= 0)
            {
                currPerson = null;
            }
        }

        bool isMouse = Input.GetKey(KeyCode.Mouse0);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider2D[] cols = Physics2D.OverlapPointAll(mousePos, playerLayer);
            
            if (cols.Length > 0)
            {
                Person person = cols[0].gameObject.GetComponent<Person>();
                FocusOnPerson(person);
            }
        }

        targetPos += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * 4;
        if (isMouse)
            targetPos += memMousePos - mousePos;

        memMousePos = mousePos;

        transform.position = Vector2.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
