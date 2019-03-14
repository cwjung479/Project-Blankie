using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollSpawner : MonoBehaviour
{
    public Transform dollPrefab;
    public float spawnRate;

    public GameObject player;
    public float xTriggerPosition = 128f;

    private bool active = false;

    private float time;
    private Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = new Vector2(144, -13);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > xTriggerPosition && !active)
        {
            active = true;
            time = Time.time;
        }

        if (active)
        {
            if (Time.time - time > spawnRate)
            {
                Instantiate(dollPrefab, spawnPosition, Quaternion.identity);
                time = Time.time;
            }
        }
    }
}
