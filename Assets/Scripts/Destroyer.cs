using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null || collision.GetComponent<Projectile>() != null) {
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<PlayerCharacter>() != null) {
            Destroy(collision.gameObject);
            GameManager.EndGame();
        }
    }
}
