using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class GameManagerScript : MonoBehaviour
{

    public GameObject pauseScreen; // Objekt panelu PauseScreen

    private bool isPaused = false; // Prom�nn� pro ulo�en� stavu hry (pozastaven� / spu�t�n�)
    public GameObject settingsPanel; // Reference na panel s nastaven�m


    public GameObject gameOverUI;
    public static bool muteMusic; // Statick� prom�nn� pro uchov�n� stavu zvuku


    public void nextLVL()
    {
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);

    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        // Deaktivace panelu PauseScreen
        //pauseScreen.SetActive(false);

        // Nastaven� prom�nn� isPaused na false
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    bool allowCursor = false;

    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = allowCursor; // Povolen� kurzoru po obnoven� hry
        // Kontrola stisku kl�vesy P
        if (Input.GetButtonUp("Pause"))
        {
            // Pokud je hra pozastavena, obnovte ji
            if (isPaused)
            {
                Continue();
                Time.timeScale = 1;
                allowCursor = false;
                pauseScreen.SetActive(false);
                //Cursor.lockState = CursorLockMode.Locked;
                isPaused = false;
            }
            // Jinak hru pozastavte
            else
            {
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
                allowCursor = true; // Povolen� kurzoru p�i pozastaven� hry
                //Cursor.lockState = CursorLockMode.None;
                isPaused = true;
            }
        }

        // Kontrola stavu Game Over UI a ovl�d�n� kurzoru
        if (gameOverUI.activeInHierarchy || settingsPanel.activeInHierarchy || pauseScreen.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Zde se kurzor neovl�d�, pokud nen� aktivn� Game Over UI
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    public void Settings()
    {
        // Deaktivuj hlavn� panel pokud existuje
        if (pauseScreen != null)
        {
            BackPauseScreen();
            pauseScreen.SetActive(false);
        }
        else
        {
            Debug.LogWarning("pauseScreen not found!");
        }

        // Aktivuj panel s nastaven�m pokud existuje
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            allowCursor = false;
        }
        else
        {
            Debug.LogWarning("SettingsPanel not found!");
        }
    }


    //Settings

    public void BackPauseScreen()
    {
        // Aktivuj hlavn� panel pokud existuje
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("pauseScreen not found!");
        }

        // Deaktivuj panel s nastaven�m pokud existuje
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Cursor.visible = false; // Skryt� kurzoru po zav�en� nastaven�
            Cursor.lockState = CursorLockMode.Locked; // Zamknut� kurzoru
        }
        else
        {
            Debug.LogWarning("SettingsPanel not found!");
        }
    }

    public void VolumeOn()
    {
        muteMusic = false;
        Debug.Log(muteMusic);
    }

    public void VolumeOff()
    {
        muteMusic = true; // Nastaven� prom�nn� na true (zvuk vypnut)
        Debug.Log(muteMusic);
    }



    public void Continue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Nastaven� �asov�ho m���tka na norm�ln�
        Time.timeScale = 1;

        // Deaktivace panelu PauseScreen
        pauseScreen.SetActive(false);

        // Nastaven� prom�nn� isPaused na false
        isPaused = false;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                                Application.Quit();
        #endif
    }

}
