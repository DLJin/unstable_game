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
    public float movementSpeed = 5.0f;
    public float baseMovementSpeed = 5.0f;

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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        attackSpeed = baseAttackSpeed + powerScale*currPower;
        movementSpeed = baseMovementSpeed + powerScale*currPower;

        if (timeSinceLastRandomPower >= nextPowerTime) {
            if (currPower >= maxPower) {
                Debug.Log("Power overload!");
                currPower = 0;
            }
            else {
                currPower++;
            }
            timeSinceLastRandomPower = 0.0f;
            nextPowerTime = Random.Range(nextPowerTimeMin, nextPowerTimeMin + nextPowerTimeRange);
        }
        else {
            timeSinceLastRandomPower += Time.deltaTime;
        }
    }

    // FixedUpdate is called once per physics frame
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

    void calculateMovement() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector3 newVelocity = new Vector3(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            newVelocity.y = newVelocity.y + movementSpeed;
        }
        if (Input.GetKey(KeyCode.A)) {
            newVelocity.x = newVelocity.x - movementSpeed;
        }
        if (Input.GetKey(KeyCode.S)) {
            newVelocity.y = newVelocity.y - movementSpeed;
        }
        if (Input.GetKey(KeyCode.D)) {
            newVelocity.x = newVelocity.x + movementSpeed;
        }

        rb.velocity = newVelocity;
        anim.SetBool("Walking", newVelocity.magnitude > 0 || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
    }
}
 