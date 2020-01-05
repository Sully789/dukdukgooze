/*
 * Sean O'Sullivan, K00180620, Programming Digital Games Engines, CA1
 * GameManager.cs handles the UI and manages the Scenes
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText; //UI that displays Game Over
    public TextMeshProUGUI gameOverMessageText; //UI that displays Game Over message 
    public TextMeshProUGUI creditsText; //UI that displays Credits text
    public RawImage welcomeText; //UI that displays Logo
    public Image menuImage; //UI that displays white background

    public Button restartButton; //Button that handles restarting level
    public Button quitButton; //Button that handles quiting the game
    public Button startButton;//Button that starts the game
    public Button creditsButton; //Button that player presses to go back to the title screen

    public Scene scene; //Scene object used to transition between scenes

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LevelComplete();
    }

    public void Opening() //Method turns on Opening Screen UI
    {
        menuImage.gameObject.SetActive(true);
        welcomeText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }

    public void LevelComplete()//Method checks if any enemies or power ups are left on the scene and loads next level if all gone
    {
        if (GameObject.FindWithTag("Enemy") == null && GameObject.FindWithTag("PowerUp") == null)
        {
            Debug.Log("Load Next Scene");
            if (scene.buildIndex <= 1) //If the buildindex is less then or equal 1 loads next scene
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                
            }
            else if (scene.buildIndex == 2)//Else loads Game Complete method when build index is at 2
            {
                GameComplete();
            }
        }
    }

    public void GameOver() //Method turns Game Over UI elements on
    {
        gameOverText.gameObject.SetActive(true);
        gameOverMessageText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void StartGame() //Method turns Menu UI elements off
    {
        menuImage.gameObject.SetActive(false);
        welcomeText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    public void RestartGame()//Method restarts the current scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()//Method brings player back to title screen
    {
        menuImage.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        gameOverMessageText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void GameComplete()//Method turns Credits UI elements on
    {
        menuImage.gameObject.SetActive(true);
        creditsText.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }
}
