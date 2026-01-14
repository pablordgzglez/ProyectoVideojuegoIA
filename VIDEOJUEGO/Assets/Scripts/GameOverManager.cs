using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string gameScene = "GameSceneFinal";
    [SerializeField] string menuScene = "MainMenu";

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameScene);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
