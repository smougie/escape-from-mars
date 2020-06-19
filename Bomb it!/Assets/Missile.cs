using System;
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

    [SerializeField] ParticleSystem thrustParticles = null;
    [SerializeField] ParticleSystem deathParticles = null;
    [SerializeField] ParticleSystem finishParticles = null;

    [SerializeField] GameObject rocketPartsObject = null;
    [SerializeField] GameObject levelLandingPad = null;

    [SerializeField] Light[] objectLights = null;

    [SerializeField] bool destroyOnDeath = true;

    Rigidbody rigidBody;
    AudioSource audioSource;
    float startVolume;
    bool landing = false;
    bool collisionsEnabled = true;
    bool isTransitioning = false;
    Vector3 landingPosition;

    [SerializeField] float fuelUseSpeed = 1f;
    [SerializeField] Slider fuelSlider;
    [SerializeField] int maxFuel = 100;
    private int currentFuel;
    private float fuelCounter = 0f;
    private bool noFuel = false;

    private Quaternion startingRotation;

    void Start()
    {
        startingRotation = gameObject.transform.rotation;
        fuelSlider.maxValue = maxFuel;
        currentFuel = maxFuel;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
    }

    void Update()
    {
        if (!isTransitioning && !landing)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
        if (landing)
        {
            StartLandingSequence();
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || !collisionsEnabled)  // ignore collisions if not in Alive state
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
                // TODO put refuel method here
                Vector3 dupa = collision.transform.position;
                AssignPadPosition(collision);
                landing = true;
                thrustParticles.Stop();
                StopThrusting();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        landing = true;  // raise landing flag which is tracked inside Update(), if true start LandingSequence
        finishParticles.Play();  // play level finish particles
        thrustParticles.Stop();  // stop playing thrusting particles
        Invoke("LoadNextLevel", levelLoadDelay);  // load next level after delay
        ManageAudio(finishSound);  // control audio
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
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

    private void StartRefuelSequence()
    {
        // TODO start refuel sequence
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

    
    private void StartLandingSequence()
    {
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

    private void UseFuel()
    {
        if (currentFuel > 0)
        {
            fuelCounter += fuelUseSpeed * Time.deltaTime;
        }
        else if (currentFuel <= 0f)
        {
            noFuel = true;
            StartNoFuelSequence();
        }
    }

    private void StartNoFuelSequence()
    {
        thrustParticles.Stop();
        ManageAudio(emptyFuelSound);
    }

    private void UpdateFuelValue()
    {
        var decreaseValue = 1;
        if (fuelCounter >= decreaseValue)
        {
            fuelCounter -= decreaseValue;
            currentFuel -= decreaseValue;
        }
        fuelSlider.value = currentFuel;
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
