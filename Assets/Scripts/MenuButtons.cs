using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFces : MonoBehaviour
{
    public void SwitchToGameScene()
    {
        SceneManager.LoadScene("MainScene"); 
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
