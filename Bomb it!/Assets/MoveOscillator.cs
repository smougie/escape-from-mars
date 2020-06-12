using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MoveOscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;
    [SerializeField] float delayTime = 0f;

    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (delayTime > 0)  // delay for start MovePlatform() method
        {
            delayTime -= Time.deltaTime;
        }
        else
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        if (period <= Mathf.Epsilon)  // if period is less than 0, avoid dividing by 0
        {
            return;
        }
        float cycles = Time.time / period;  // number of cycles as decimal number, from starting the game
        const float tau = Mathf.PI * 2f;  // tau is equal to 2x PI number, so just raw number 6.28(...)

        // Sine with values from -1 to 1, whole period takes 2 PI or 1 tau, converting number of cycles * tau
        // if cycles is in half (cycles = 0.5f) sin value would be 0 because 0.5 * 2PI gives 1 PI = half cycle
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = Mathf.Sin(cycles * period) / 2f + 0.5f;  // converting sin values from range(-1,1) to range(0,1) by dividing value by two and adding 0.5f
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
