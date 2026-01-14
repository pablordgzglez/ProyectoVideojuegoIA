using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string gameSceneName = "GameSceneFinal";
    [SerializeField] private string menuSceneName = "MainMenu";

    private bool paused;
    private ShopUI shopUI;

    void Start()
    {
        paused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Buscar la tienda (si existe)
        shopUI = FindFirstObjectByType<ShopUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (shopUI != null && shopUI.IsOpen())
            {
                shopUI.Close();
                return;
            }

            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;

        Time.timeScale = paused ? 0f : 1f;

        if (pausePanel != null)
            pausePanel.SetActive(paused);

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    public void Resume()
    {
        if (paused)
            TogglePause();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
