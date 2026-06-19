using UnityEngine;
using UnityEngine.SceneManagement;

public class GameToMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.SetString(
                "LastScene",
                SceneManager.GetActiveScene().name
            );

            PlayerPrefs.Save();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene(0);
        }
    }
}