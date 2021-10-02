using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyDistribution
    {
        public GameObject prefab;
        public Enemy.MovePattern movePattern;
        [Range(0, 101)]
        public int percentage;
        public int minSpeed, maxSpeed;
    }

    public int verticalRange = 5;
    [Tooltip("Note: Percentages must sum to 100")]
    public EnemyDistribution[] enemies;

    private Vector3 originalPosition;

    void Start() {
        originalPosition = transform.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) == true) {
            transform.position = originalPosition + Vector3.up * Random.Range(-verticalRange, verticalRange + 1);

            int enemySelection = Random.Range(0, 100);
            int totalPercentage = 0;
            foreach(EnemyDistribution e in enemies) {
                totalPercentage += e.percentage;
                if (enemySelection < totalPercentage) {
                    GameObject go = Instantiate(e.prefab, transform.position, Quaternion.identity);
                    Enemy enemyConfig = go.GetComponent<Enemy>();
                    enemyConfig.InitializeEnemy(movePattern: e.movePattern, angle: Random.Range(-2, 3) * 15, speed: Random.Range(e.minSpeed, e.maxSpeed + 1));
                    break;
                }
            }
        }
    }

    private void OnValidate() {
        int totalPercentage = 0;
        foreach(EnemyDistribution e in enemies) {
            totalPercentage += e.percentage;
        }
        if (totalPercentage != 100) {
            Debug.LogWarning("Resetting enemy distribution percentages.");
            totalPercentage = 0;
            for (int i = 0; i < enemies.Length - 1; ++i) {
                enemies[i].percentage = 100/enemies.Length;
                totalPercentage += 100 / enemies.Length;
            }
            enemies[enemies.Length - 1].percentage = 100 - totalPercentage;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(transform.position.x, transform.position.y - verticalRange, 0),
            new Vector3(transform.position.x, transform.position.y + verticalRange, 0));
    }
}
