using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }


}
