using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyDistribution
    {
        public GameObject prefab;
        [Range(0, 101)]
        public int percentage;
    }

    [System.Serializable]
    public struct ScriptStep
    {
        public float timestamp;
        public int enemyVariant;
        public float verticalLocation;
        public int angle;
    }

    [System.Serializable]
    public struct EnemySpawningScript
    {
        public ScriptStep[] steps;
    }

    public int verticalRange = 5;
    [Tooltip("Note: Percentages must sum to 100")]
    public EnemyDistribution[] enemies;
    public bool scripted;
    public TextAsset scriptFile;
    public bool autoSpawn = false;
    public float autoSpawnTimer = 0.0f;
    public float autoSpawnSpeed = 3.0f;

    private Vector3 originalPosition;
    private float currentTime;
    private EnemySpawningScript script;
    private int scriptStep;

    private void Awake() {
        if (scripted) {
            script = JsonUtility.FromJson<EnemySpawningScript>(scriptFile.text);
        }
    }

    void Start() {
        currentTime = 0f;
        scriptStep = 0;
        originalPosition = transform.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightShift) == true) {
            autoSpawn = !autoSpawn;
        }
        autoSpawnTimer += Time.deltaTime;

        if (scripted) {
            for(; scriptStep < script.steps.Length && script.steps[scriptStep].timestamp < currentTime; ++scriptStep) {
                transform.position = originalPosition + Vector3.up * script.steps[scriptStep].verticalLocation;
                EnemyDistribution e = enemies[script.steps[scriptStep].enemyVariant];
                GameObject go = Instantiate(e.prefab, transform.position, Quaternion.identity);
                Enemy enemyConfig = go.GetComponent<Enemy>();
                enemyConfig.InitializeEnemy(angle: script.steps[scriptStep].angle);
            }
        } else if (autoSpawn && autoSpawnTimer > 1.0f/autoSpawnSpeed) {
            transform.position = originalPosition + Vector3.up * Random.Range(-verticalRange, verticalRange + 1);

            int enemySelection = Random.Range(0, 100);
            int totalPercentage = 0;
            foreach(EnemyDistribution e in enemies) {
                totalPercentage += e.percentage;
                if (enemySelection < totalPercentage) {
                    GameObject go = Instantiate(e.prefab, transform.position, Quaternion.identity);
                    Enemy enemyConfig = go.GetComponent<Enemy>();
                    enemyConfig.InitializeEnemy(angle: Random.Range(-2, 3) * 15);
                    break;
                }
            }

            autoSpawnTimer = 0.0f;
        }
        currentTime += Time.deltaTime;
    }

    private void OnValidate() {
        int totalPercentage = 0;
        foreach(EnemyDistribution e in enemies) {
            totalPercentage += e.percentage;
        }
        if (totalPercentage != 100) {
            Debug.LogError("Enemy distribution percentage sum is should be 100. Currently: " + totalPercentage);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(transform.position.x, transform.position.y - verticalRange, 0),
            new Vector3(transform.position.x, transform.position.y + verticalRange, 0));
    }
}
