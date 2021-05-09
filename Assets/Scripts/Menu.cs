using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    void Start()
    {
        newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        newGameButton.onClick.AddListener(StartGame);

        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(Exit);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    private void Exit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private Button newGameButton;
    private Button exitButton;
}
