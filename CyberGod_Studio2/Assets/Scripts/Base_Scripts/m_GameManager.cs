using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class m_GameManager : MonosingletonTemp<m_GameManager>
{
    [SerializeField]public int LevelChange = 0;
    [SerializeField]public GameOverScreen GameOverScreen;
    [SerializeField]public PauseScreen PauseScreen;
    [SerializeField]public bool isPaused = false;

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

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;

        }
    }

    public void GameOver(string text)
    {
        GameOverScreen.Setup(text);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager OnSceneLoaded");
        ControlMode_Manager.Instance.m_controlMode = ControlMode.DIALOGUE;
        isPaused = false;
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