﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Missile : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 3f;

    [SerializeField] AudioClip thrustSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip finishSound = null;
    [SerializeField] AudioClip emptyFuelSound = null;
    [SerializeField] AudioClip refuelingSound = null;

    [SerializeField] ParticleSystem thrustParticles = null;
    [SerializeField] ParticleSystem deathParticles = null;
    [SerializeField] ParticleSystem finishParticles = null;

    [SerializeField] GameObject rocketPartsObject = null;
    [SerializeField] GameObject levelLandingPad = null;
    [SerializeField] GameObject refuelingEffect = null;

    [SerializeField] Light[] objectLights = null;

    [SerializeField] bool destroyOnDeath = true;

    Rigidbody rigidBody;
    AudioSource audioSource;
    float startVolume;
    bool landing = false;
    bool collisionsEnabled = true;
    Vector3 landingPosition;

    [SerializeField] float fuelUseSpeed = 1f;
    [SerializeField] float refuelingSpeed = 1f;
    [SerializeField] Slider fuelSlider;
    [SerializeField] int maxFuel = 100;
    private int currentFuel;
    private float fuelCounter = 0f;
    private bool noFuel = false;

    private Quaternion startingRotation;
    private bool refueling;
    private bool rocketOnRefuelingPad;
    GameObject activeRefuelingEffect;

    enum State { Flying, Refueling, Transistioning, Launching};
    State state;
    

    void Start()
    {
        startingRotation = gameObject.transform.rotation;
        fuelSlider.maxValue = maxFuel;
        currentFuel = maxFuel;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
        state = State.Launching;
    }

    void Update()
    {
        if (state != State.Transistioning && state != State.Launching && !landing && !rocketOnRefuelingPad)
        {
            RespondToRotateInput();
        }
        if (state != State.Transistioning && !landing)
        {
            RespondToThrustInput();
        }
        if (state == State.Launching)
        {
            StartFlyingSequence();
        }
        if (landing)
        {
            StartLandingSequence();
        }
        if (refueling)
        {
            RefuelingRocket();
        }
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
        UpdateFuelValue();
    }

    private void DebugKeys()
    {
        Collider[] objectsColliders = GetComponentsInChildren<Collider>();
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;  // simple toggle
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            currentFuel = maxFuel / 10;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == State.Transistioning || !collisionsEnabled)  // ignore collisions if not in Alive state
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                AssignPadPosition(collision);
                StartSuccessSequence();
                break;
            case "Refuel":
                AssignPadPosition(collision);
                StartRefuelSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (state != State.Refueling)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Refuel":
                rocketOnRefuelingPad = false;
                break;
            default:
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transistioning;
        landing = true;  // raise landing flag which is tracked inside Update(), if true start LandingSequence
        finishParticles.Play();  // play level finish particles
        thrustParticles.Stop();  // stop playing thrusting particles
        Invoke("LoadNextLevel", levelLoadDelay);  // load next level after delay
        ManageAudio(finishSound);  // control audio
    }

    private void StartDeathSequence()
    {
        state = State.Transistioning;
        deathParticles.Play();
        thrustParticles.Stop();
        Invoke("LoadFirstLevel", levelLoadDelay);
        ManageAudio(deathSound);
        if (!destroyOnDeath)
        {
            DisableLight();
        }
        if (destroyOnDeath)
        {
            DestroyObject();
        }
    }

    private void StartNoFuelSequence()
    {
        // TODO add state to control situtation when there is no fuel but player still live - for example blocked somewhere, start death sequence after X seconds
        thrustParticles.Stop();
        ManageAudio(emptyFuelSound);
    }

    private void StartRefuelSequence()
    {
        landing = true;
        refueling = true;
        rocketOnRefuelingPad = true;
        state = State.Refueling;
        StopThrusting();
        SpawnRefuelEffect();
    }

    int prefabCount = 0;
    private void SpawnRefuelEffect()
    {
        if (prefabCount == 0)
        {
            activeRefuelingEffect = Instantiate(refuelingEffect, new Vector3(0, transform.position.y - 1.8f, 0), Quaternion.Euler(0, 0, 0), transform);
            prefabCount++;
        }
    }

    IEnumerator DelayRefuelEffect()
    {
        yield return new WaitForSeconds(1f);
    }

    private void StopRefuelEffetct()
    {
        Destroy(activeRefuelingEffect);
        prefabCount--;
    }

    private void RefuelingRocket()
    {
        if (currentFuel < maxFuel)
        {
            fuelCounter += refuelingSpeed * Time.deltaTime; // increase fuelCounter
        }
        else
        {
            refueling = false;
            state = State.Flying;
            StartCoroutine(FadeOut(audioSource, 1f));
        }
    }

    private void UseFuel()
    {
        if (currentFuel > 0)
        {
            fuelCounter += fuelUseSpeed * Time.deltaTime;  // increase fuelCounter value while pressing Thrust button (around 0.03 to 0.1 per space hit)
        }
        else if (currentFuel <= 0f)
        {
            noFuel = true;
            StartNoFuelSequence();
        }
    }

    private void UpdateFuelValue()
    {
        var counterUpdateValue = 1;
        if (fuelCounter >= counterUpdateValue)  // if fuel counter hit value 1 (used 1 fuel unit) than decrase bar by unit and reset counter
        {
            fuelCounter -= counterUpdateValue;  // reset counter value from hitted 1 to 0

            switch (state)  // switch on state if flying -> decrase currentFuel, if refueling -> increase currentFuel
            {
                case State.Flying:
                    currentFuel -= counterUpdateValue;  // update fuel value
                    break;
                case State.Refueling:
                    currentFuel += counterUpdateValue;  // update fuel value
                    break;
                case State.Transistioning:
                    break;
                default:
                    break;
            }

        }
        fuelSlider.value = currentFuel;  // update bar value
    }

    // stop current audioSource (thrusting), play death/finish clip, fade out audio death/finish clip in same time as level transcend
    private void ManageAudio(AudioClip audioClip)
    {
        StopAllCoroutines();  // stop FadeOut coroutine which can fadeout finish/death audio clip
        audioSource.volume = startVolume;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(FadeOut(audioSource, levelLoadDelay));
    }

    private void PlayRefuelSound()
    {
        StopAllCoroutines();  // stop FadeOut coroutine which can fadeout finish/death audio clip
        audioSource.volume = startVolume;
        audioSource.Stop();
        audioSource.PlayOneShot(refuelingSound);
    }

    private void StartLandingSequence()
    {
        var startTime = Time.time;

        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        if (transform.rotation.z >= .01f || transform.rotation.z <= -.01f)
        {
            Landing();
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            landing = false;
            rigidBody.constraints = RigidbodyConstraints.None;
        }
        var endTime = Time.time;
        var executionTime = endTime - startTime;
        print("Execution time: " + executionTime);
    }

    private void Landing()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), .02f);
        transform.position = Vector3.Lerp(transform.position, landingPosition, .02f);
    }

    private void AssignPadPosition(Collision collision)
    {
        Vector3 padPosition = collision.gameObject.transform.position;
        // TODO decide which is better: landing in the center or landing in rocket position
        // code below for landing in the center of pad
        padPosition.y = padPosition.y + 2;
        landingPosition = padPosition;
        //landingPosition = transform.position;
        //landingPosition.y = padPosition.y + 2;
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = sceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)  // if sceneIndex is equal to last scene, load first level (loop)
        {
            LoadFirstLevel();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }

    }

    // Rotate object, reacting only to one statement at time
    private void RespondToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;  // rotate speed
        rigidBody.angularVelocity = Vector3.zero;  // remove rotation due to physics
  
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    // Accelerate object, play audio sound of thrusting while pressing button
    private void RespondToThrustInput()
    {
        if (!noFuel)  // if fuel is not empty
        {
            if (Input.GetKey(KeyCode.Space))  // can adjust speed while rotating
            {
                ApplyThrust();
                UseFuel();
            }
            else if (Input.GetKeyUp(KeyCode.Space))  // stop playing audio by fading out audio sound
            {
                StopThrusting();
            }
        }
    }

    private void StopThrusting()
    {
        StartCoroutine(FadeOut(audioSource, .5f));
        thrustParticles.Stop();
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSound);
        }
        if (!thrustParticles.isPlaying)
        {
            thrustParticles.Play();
        }
    }

    private void StartFlyingSequence()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = State.Flying;
        }
    }

    private void DisableLight()
    {
        foreach (var item in objectLights)
        {
            item.enabled = false;
        }
    }

    private void DestroyObject()
    {
        Vector3 deathPosition = transform.position;
        Quaternion deathRotation = transform.rotation;
        Instantiate(rocketPartsObject, deathPosition, deathRotation);
        DisableMesh();
    }

    private void DisableMesh()
    {
        GameObject[] objectMeshes = GameObject.FindGameObjectsWithTag("RocketPart");
        foreach (var item in objectMeshes)
        {
            item.GetComponent<Renderer>().enabled = false;
        }
    }

    // Fade out audioSource to avoid clip after .Stop()
    IEnumerator FadeOut(AudioSource audioSource, float fadingTime)
    {
        float fadeTime = fadingTime;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
