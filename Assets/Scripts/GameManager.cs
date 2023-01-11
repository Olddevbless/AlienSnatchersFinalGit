using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [Header("UI")]
    [SerializeField] Text scoreText;
    [SerializeField] Text humanlivesText;
    [SerializeField] Text alienShipsText;
    [SerializeField] Text timerText;
    [SerializeField] GameObject finalScoreImage;
    [SerializeField] GameObject pauseMenuImage;
    [SerializeField] Text playerHealthText;
    [SerializeField] Text playerEnergyText;
    [SerializeField] Slider playerEnergySlider;
    GameObject player;
    public Button endGameRestartButton;
    public Button endGameMainMenuButton;
    public Button pauseMenuRestartButton;
    public Button pauseMenuMainMenuButton;
    public Button pauseMenuResumeButton;
    [Header("Game Management")]
    public int score = 0;
    public int playerHealth;
    public int maxPlayerHealth = 10;
    public bool gameIsPaused = false;
    public bool isDead = false;
    private float timer = 0f;
    public int humanlives = 3;
    //private void Awake()
    //{
    //    if (Instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    private void Start()
    {
         player = FindObjectOfType<PlayerController>().gameObject;
        GameManager[] gms= FindObjectsOfType<GameManager>();
        Debug.Log(gms.Length);
        humanlives = 3;
        score = 0;
        playerHealth = maxPlayerHealth;
    }
    // Update is called once per frame
    void Update()
    {
        // Increment the timer
        if (!isDead)
        {
            timer += Time.deltaTime;
        }
       
        timerText.text = "Time: "+(int)timer;
        alienShipsText.text = "Alien Snatchers: "+FindObjectsOfType<EnemyController>().Length;
        scoreText.text = "Score: " +score;
        humanlivesText.text = "Human Lives: " + humanlives;
        playerHealthText.text = "Health:" + playerHealth;
        playerEnergyText.text = "Bullet Time Energy";
        if (player.activeInHierarchy)
        {
            playerEnergySlider.value = player.GetComponent<PlayerController>().currentEnergy;
        }
        
        if (gameIsPaused)
        {
            Time.timeScale = 0;
        }
        if (gameIsPaused == false&&!isDead)
        {
            Time.timeScale = 1;
            pauseMenuImage.SetActive(false);
        }
    
        if (playerHealth <=0)
        {
            EndGame("You have been shot down, your score is\n " + score) ;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused== true)
            {
                ResumeGame();
            }
            else
            {
                PauseMenu();
            }
            
        }
        if (isDead == true)
        {
            // Show the final score image
            finalScoreImage.SetActive(true);
            //EndGame("You Crashed");
        }
        else
        {
            finalScoreImage.SetActive(false);
        }
    }

    // Function to add points to the score
    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("score is :"+score);
    }

    // Function to remove a life from the player
    public void RemoveLife()
    {
        humanlives--;
        Debug.Log("Human lives left " + humanlives);
        if (humanlives <= 0)
        {
            // End the game
            EndGame("Mission Failed! We lost too many!"+"Your score is :\n"+ score);
        }
    }

    // Function to end the game
    public void EndGame(string endgameText)
    {
        isDead = true;
        // Stop the game
        Time.timeScale = 0.5f;
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);

        // Set the final score text
        finalScoreImage.GetComponentInChildren<Text>().text = endgameText ;
                
        // Register a click event handler for the button
        endGameRestartButton.onClick.AddListener(() =>
        {
            RestartLevel();     
        });
        endGameMainMenuButton.onClick.AddListener(() =>
        {
            // Handle the button click here
            GoToMainMenu();

        });
    }
    public void PauseMenu()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
        pauseMenuImage.SetActive(true);
        pauseMenuResumeButton.onClick.AddListener(() =>
        {
            ResumeGame();
        });
        // Register a click event handler for the button
        pauseMenuRestartButton.onClick.AddListener(() =>
        {
            RestartLevel();
        });
        pauseMenuMainMenuButton.onClick.AddListener(() =>
        {
            // Handle the button click here
            GoToMainMenu();

        });
    }
    public void ResumeGame()
    {
        // Unpause the game
        gameIsPaused = false;
        Time.timeScale = 1;
        pauseMenuImage.SetActive(false);
        

    }
    public void RestartLevel()
    {
        ////reset player health
        playerHealth = maxPlayerHealth;
        //// Reset the timer
        timer = 0f;

        //// Reset the score
        score = 0;

        //// Reset the number of lives
        humanlives = 3;

        //// Hide the final score image
        finalScoreImage.gameObject.SetActive(false);

        //// Unpause the game
        Time.timeScale = 1;

        // Restart the current level
        SceneManager.LoadScene("Play");
    }
    public void GoToMainMenu()
    {
        // Hide the final score image
        finalScoreImage.gameObject.SetActive(false);

        // Unpause the game
        Time.timeScale = 1;

        // Go to the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
    public void TakeDamage(int damage)
    {
        playerHealth = -damage ;
    }

    







}


