using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    public List<SpriteRenderer> sprites;
    void Start()
    {
        var childs = GetComponentsInChildren<SpriteRenderer>().ToList();
        
        childs.Sort((a, b) =>
        {
            float diff = 0;
            if (Mathf.Abs(a.transform.position.y - b.transform.position.y) > 0.3f)
                diff = b.transform.position.y - a.transform.position.y;
            else
                diff = a.transform.position.x - b.transform.position.x;
            return (int) (diff * 10000);
        });

        sprites = childs;
    }

    void Update()
    {
        
    }
}
