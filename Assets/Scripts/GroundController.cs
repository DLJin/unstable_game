using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform blockParent;

    public float scrollSpeed = 4f;
    public GameObject[] groundBlocks;

    private GameObject lastSpawned;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawned = Instantiate(groundBlocks[Random.Range(0, groundBlocks.Length)], spawnPoint.position, Quaternion.identity, blockParent);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnPoint.position.x - lastSpawned.transform.position.x >= 8) {
            lastSpawned = Instantiate(groundBlocks[Random.Range(0, groundBlocks.Length)], lastSpawned.transform.position + Vector3.right * 8, Quaternion.identity, blockParent);
        }
        for(int i = 0; i < blockParent.childCount; ++i) {
            blockParent.GetChild(i).position = blockParent.GetChild(i).position - Vector3.right * Time.deltaTime * scrollSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<CollapsingGround>() != null) {
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
