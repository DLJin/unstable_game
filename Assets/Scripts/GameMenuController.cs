using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMenuController : MonoBehaviour
{
    [HideInInspector]
    public Button pauseButton;
    [HideInInspector]
    public Button muteButton;
    [HideInInspector]
    public VisualElement playerHealth;
    [HideInInspector]
    public VisualElement playerSpeed;
    [HideInInspector]
    public VisualElement playerAttack;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        pauseButton = root.Q<Button>("pause-button");
        muteButton = root.Q<Button>("mute-button");
        playerHealth = root.Q<VisualElement>("player-health");
        playerSpeed = root.Q<VisualElement>("player-speed");
        playerAttack = root.Q<VisualElement>("player-attack");

        pauseButton.clicked += TogglePause;
        muteButton.clicked += ToggleMute;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01((float) gameManager.player.currHealth / gameManager.player.maxHealth)));
        playerSpeed.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01((float)gameManager.player.speedUps / 3)));
        playerAttack.style.width = new StyleLength(Length.Percent(100 * Mathf.Clamp01((float)gameManager.player.frequencyUps / 3)));
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
