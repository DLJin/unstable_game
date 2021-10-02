using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int verticalRange = 5;
    public GameObject[] enemies;

    private Vector3 originalPosition;

    void Start() {
        originalPosition = transform.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) == true) {
            transform.position = originalPosition + Vector3.up * Random.Range(-verticalRange, verticalRange + 1);
            GameObject go = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
            go.GetComponent<Enemy>().setAngle(Random.Range(-3, 4) * 15);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(transform.position.x, transform.position.y - verticalRange, 0),
            new Vector3(transform.position.x, transform.position.y + verticalRange, 0));
    }
}
