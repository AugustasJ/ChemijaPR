using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARChallengeMenu : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
