using UnityEngine;
using System.Collections;
using NaughtyAttributes;

public class SaveSystemSetup : MonoBehaviour
{

    public string fileName = "Profile.bin"; // file to save with the specified resolution
    public bool dontDestroyOnLoad = true; // the object will move from one scene to another (you only need to add it once)

    static SaveSystemSetup main;

    void Awake()
    {
        if (main)
        {
            if (main != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            main = this;
        }

        SaveSystem.Initialize(fileName);
        if (dontDestroyOnLoad) DontDestroyOnLoad(transform.gameObject);
    }

    [Button]
    public void Save()
    {
        SaveSystem.SaveToDisk();
    }

    // if the object is present in all game scenes, auto save before exiting
    // on some platforms there may not be an exit function, see the Unity help
    void OnApplicationQuit()
    {
        SaveSystem.SaveToDisk();
    }
}
