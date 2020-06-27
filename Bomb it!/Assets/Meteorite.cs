using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField] float delayTime = 0f;
    [SerializeField] float speed = 25f;
    [SerializeField] bool randomizeSpeed = false;
    [SerializeField] float speedMinRange = 0f;
    [SerializeField] float speedMaxRange = 0f;
    [SerializeField] float respawnTime = 0f;
    [SerializeField] bool randomizeRespawnTime = false;
    [SerializeField] float respawMinRange = 0f;
    [SerializeField] float respawnMaxRange = 0f;


    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool moving = false;
    private bool restart = false;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayTime);
        startPosition = transform.position;
        endPosition = transform.Find("endPosition").transform.position;  // prefab Meteorite(parent), inside Meteorite child named "endPosition" which represent end point
        if (randomizeRespawnTime)
        {
            SetRandomRespawnTime();
        }
        if (randomizeSpeed)
        {
            SetRandomSpeed();
        }
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            MoveMeteorite();
            CheckMeteoritePosition();
        }
        if (restart)
        {
            RestartMovingMeteorite();
        }
    }

    private void MoveMeteorite()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
    }

    private void RestartMovingMeteorite()
    {
        restart = false;
        if (randomizeRespawnTime)
        {
            SetRandomRespawnTime();
        }
        if (randomizeSpeed)
        {
            SetRandomSpeed();
        }
        StartCoroutine(MovingDelay());

    }

    private void CheckMeteoritePosition()
    {
        if (transform.position == endPosition)
        {
            transform.position = startPosition;
            moving = false;
            restart = true;
        }
    }

    private void SetRandomRespawnTime()
    {
        respawnTime = Random.Range(respawMinRange, respawnMaxRange);
        print(respawnTime);
    }

    private void SetRandomSpeed()
    {
        speed = Random.Range(speedMinRange, speedMaxRange);
        print(speed);
    }

    IEnumerator MovingDelay()
    {
        yield return new WaitForSeconds(respawnTime);
        moving = true;
    }
}
