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
        if (Input.GetMouseButtonDown(2))
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
        ResumeGame();
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
        // 检查 PauseScreen 对象是否为 null
        if (PauseScreen != null)
        {
            PauseScreen.SetDown();
        }
        isPaused = false;
    }
    
    //切换场景
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}