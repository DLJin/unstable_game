using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Projectile>() != null || collision.GetComponent<Enemy>() != null || collision.GetComponent<PlayerCharacter>() != null) {
            Destroy(collision.gameObject);
        }
    }
}
