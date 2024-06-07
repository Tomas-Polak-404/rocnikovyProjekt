using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    private LevelLoader nextlvl;
    public void PlayGame()
    {
        nextlvl = GetComponent<LevelLoader>();
        nextlvl.LoadNextLevel();
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }


}
