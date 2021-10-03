using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float scrollSpeed = 4f;
    public string type = "weapon";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - Vector3.right * Time.deltaTime * scrollSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Pickup Collision!");
        if (collision.gameObject.GetComponent<PlayerCharacter>() != null) {
            Destroy(gameObject);
        }
    }
}
