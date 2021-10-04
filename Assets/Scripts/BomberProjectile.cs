using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberProjectile : MonoBehaviour
{
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerCharacter p= collision.GetComponent<PlayerCharacter>();
        if (p != null) {
            p.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
