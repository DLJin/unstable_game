using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Rigidbody2D player;
    public GroundController ground;
    public GameObject[] far, mid, close;
    public float farSpeed, midSpeed, closeSpeed;

    private int farIndex, midIndex, closeIndex;

    private void Start() {
        farIndex = 0;
        midIndex = 0;
        closeIndex = 0;
    }

    void Update()
    {
        if (player == null) {
            return;
        }
        float xSpeed = player.velocity.x + ground.scrollSpeed;

        foreach(GameObject f in far) {
            f.transform.position = new Vector3(f.transform.position.x - xSpeed * farSpeed * Time.deltaTime, f.transform.position.y, 0);
        }
        foreach (GameObject m in mid) {
            m.transform.position = new Vector3(m.transform.position.x - xSpeed * midSpeed * Time.deltaTime, m.transform.position.y, 0);
        }
        foreach (GameObject c in close) {
            c.transform.position = new Vector3(c.transform.position.x - xSpeed * closeSpeed * Time.deltaTime, c.transform.position.y, 0);
        }

        if(far[farIndex].transform.position.x <= -28.8) {
            far[farIndex].transform.position = new Vector3(far[(farIndex + 2) % 3].transform.position.x + 28.8f, 0, 0);
            farIndex = (farIndex + 1) % 3;
        }
        if (mid[midIndex].transform.position.x <= -28.8) {
            mid[midIndex].transform.position = new Vector3(mid[(midIndex + 2) % 3].transform.position.x + 28.8f, 0, 0);
            midIndex = (midIndex + 1) % 3;
        }
        if (close[closeIndex].transform.position.x <= -28.8) {
            close[closeIndex].transform.position = new Vector3(close[(closeIndex + 2) % 3].transform.position.x + 28.8f, 0, 0);
            closeIndex = (closeIndex + 1) % 3;
        }
    }
}
