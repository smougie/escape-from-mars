using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Missile : MonoBehaviour
{
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 1.5f;

    [SerializeField] AudioClip thrustSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip finishSound = null;

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
    float padLandingPositionY = 0f;

    private int maxFuel = 100;
    private int currentFuel;
    [SerializeField] Slider fuelSlider;

    void Start()
    {
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

    private void AssignPadPosition(Collision collision)
    {
        padLandingPositionY = collision.gameObject.transform.position.y;
        print(collision.gameObject.transform.position.y);
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
        float landingInheritAndSpeed = 2f;
        var currentRotation = transform.rotation;
        Vector3 rocketStartingPosition = transform.position;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        if (transform.rotation.z <= -.01f || transform.rotation.z >= .01f)
        {
            transform.Rotate(-Vector3.forward * transform.rotation.z * (levelLoadDelay * landingInheritAndSpeed));  // TODO bugged
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(rocketStartingPosition.x, padLandingPositionY + landingInheritAndSpeed, rocketStartingPosition.z), Time.deltaTime);
        }
        else
        {
            landing = false;
            rigidBody.constraints = RigidbodyConstraints.None;
        }
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

    private float fuelCounter = 0f;
    [SerializeField] float fuelUseSpeed = 1f;
    private void UseFuel()
    {
        fuelCounter += fuelUseSpeed * Time.deltaTime;
        print(fuelCounter);
    }

    private void UpdateFuelValue()
    {
        if (fuelCounter >= 1f)
        {
            fuelCounter -= 1f;
            currentFuel -= 1;
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
