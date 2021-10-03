using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PowerType
    {
        Heal, Speed, Frequency
    }

    public PowerType type = PowerType.Heal;
    public float speedDampening = 0.8f;
    public float floatSpeed = 0.2f;

    void Update()
    {
        transform.position = transform.position + Vector3.left * Time.deltaTime * GameManager.groundSpeed * speedDampening + Vector3.down * floatSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Pickup Collision!");
        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null) {
            player.applyPowerup(type);
            Destroy(gameObject);
        }
    }
}
