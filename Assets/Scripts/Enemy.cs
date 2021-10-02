using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum MovePattern { Straight, Curve, Wave }
    public int health = 100;
    public int attack = 5;
    public float speed = 2f;
    [Range(0.25f, 2.5f)]
    public float waveSpeed = 1.5f;
    public float turnSpeed = 4f;
    [Range(-45, 45)]
    public int angle = 0;
    public MovePattern movePattern = MovePattern.Straight;

    private Rigidbody2D rb;
    private float time;

    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        time = 0;
        //setAngle(angle);
        StartMovement();
    }

    private void FixedUpdate() {
        time += Time.fixedDeltaTime;
        if (movePattern == MovePattern.Wave) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Quaternion.LookRotation(Vector3.forward, new Vector3(Mathf.Cos(time * Mathf.PI * waveSpeed), 1, 0)).eulerAngles.z - angle), Time.fixedDeltaTime * turnSpeed);
            rb.velocity = -1 * transform.right * speed;
        }
    }

    public void InitializeEnemy(MovePattern movePattern, int angle, float speed) {
        this.movePattern = movePattern;
        this.angle = angle;
        this.speed = speed;

        StartMovement();
    }

    public void StartMovement() {
        transform.rotation = Quaternion.Euler(0, 0, -angle);
        rb.velocity = -1 * transform.right * speed;
    }

    public void setAngle(int angle) {
        this.angle = angle;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
        rb.velocity = -1 * transform.right * speed;
    }
}
