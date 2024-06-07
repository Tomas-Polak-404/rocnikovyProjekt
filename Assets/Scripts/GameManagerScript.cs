using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class GameManagerScript : MonoBehaviour
{

    public GameObject pauseScreen; // Objekt panelu PauseScreen

    private bool isPaused = false; // Promìnná pro uložení stavu hry (pozastavená / spuštìná)
    public GameObject settingsPanel; // Reference na panel s nastavením


    public GameObject gameOverUI;
    public static bool muteMusic; // Statická promìnná pro uchování stavu zvuku


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

        // Nastavení promìnné isPaused na false
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    bool allowCursor = false;

    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = allowCursor; // Povolení kurzoru po obnovení hry
        // Kontrola stisku klávesy P
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
                allowCursor = true; // Povolení kurzoru pøi pozastavení hry
                //Cursor.lockState = CursorLockMode.None;
                isPaused = true;
            }
        }

        // Kontrola stavu Game Over UI a ovládání kurzoru
        if (gameOverUI.activeInHierarchy || settingsPanel.activeInHierarchy || pauseScreen.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Zde se kurzor neovládá, pokud není aktivní Game Over UI
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
        // Deaktivuj hlavní panel pokud existuje
        if (pauseScreen != null)
        {
            BackPauseScreen();
            pauseScreen.SetActive(false);
        }
        else
        {
            Debug.LogWarning("pauseScreen not found!");
        }

        // Aktivuj panel s nastavením pokud existuje
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
        // Aktivuj hlavní panel pokud existuje
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("pauseScreen not found!");
        }

        // Deaktivuj panel s nastavením pokud existuje
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Cursor.visible = false; // Skrytí kurzoru po zavøení nastavení
            Cursor.lockState = CursorLockMode.Locked; // Zamknutí kurzoru
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
        muteMusic = true; // Nastavení promìnné na true (zvuk vypnut)
        Debug.Log(muteMusic);
    }



    public void Continue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Nastavení èasového mìøítka na normální
        Time.timeScale = 1;

        // Deaktivace panelu PauseScreen
        pauseScreen.SetActive(false);

        // Nastavení promìnné isPaused na false
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
