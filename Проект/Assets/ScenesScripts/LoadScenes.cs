using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes: MonoBehaviour
{
    string sceneforload;
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneforload);
    }
}
