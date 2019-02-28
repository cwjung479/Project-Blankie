using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPool : MonoBehaviour
{
    public int poolSize = 10;
    public GameObject prefab;
    public float spawnRate = 4f;
    public float minHeight = -1f;
    public float maxHeight = 3.5f;

    private GameObject[] pool;
    private Rigidbody2D[] rbs;
    private Vector2 poolPosition = new Vector2(-150f, -250f);
    private float timeSinceLastSpawned;
    private float spawnXPosition = -12f;
    private int current = 0;

    void Start() {
        pool = new GameObject[poolSize];
        rbs = new Rigidbody2D[poolSize];
        for (int i = 0; i < poolSize; i++) {
            pool[i] = (GameObject)Instantiate(prefab, poolPosition, Quaternion.identity);
            rbs[i] = pool[i].GetComponent<Rigidbody2D>();
        }
    }

    void Update() {
        timeSinceLastSpawned += Time.deltaTime;
        if (GameControl.instance.gameOver == false && timeSinceLastSpawned >= spawnRate) {
            timeSinceLastSpawned = 0;

            pool[current].transform.position = transform.position;
            rbs[current].velocity = GetComponent<BedBossController>().player.transform.position - transform.position;
            current++;
            if (current >= poolSize) {
                current = 0;
            }
        }
    }
}
