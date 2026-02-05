using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePauseMenu()
    {
        if (pauseMenuUI != null)
        {
            isPaused = pauseMenuUI.activeSelf;
            pauseMenuUI.SetActive(!isPaused);
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }        

    public void QuitGame()
    {
        GameManager.Instance.CloseGame();
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void BackToMain()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePauseMenu();
        }
    }
}
