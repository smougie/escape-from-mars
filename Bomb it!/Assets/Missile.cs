using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 200f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Rotate();
        Thrust();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // remove print lines
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Fuel":
                print("Fuel");
                break;
            default:
                print("Dead");  
                // destroy player gameObject
                break;
        }
    }

    // Rotate object, reacting only to one statement at time
    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;  // rotate speed
        rigidBody.freezeRotation = true;  // take manual control of rotation

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;  // resume physics control of rotation
    }

    // Accelerate object, play audio sound of thrusting while pressing button
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))  // can adjust speed while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up *  mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))  // fade out audio sound
        {
            StartCoroutine(FadeOut(audioSource));
        }
    }

    // Fade out audioSource to avoid clip after .Stop()
    IEnumerator FadeOut(AudioSource audioSource)
    {
        float FadeTime = .5f;
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;

    }
}
