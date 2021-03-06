using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Normal, Fast, Heavy, Camo, Bomber }
    public enum MovePattern { Straight, Wave }
    [System.Serializable]
    public struct PickupDistribution
    {
        public GameObject pickup;
        [Range(0f, 1f)]
        public float dropRate;
    }
    [Header("Projectile")]
    public GameObject projectile;
    public float projectileSpeed = 7.5f;

    [Header("Attack")]
    public float attackSpeed = 1.0f;
    public int weaponDamage = 1;
    private float timeSinceLastAttack = 0.0f;

    [Header("Properties")]
    public EnemyType type;
    public int health = 20;
    public PickupDistribution[] pickups;
    [Header("Movement")]
    public MovePattern movePattern = MovePattern.Straight;
    public float speed = 2f;
    [Range(-45, 45)]
    public int angle = 0;
    [Header("[Wave movement only]")]
    [Range(0.25f, 2.5f)]
    public float waveSpeed = 1.5f;
    public float turnSpeed = 4f;

    private Rigidbody2D rb;
    private float time;

    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        time = 0;
        //setAngle(angle);
        StartMovement();
    }

    private void FixedUpdate() {
        time += Time.fixedDeltaTime;
        if (movePattern == MovePattern.Wave) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Quaternion.LookRotation(Vector3.forward, new Vector3(Mathf.Cos(time * Mathf.PI * waveSpeed), 1, 0)).eulerAngles.z - angle), Time.fixedDeltaTime * turnSpeed);
            rb.velocity = -1 * transform.right * speed;
        }

        // shooting logic
        shoot();
    }

    public void InitializeEnemy(int angle) {
        this.angle = angle;
        StartMovement();
    }

    public void StartMovement() {
        transform.rotation = Quaternion.Euler(0, 0, -angle);
        rb.velocity = -1 * transform.right * speed;
    }

    public void shoot() {
        timeSinceLastAttack += Time.deltaTime;

        if (type == EnemyType.Normal) {
            return;
        }
        else if (type == EnemyType.Fast || type == EnemyType.Heavy) {
            if (timeSinceLastAttack > 1.0f/(attackSpeed)) {
                Quaternion rotation = new Quaternion(0, 0, 90, 90);
                GameObject go = Instantiate(projectile, transform.position, rotation);
                go.GetComponent<EnemyProjectile>().damage = weaponDamage;
                go.GetComponent<Rigidbody2D>().velocity = Vector3.left * projectileSpeed;
                timeSinceLastAttack = 0.0f;
            }
        }
        else if (type == EnemyType.Bomber) {
            if (timeSinceLastAttack > 1.0f/(attackSpeed)) {
                Quaternion rotation = new Quaternion(0, 0, 90, 90);
                GameObject go = Instantiate(projectile, transform.position, rotation);
                go.GetComponent<BomberProjectile>().damage = weaponDamage;
                go.GetComponent<Rigidbody2D>().velocity = Vector3.down * projectileSpeed;
                timeSinceLastAttack = 0.0f;
            }
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            float dropChance = Random.Range(0f, 1f);
            float distSum = 0;
            Debug.Log("Random chance is " + dropChance);
            foreach(PickupDistribution p in pickups) {
                if(dropChance <= distSum + p.dropRate) {
                    Debug.Log("Spawning " + p.pickup.name);
                    Instantiate(p.pickup, transform.position, Quaternion.identity);
                    break;
                }
                Debug.Log("Skipping " + p.pickup.name);
                distSum += p.dropRate;
            }
            Destroy(gameObject);
        }
    }
}
