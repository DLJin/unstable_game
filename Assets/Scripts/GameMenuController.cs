using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameMenuController : MonoBehaviour
{
    public string winText = "Congratulations, you survived the invasion! The robot will live to see another day!";
    public string loseText = "Oh no! The invaders won this time...";

    [HideInInspector]
    public Button startButton;
    [HideInInspector]
    public Button pauseButton;
    [HideInInspector]
    public Button muteButton;
    [HideInInspector]
    public Button replayButton;
    [HideInInspector]
    public VisualElement startOverlay;
    [HideInInspector]
    public VisualElement endOverlay;
    [HideInInspector]
    public VisualElement playerHealth;
    [HideInInspector]
    public VisualElement playerSpeed;
    [HideInInspector]
    public VisualElement playerAttack;
    [HideInInspector]
    public VisualElement playerProgress;
    [HideInInspector]
    public Label flavorText;

    public GameManager gameManager;

    private float totalTime;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("start-button");
        pauseButton = root.Q<Button>("pause-button");
        muteButton = root.Q<Button>("mute-button");
        replayButton = root.Q<Button>("replay-button");
        startOverlay = root.Q<VisualElement>("start-overlay");
        endOverlay = root.Q<VisualElement>("end-overlay");
        playerHealth = root.Q<VisualElement>("player-health");
        playerSpeed = root.Q<VisualElement>("player-speed");
        playerAttack = root.Q<VisualElement>("player-attack");
        playerProgress = root.Q<VisualElement>("player-progress");
        flavorText = root.Q<Label>("flavor-text");

        startButton.clicked += StartGame;
        pauseButton.clicked += TogglePause;
        muteButton.clicked += ToggleMute;
        replayButton.clicked += RestartGame;

        totalTime = gameManager.timeToSurvive;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01((float) gameManager.player.currHealth / gameManager.player.maxHealth)));
        playerSpeed.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01((float)gameManager.player.speedUps / 3)));
        playerAttack.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01((float)gameManager.player.frequencyUps / 3)));
        playerProgress.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01(gameManager.timeToSurvive / totalTime)));
        if (gameManager.isGameOver) {
            endOverlay.style.visibility = Visibility.Visible;
            flavorText.text = gameManager.lost ? loseText : winText;
            if (!gameManager.lost && gameManager.player == null) {
                flavorText.text = winText + " ... oops";
            }
        }
    }

    private void StartGame() {
        startOverlay.style.visibility = Visibility.Hidden;
        GameManager.PauseGame();
    }

    private void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void TogglePause() {
        GameManager.PauseGame();
        pauseButton.text = GameManager.isPaused ? "Resume" : "Pause";
    }

    private void ToggleMute() {
        gameManager.MuteGame();
        muteButton.text = gameManager.isMuted ? "Unmute" : "Mute";
    }
}
