using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RotateOscillator : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;

    Quaternion startingRotation;

    void Start()
    {
        startingRotation = transform.rotation;
    }

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        float cycles = Time.time / period;  // number of cycles as decimal number, from starting the game
        const float tau = Mathf.PI * 2;  // tau is equal to 2x PI number, so just raw number 6.28(...)

        // Sine with values from -1 to 1, whole period takes 2 PI or 1 tau, converting number of cycles * tau
        // if cycles is in half (cycles = 0.5f) sin value would be 0 because 0.5 * 2PI gives 1 PI = half cycle
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = Mathf.Sin(cycles * period);  // converting sin values from range(-1,1) to range(0,1) by dividing value by two and adding 0.5f
        //transform.position = startingRotation + offset;
        transform.rotation = Quaternion.AngleAxis(angle * movementFactor, Vector3.forward);
    }
}
