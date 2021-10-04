using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingGround : MonoBehaviour
{
    public bool isPlatform;
    public Sprite crackedSprite;
    public GameObject destroyedPlatform;
    [Range(0f, 1f)]
    public float crackThreshold = 0.1f;
    public bool onlyCrackDuringContact = false;
    public float lifeTime = 2f;

    private bool spriteChanged;
    private bool startCollapse;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        spriteChanged = false;
        startCollapse = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(startCollapse) {
            timer += Time.deltaTime;
        }

        if(!spriteChanged && timer >= crackThreshold * lifeTime) {
            GetComponent<SpriteRenderer>().sprite = crackedSprite;
            spriteChanged = true;
        }
        if(timer >= lifeTime) {
            if (isPlatform) {
                Instantiate(destroyedPlatform, transform.position, Quaternion.identity);
                Destroy(gameObject);
            } else {
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.GetComponent<PlayerCharacter>() != null) {
            if(collision.contactCount > 0) {
                ContactPoint2D contact = collision.GetContact(0);
                if(Vector3.Dot(contact.normal, Vector3.down) > 0.5) {
                    startCollapse = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<BomberProjectile>() != null) {
            Destroy(collision.gameObject);
            startCollapse = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<PlayerCharacter>() != null) {
            if (startCollapse && onlyCrackDuringContact) {
                startCollapse = false;
            }
        }
    }
}
