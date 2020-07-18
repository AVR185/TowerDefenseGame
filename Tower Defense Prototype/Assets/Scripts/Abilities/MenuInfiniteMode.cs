using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInfiniteMode : MonoBehaviour
{
    public void LoadInfiniteMode(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
