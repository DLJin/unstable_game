using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float attackSpeed = 3.0f;
    public GameObject projectile;
    public float projectileSpeed = 20.0f;

    private float timeSinceLastAttack = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    }
}
 