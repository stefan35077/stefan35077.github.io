using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void OnPlayGameButton()
    {
        Debug.Log("Play game!");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game!");
    }
}
