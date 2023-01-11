using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button startGame;
    [SerializeField] Button exitGame;
    [SerializeField] Text instructionsText;

    private void Start()
    {
        instructionsText.text = "How To Play:\n" +
            "Destroy the alien Snatchers before they reach the MOTHERSHIP! Destroy and evade Interceptors!\nSave every human!\n" +
            "Controls:\n" +
            "WASD- Gliding Movement\n" +
            "Shift - Barrel Roll\n" +
            "Space - Boost\n" +
            "LeftMouse - Fire Bullets\n" +
            "RightMouse - Fire Nets\n" +
            "MiddleMouse- Bullet Time\n" +
            "Escape - Pause";
            
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Play");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
