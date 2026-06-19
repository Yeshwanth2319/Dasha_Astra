using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("LastScene", currentScene);
        PlayerPrefs.Save();

        SceneManager.LoadScene(0);
    }
}