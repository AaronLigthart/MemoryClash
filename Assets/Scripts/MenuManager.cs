using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour    
{
    public void OnTrigerBehaviour(int i)
    {
        switch (i)
        {
            default: break;
            case 0:
                SceneManager.LoadScene("Game");
                break;
            case 1:
                Application.Quit();
                break;
        }
    }



}
