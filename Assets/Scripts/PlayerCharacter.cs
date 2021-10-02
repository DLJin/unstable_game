using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector3 newVelocity = new Vector3(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            newVelocity.y = newVelocity.y + 5;
        }
        if (Input.GetKey(KeyCode.A)) {
            newVelocity.x = newVelocity.x - 5;
        }
        if (Input.GetKey(KeyCode.S)) {
            newVelocity.y = newVelocity.y - 5;
        }
        if (Input.GetKey(KeyCode.D)) {
            newVelocity.x = newVelocity.x + 5;
        }

        rb.velocity = newVelocity;
    }
}
 