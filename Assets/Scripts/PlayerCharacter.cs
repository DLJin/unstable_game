using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Animator anim;
    public GameManager gameManager;

    [Header("Projectile")]
    public GameObject projectile;
    public Transform spawnPoint;
    public float projectileSpeed = 20.0f;

    [Header("Attack")]
    public float attackSpeed = 5.0f;
    public float baseAttackSpeed = 5.0f;
    public int weaponDamage = 10;
    private float timeSinceLastAttack = 0.0f;

    [Header("Movement")]
    public float movementSpeed = 0.5f;
    public float baseMovementSpeed = 0.5f;
    public float maxBaseMovementSpeed = 5f;

    [Header("Health")]
    public int maxHealth = 10;
    public int currHealth = 10;

    [Header("Powerups")]
    public int speedUps = 0;
    public float speedUpPerLevel = 1f;
    public int frequencyUps = 0;
    public float frequencyUpPerLevel = 1f;

    [Header("Jumping")]
    public bool onGround = false;
    public bool jumping = false;
    public int jumpingFrameCount = 5;
    public int jumpingCurrentFrameCount = 0;

    [Header("Sounds")]
    public AudioClip[] shootSounds;
    public AudioClip[] hurtSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] collectSounds;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastAttack = 0.0f;
        currHealth = 10;
        weaponDamage = 10;
        speedUps = 0;
        frequencyUps = 0;
        onGround = false;
        jumping = false;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {

    }
    
    void FixedUpdate()
    {
        calculateJumping();
        calculateMovement();

        if (timeSinceLastAttack > 1.0f/(attackSpeed + frequencyUpPerLevel * frequencyUps) && Input.GetKey(KeyCode.Space)) {
            Quaternion rotation = new Quaternion(0, 0, 90, 90);
            GameObject go = Instantiate(projectile, spawnPoint.position, rotation);
            go.GetComponent<Projectile>().damage = weaponDamage;
            go.GetComponent<Rigidbody2D>().velocity = Vector3.right * projectileSpeed;
            timeSinceLastAttack = 0.0f;
            PlaySound(shootSounds);
        }
        else {
            timeSinceLastAttack += Time.deltaTime;
        }

    }

    private void OnDestroy() {
        gameManager.EndGame(currHealth <= 0);
    }

    public void TakeDamage(int dmg = 1) {
        currHealth = Mathf.Clamp(currHealth - dmg, 0, maxHealth);
        if (dmg > 0) {
            PlaySound(hurtSounds);
            anim.SetTrigger("Damaged");
        }
        if (currHealth == 0) {
            Debug.LogError("Player is dead!");
            Destroy(gameObject);
        }
    }

    void calculateMovement() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        if (Input.GetKey(KeyCode.A)) {
            rb.AddForce(movementSpeed*Time.deltaTime*Vector3.left);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb.AddForce(movementSpeed*Time.deltaTime*Vector3.right);
        }

        float maxMovementSpeed = maxBaseMovementSpeed + speedUps * speedUpPerLevel;
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxMovementSpeed, maxMovementSpeed), rb.velocity.y);
        anim.SetFloat("Velocity", rb.velocity.x/4f + 1);
    }

    void calculateJumping() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // starting to jump
        if (Input.GetKey(KeyCode.W) && onGround && !jumping) {
            Debug.Log("Starting Jump [onGround: " + onGround + ", jumping: " + jumping + "]");
            jumping = true;
            PlaySound(jumpSounds);
            anim.SetTrigger("Jumping");
            jumpingCurrentFrameCount = 1;
        }
        else if (jumping) {
            // end of jump
            if (jumpingCurrentFrameCount > jumpingFrameCount) {
                jumping = false;
                jumpingCurrentFrameCount = 0;
            }
            // in the middle of jumping
            if (Input.GetKey(KeyCode.W) && (jumpingCurrentFrameCount == 1 || jumpingCurrentFrameCount == jumpingFrameCount)) {
                rb.AddForce(1.75f*Time.deltaTime*Vector3.up);
            }
            jumpingCurrentFrameCount++;
        }
    }

    public void applyPowerup(Pickup.PowerType type) {
        PlaySound(collectSounds);
        switch(type) {
            case Pickup.PowerType.Heal:
                TakeDamage(-1);
                break;
            case Pickup.PowerType.Speed:
                if(speedUps < 3) {
                    speedUps++;
                }
                break;
            case Pickup.PowerType.Frequency:
                if(frequencyUps < 3) {
                    frequencyUps++;
                }
                break;
        }
    }

    public void losePowerup(Pickup.PowerType type) {
        // Todo: Instantiate powerup falling from player or some other visual indicator
        switch (type) {
            case Pickup.PowerType.Speed:
                if(speedUps > 0) {
                    speedUps--;
                }
                break;
            case Pickup.PowerType.Frequency:
                if(frequencyUps > 0) {
                    frequencyUps--;
                }
                break;
        }
    }

    public void PlaySound(AudioClip[] sounds) {
        if (sounds.Length == 0) {
            return;
        }
        int clip = Random.Range(0, sounds.Length);
        Debug.Log("Playing " + sounds[clip].name);
        audioSource.PlayOneShot(sounds[clip]);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("Collision!");

        if (collision.gameObject.GetComponent<CollapsingGround>() != null) {
            if(collision.contactCount > 0) {
                ContactPoint2D contact = collision.GetContact(0);
                if(Vector3.Dot(contact.normal, Vector3.up) > 0.5) {
                    onGround = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Enemy>() != null) {
            PlaySound(hurtSounds);
            Destroy(collision.gameObject);
            TakeDamage();
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        //Debug.Log("Exit!");
        if (collision.gameObject.GetComponent<CollapsingGround>() != null) {
            //Debug.Log("Exit is with something that has CollapsingGround!");
            onGround = false;
        }
    }
}
 