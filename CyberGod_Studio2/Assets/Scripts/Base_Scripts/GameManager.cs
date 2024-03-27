using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonosingletonTemp<GameManager>
{
    public int LevelChange = 0;
    public int body = 0;
    public GameOverScreen GameOverScreen;
    public PauseScreen PauseScreen;
    public bool isPaused = false;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // HandleMusic(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver(int points)
    {
        body = points;
        GameOverScreen.Setup(body);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager OnSceneLoaded");
        HandleMusic(scene.name);
        FindScreensInScene(scene);
    }

    private void HandleMusic(string sceneName)
    {
        if (sceneName == "Congra" || sceneName == "Main" || sceneName == "Select")
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[1]);
        }
        else
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[0]);
        }
    }

    private void FindScreensInScene(Scene scene)
    {
        GameOverScreen = FindObjectOfType<GameOverScreen>();
        PauseScreen = FindObjectOfType<PauseScreen>();

        Debug.Log(GameOverScreen != null ? "GameOverScreen is not null" : "GameOverScreen is null");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseScreen.Setup();
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseScreen.SetDown();
        isPaused = false;
    }
}