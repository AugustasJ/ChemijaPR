using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenElementsAR()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenChallengeAR()
    {
        SceneManager.LoadScene(3);
    }

}
