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
    public float movementSpeed = 0.1f;
    public float baseMovementSpeed = 0.1f;

    public int maxHealth = 10;
    public int currHealth = 10;
    public float timeSinceLastRandomPower = 0.0f;
    public float nextPowerTime = 5.0f;
    public float nextPowerTimeRange = 5.0f;
    public float nextPowerTimeMin = 5.0f;
    public int maxPower = 5;
    public int currPower = 0;
    public float powerScale = 2.0f;

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
        //onGround = IsGrounded();
    }
    void FixedUpdate()
    {
        calculateMovement();

        if (timeSinceLastAttack > 1.0f/attackSpeed && Input.GetKey(KeyCode.Space)) {
            Quaternion rotation = new Quaternion(0, 0, 90, 90);
            GameObject go = Instantiate(projectile, transform.position, rotation);
            go.GetComponent<Rigidbody2D>().velocity = Vector3.right * projectileSpeed;
            timeSinceLastAttack = 0.0f;
        }
        else {
            timeSinceLastAttack += Time.deltaTime;
        }

    }

    bool IsGrounded() {
        Debug.DrawRay(transform.position, -Vector3.up, Color.green);
        Debug.Log("distToGround: " + distToGround);
        Debug.Log("transform.position: " + transform.position);
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 20.1f);
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

        anim.SetBool("Walking", rb.velocity.magnitude > 0 || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision!");
        if (collision.gameObject.GetComponent<CollapsingGround>() != null &&
            collision.gameObject.transform.position.y < transform.position.y) {
                Debug.Log("Collision is with something that has CollapsingGround!");
                onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("Exit!");
        if (collision.gameObject.GetComponent<CollapsingGround>() != null) {
            Debug.Log("Exit is with something that has CollapsingGround!");
            onGround = false;
        }
    }
}
 