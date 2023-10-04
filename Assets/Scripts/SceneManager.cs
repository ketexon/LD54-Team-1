using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ExternAudioSupport()
    {
        AudioManager.Instance.PlayButtonSFX();
    }
    
    public void ExternPauseSupport()
    {
        AudioManager.Instance.Pause();
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
