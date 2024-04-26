using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    // private static GameManager _instance;
    //
    // public static GameManager instance
    // {
    //     get
    //     {
    //         if (_instance == null)
    //         {
    //             _instance = FindObjectOfType<GameManager>();
    //             if (_instance == null)
    //             {
    //                 GameObject gameManagerGameObject = new GameObject();
    //                 _instance = gameManagerGameObject.AddComponent<GameManager>();
    //                 gameManagerGameObject.name = "Game Manager";
    //             }
    //         }
    //
    //         return _instance;
    //     }
    // }

    private GameObject m_GameOverCanvas;

    private void Awake()
    {
        // // Singleton
        // if (_instance == null)
        // {
        //     _instance = this;
        //     DontDestroyOnLoad(this.gameObject);
        // }
        // else
        // {
        //     Destroy(this.gameObject);
        // }

        // Set the gameOverCanvas to false at the start of the game
        m_GameOverCanvas = GameObject.Find("Game Over Canvas");
        m_GameOverCanvas.SetActive(false);
        
        // Set the Cursor to false at the start of the game
        // Cursor.visible = false;  // This is not needed because we are locking the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Set Time.timeScale to 1 at the start of the game
        Time.timeScale = 1;
    }

    public void InvokeGameOverCanvas()
    {
        Time.timeScale = 0;
        m_GameOverCanvas.SetActive(true);
        // Cursor.visible = true;  // This is not needed because we are unlocking the cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}