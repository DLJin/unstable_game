using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject projectile;
    public float projectileSpeed = 20.0f;

    private float timeSinceLastAttack = 0.0f;
    public float attackSpeed = 5.0f;
    public float baseAttackSpeed = 5.0f;
    public float movementSpeed = 0.5f;
    public float baseMovementSpeed = 0.5f;

    public int maxHealth = 10;
    public int currHealth = 10;
    public int weaponDamage = 10;

    private Animator anim;
    public bool onGround = false;
    public bool jumping = false;
    public int jumpingFrameCount = 4;
    public int jumpingCurrentFrameCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update() 
    {
    }
    void FixedUpdate()
    {
        calculateJumping();
        calculateMovement();

        if (timeSinceLastAttack > 1.0f/attackSpeed && Input.GetKey(KeyCode.Space)) {
            Quaternion rotation = new Quaternion(0, 0, 90, 90);
            GameObject go = Instantiate(projectile, transform.position, rotation);
            go.GetComponent<Projectile>().damage = weaponDamage;
            go.GetComponent<Rigidbody2D>().velocity = Vector3.right * projectileSpeed;
            timeSinceLastAttack = 0.0f;
        }
        else {
            timeSinceLastAttack += Time.deltaTime;
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

        if (rb.velocity.x < -5f) {
            rb.velocity = new Vector3(-5f, rb.velocity.y);
        }
        if (rb.velocity.x > 5f) {
            rb.velocity = new Vector3(5f, rb.velocity.y);
        }

        anim.SetBool("Walking", rb.velocity.magnitude > 0 || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
    }

    void calculateJumping() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // starting to jump
        if (Input.GetKey(KeyCode.W) && onGround && !jumping) {
            jumping = true;
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

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision!");

        if (collision.gameObject.GetComponent<CollapsingGround>() != null) {
                if(collision.contactCount > 0) {
                    ContactPoint2D contact = collision.GetContact(0);
                    if(Vector3.Dot(contact.normal, Vector3.up) > 0.5) {
                        onGround = true;
                    }
                }
        } else if (collision.gameObject.GetComponent<Enemy>() != null) {
            Destroy(collision.gameObject);
            currHealth--;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("Exit!");
        if (collision.gameObject.GetComponent<CollapsingGround>() != null) {
            Debug.Log("Exit is with something that has CollapsingGround!");
            onGround = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Pickup>() != null) {
            if (collision.gameObject.GetComponent<Pickup>().type == "weapon") {
                weaponDamage += 10;
            }
            else {
                Debug.Log("UNKNOWN PICKUP");
            }
        }

    }
}
 