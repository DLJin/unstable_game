using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource BGM;
    public PlayerCharacter player;

    public static bool isPaused;
    [HideInInspector]
    public bool isMuted;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isMuted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PauseGame() {
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
    }

    public void MuteGame() {
        BGM.volume = isMuted? 1 : 0;
        isMuted = !isMuted;
    }
}