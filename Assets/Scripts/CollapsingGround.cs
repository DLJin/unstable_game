using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingGround : MonoBehaviour
{

    public Sprite crackedSprite;
    [Range(0f, 1f)]
    public float crackThreshold = 0.75f;
    public bool onlyCrackDuringContact = false;
    public float lifeTime = 3f;

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
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.GetComponent<PlayerCharacter>() != null) {
            if (collision.transform.position.y > transform.position.y) {
                startCollapse = true;
            }
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
