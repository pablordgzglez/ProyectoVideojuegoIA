using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameSceneFinal";
    [SerializeField] private string menuSceneName = "MainMenu";

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
