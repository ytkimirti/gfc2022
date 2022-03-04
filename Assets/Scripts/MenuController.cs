using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public float fadeDelayAfterStart;
    public Animator anim;

    public void StartGame()
    {
        anim.enabled = true;
        Invoke("StartFade", fadeDelayAfterStart);
    }

    void StartFade()
    {
        Fader.main.FadeIn();
        Invoke("LoadScene", Fader.main.fadeSpeed);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
