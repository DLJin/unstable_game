using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    public float fadeStart = 1f;
    public float fadeTime = 2f;

    float timer;

    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > fadeStart) {
            for(int i = 0; i < transform.childCount; ++i) {
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Clamp01((fadeStart + fadeTime - timer) / fadeTime));
            }
            if (timer > fadeStart + fadeTime) {
                Destroy(gameObject);
            }
        }
    }
}
