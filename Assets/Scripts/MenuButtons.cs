using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFces : MonoBehaviour
{
    public void SwitchToGameScene()
    {
        SceneManager.LoadScene("MainScene"); 
    }
    public GameObject mainMenuPanel;  // Reference na hlavní panel
    public GameObject settingsPanel;  // Reference na panel s nastavením

    public static bool muteMusic = false; // Statická promìnná pro uchování stavu zvuku


    public void Setting()
    {
        // Deaktivuj hlavní panel pokud existuje
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("MainMenuPanel not found!");
        }

        // Aktivuj panel s nastavením pokud existuje
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("SettingsPanel not found!");
        }
    }


    public void BackToMainMenu()
    {
        // Aktivuj hlavní panel pokud existuje
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("MainMenuPanel not found!");
        }

        // Deaktivuj panel s nastavením pokud existuje
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SettingsPanel not found!");
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }


    // Settings

    public void VolumeOn()
    {
        muteMusic = false;
        Debug.Log(muteMusic);
    }

    public void VolumeOff()
    {
        muteMusic = true; // Nastavení promìnné na true (zvuk vypnut)
        Debug.Log(muteMusic);
    }

    public void BackButton()
    {
        BackToMainMenu();
    }











}
