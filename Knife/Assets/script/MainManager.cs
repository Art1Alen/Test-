using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private string GameURL = "";
    private string appID = "";
    public void Rate()
    {
        Application.OpenURL(GameURL + appID);
    }
   public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
