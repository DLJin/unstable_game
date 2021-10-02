using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum MovePattern { Straight, Curve, Wave }
    public int health = 100;
    public int attack = 5;
    public float speed = 2f;
    [Range(-45, 45)]
    public int angle = 0;
    public MovePattern movePattern = MovePattern.Straight;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setAngle(int angle) {
        this.angle = angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = -1 * transform.right * speed;
    }
}
