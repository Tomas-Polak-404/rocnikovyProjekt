using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvl2PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManagerScript lvl2Pause;
    void Start()
    {
        lvl2Pause = GetComponent<GameManagerScript>();
    }

    public void Cont()
    {
        lvl2Pause.Continue();
    }
    public void MainMenuGoTo()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

    }
}
