using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioClip deathNoise;
    public AudioSource PlayerSFX;
    public PlayerCharacter player;
    public GroundController ground;

    public static float groundSpeed = 4f;
    public static bool isPaused;
    [HideInInspector]
    public bool isMuted;

    public Color[] bgColors;
    public float colorTransitionTime;
    private float colorTimer;
    private Color originalColor;
    private Color targetColor;
    public float timeToSurvive = 10.0f;
    private bool lost = false;
    public bool isGameOver;

    public float endingSlowFadeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        isGameOver = false;
        isPaused = true;
        isMuted = false;
        ground.scrollSpeed = groundSpeed;
        SelectNewColor();
    }

    // Update is called once per frame
    void Update()
    {
        colorTimer += Time.deltaTime;
        Camera.main.backgroundColor = Color.Lerp(originalColor, targetColor, colorTimer / colorTransitionTime);
        if (colorTimer >= colorTransitionTime) {
            SelectNewColor();
        }

        if (!isGameOver) {
            timeToSurvive -= Time.deltaTime;
            if (timeToSurvive < 0.0f) {
                EndGame(true);
            }
        }
    }

    private void SelectNewColor() {
        colorTimer = 0;
        Color newColor;
        do {
            newColor = bgColors[Random.Range(0, bgColors.Length)];
        } while (newColor == targetColor);
        originalColor = Camera.main.backgroundColor;
        targetColor = newColor;
    }

    public static void PauseGame() {
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
    }

    public void MuteGame() {
        BGM.volume = isMuted ? 1 : 0;
        PlayerSFX.volume = isMuted ? 0.15f : 0;
        isMuted = !isMuted;
    }

    public void EndGame(bool winner) {
        if (!isGameOver) {
            Debug.LogError("Game Over!");
            isGameOver = true;
            if (winner && !lost) {
                Debug.Log("Player Won!");
            } else {
                lost = true;
                BGM.PlayOneShot(deathNoise);
            }
            StartCoroutine(SlowDownTime());
        }
    }

    IEnumerator SlowDownTime() {
        float slowTimer = 0;
        while(slowTimer < endingSlowFadeTime) {
            slowTimer += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(1, 0.3f, slowTimer / endingSlowFadeTime);
            yield return null;
        }
        yield return null;
    }
}
