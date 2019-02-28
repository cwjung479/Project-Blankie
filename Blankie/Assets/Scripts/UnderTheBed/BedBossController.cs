using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBossController : MonoBehaviour
{
    public int health = 50;
    public int damage = 1;
    public bool dead = false;
    public GameObject spring;
    public GameObject pillow;
    public GameObject player;
    public Transform leftBound;
    public Transform rightBound;

    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 startPos;
    private int nextAttack = 0;
    private ActionType lastState = ActionType.Idle;
    private ActionType curState = ActionType.Idle;
    private float bossWaitTime = 10;
    public float curWaitTime = 0;

    private bool swap = false;

    public enum ActionType {
        Idle,
        Moving,
        Patrolling,
        Attacking
    }

    void Start () {
        leftPos = leftBound.position;
        rightPos = rightBound.position;
        startPos = transform.position;
    }

    void Update()
    {
        switch (curState) {
            case ActionType.Moving:
                move();
                if (lastState == ActionType.Patrolling)
                    curState = ActionType.Idle;
                else if (lastState == ActionType.Idle)
                    curState = ActionType.Patrolling;
                lastState = ActionType.Moving;
                break;
            case ActionType.Patrolling:
                patrol();
                if (lastState == ActionType.Moving)
                    curState = ActionType.Attacking;
                else if (lastState == ActionType.Attacking)
                    curState = ActionType.Moving;
                lastState = ActionType.Patrolling;
                break;
            case ActionType.Attacking:
                if (nextAttack == 0)
                    launchSpring();
                else if (nextAttack == 1)
                    launchPillow();
                else if (nextAttack == 2)
                    charge();
                nextAttack = (health * 3 / 2 + Mathf.FloorToInt(Time.time)) % 3;
                curState = ActionType.Patrolling;
                lastState = ActionType.Attacking;
                break;
            default:
                if (curWaitTime >= bossWaitTime) {
                    curState = ActionType.Moving;
                    lastState = ActionType.Idle;
                } else {
                    curWaitTime += Time.deltaTime;
                }
                break;
        }
    }

    public int takeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            dead = true;
            health = 0;
        }
        return health;
    }

    void patrol() {

    }

    void move() {
        if (transform.position.x ==  leftPos.x) {
            swap = true;
        }

        if (transform.position.x == rightPos.x) {
            swap = false;
        }

        if (swap) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(rightPos.x, transform.position.y), 3 * Time.deltaTime);
        } else {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(leftPos.x, transform.position.y), 3 * Time.deltaTime);
        }

    }

    void launchSpring() {

        Debug.Log("Launch Spring");
    }

    void launchPillow() {
        Debug.Log("Launch Pillow");
    }

    void charge() {
        Debug.Log("Charge");
    }
}
