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
    public float movementSpeed = 0.2f;
    public float baseMovementSpeed = 0.2f;

    public int maxHealth = 10;
    public int currHealth = 10;
    public int weaponDamage = 10;

    private Animator anim;
    public bool onGround = false;

    private float distToGround;
    

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        // get the distance to ground
        distToGround = bc.bounds.extents.y;

        anim = GetComponent<Animator>();
    }
    
    void Update() 
    {
    }
    void FixedUpdate()
    {
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

        if (Input.GetKey(KeyCode.W) && onGround) {
            rb.AddForce(10.0f*movementSpeed*Time.deltaTime*Vector3.up);
        }
        if (Input.GetKey(KeyCode.A)) {
            rb.AddForce(movementSpeed*Time.deltaTime*Vector3.left);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb.AddForce(movementSpeed*Time.deltaTime*Vector3.right);
        }

        if (rb.velocity.x < -6f) {
            rb.velocity = new Vector3(-6f, rb.velocity.y);
        }
        if (rb.velocity.x > 6f) {
            rb.velocity = new Vector3(6f, rb.velocity.y);
        }

        anim.SetBool("Walking", rb.velocity.magnitude > 0 || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
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
            // TODO: Player takes damage here
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
 