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
    [SerializeField] AudioClip refuelingSound = null;

    [SerializeField] ParticleSystem thrustParticles = null;
    [SerializeField] ParticleSystem deathParticles = null;
    [SerializeField] ParticleSystem finishParticles = null;

    [SerializeField] GameObject rocketPartsObject = null;
    [SerializeField] GameObject refuelingEffect = null;
    [SerializeField] GameObject launchPadObject = null;
    private PadController padControllerRef;

    [SerializeField] bool refuelingOnLevel = false;
    [SerializeField] GameObject refuelingPad = null;
    private StatusLight statusLight;
    private Light refuelingPadLight;

    Rigidbody rigidBody;
    AudioSource audioSource;
    float startVolume;
    bool landing = false;
    bool collisionsEnabled = true;
    Vector3 landingPosition;

    [SerializeField] float fuelUseSpeed = 1f;
    [SerializeField] Slider fuelSlider;
    [SerializeField] int maxFuel = 100;
    private float refuelingSpeed;
    private int currentFuel;
    private float fuelCounter = 0f;
    private bool noFuel = false;
    private bool alreadyRefueled = false;

    private Quaternion startingRotation;
    private bool refueling;
    private bool rocketOnRefuelingPad;
    int prefabCount = 0;
    GameObject activeRefuelingEffect;

    enum State { Flying, Refueling, Transistioning, Launching, Landing};
    State state;
    private bool destroyingRefuelEffect = false;

    private GameObject gameManagerObject;
    private GameManager gameManager;

    private Vector3 rocketStartingPosition;

    void Start()
    {
        startingRotation = gameObject.transform.rotation;
        fuelSlider.maxValue = maxFuel;
        SetFuelStartingValue();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
        state = State.Launching;
        if (refuelingOnLevel)
        {
            statusLight = refuelingPad.GetComponentInChildren<StatusLight>();
            refuelingPadLight = refuelingPad.GetComponentInChildren<Light>();
        }
        gameManagerObject = GameObject.Find("Game Manager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        rocketStartingPosition = transform.position;
        padControllerRef = launchPadObject.GetComponent<PadController>();
    }

    void Update()
    {
        if (state != State.Transistioning && state != State.Launching && state != State.Refueling && !landing && !rocketOnRefuelingPad)
        {
            RespondToRotateInput();
        }
        if (state != State.Transistioning && state != State.Refueling && !landing)
        {
            RespondToThrustInput();
        }
        if (state == State.Launching)
        {
            StartFlyingSequence();
        }
        if (landing)
        {
            Landing();
        }
        if (refueling && !landing)
        {
            RefuelingRocket();
            ChangeRefuelSpeed();
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
            RespawnRocket();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == State.Transistioning || !collisionsEnabled || rocketOnRefuelingPad)  // ignore collisions if not in Alive state
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
                if (alreadyRefueled && noFuel)  // if player already refuel his rocket once (so refuel pad is not active) and rocket has no fuel threat refueling pad as normal obstacle
                {
                    StartDeathSequence();
                }
                else  // in other case use refueling pad normally
                {
                    AssignPadPosition(collision);
                    StartRefuelSequence();
                }
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            // TODO check if those statements are still necessary if we already force player refuel to max at once
            case "Refuel":
                rocketOnRefuelingPad = false;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        switch (trigger.transform.tag)
        {
            case "Life":
                if (gameManager.CanCollectLife())
                {
                    DestroyCollectible(trigger);
                    gameManager.IncreaseLife();
                }
                break;
            case "Collectible":
                DestroyCollectible(trigger);
                gameManager.IncreaseCollectiblesCount();
                break;
            default:
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transistioning;
        StartLandingSequence();
        finishParticles.Play();  // play level finish particles
        thrustParticles.Stop();  // stop playing thrusting particles
        ManageAudio(finishSound);  // control audio
        Invoke("LoadNextLevel", levelLoadDelay);  // load next level after delay
    }

    // TODO this method is previous StartDeathSequence
    private void StartGameOverSequence()
    {
        state = State.Transistioning;
        deathParticles.Play();
        thrustParticles.Stop();
        Invoke("LoadFirstLevel", levelLoadDelay);
        ManageAudio(deathSound);
        DestroyRocket();
    }

    private void StartDeathSequence()
    {
        state = State.Transistioning;
        deathParticles.Play();
        thrustParticles.Stop();
        ManageAudio(deathSound);
        DestroyRocket();
        gameManager.DecreaseLife();
        Invoke("RespawnRocket", levelLoadDelay);
        gameManager.CalculateLevelScore();
    }

    private void RespawnRocket()
    {
        FreezeRigidbody(true);  // to avoid situation when rocket collides with pad and forces push rocket in weird positions
        padControllerRef.PadActive(true);  // respawn launch pad
        transform.position = rocketStartingPosition;  // move rocket object to starting position
        transform.rotation = Quaternion.Euler(0, 0, 0);  // set rotation to 0
        SetFuelStartingValue();
        state = State.Launching;  // change game state
        RenderMesh(true);  // render rocket object (stopped rendering mesh of rocket object in DestroyRocket() method)
        ClearRocketParts();  // Destroy Rocket Parts Object
        FreezeRigidbody(false);  // unfreeze rocket
    }

    private void FreezeRigidbody(bool freeze)
    {
        if (freeze)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints.None;
        }
    }

    private void ClearRocketParts()
    {
        var rocketParts = GameObject.Find("Rocket Ship Parts(Clone)");
        Destroy(rocketParts);
    }

    private void SetFuelStartingValue()
    {
        currentFuel = maxFuel;
    }

    private void StartNoFuelSequence()
    {
        // TODO add state to control situtation when there is no fuel but player still live - for example blocked somewhere, start death sequence after X seconds
        thrustParticles.Stop();
        ManageAudio(emptyFuelSound);
    }

    private void StartRefuelSequence()
    {
        if (!alreadyRefueled)  // check if player already refuel rocket
        {
            state = State.Refueling;
            refueling = true;  // raise refueling flag,
        }
        StartLandingSequence();  // start auto landing sequence
    }

    private void SpawnRefuelEffect()
    {
        if (prefabCount == 0)  // check is refueling effect already spawneds
        {
            activeRefuelingEffect = Instantiate(refuelingEffect, transform);  // spawn refueling effect as a child of rocket
            prefabCount++;  // increase counter
        }
    }

    private void StopRefuelEffect()
    {
        prefabCount--;  // decrease prefab count
        if (!destroyingRefuelEffect)
        {
            StartCoroutine(DestroyRefuelEffect());  // genlty stop playing refueling effect, than destroy it
        }
        else
        {
            return;
        }
    }

    IEnumerator DestroyRefuelEffect()
    {
        destroyingRefuelEffect = true;
        var destroyAfter = 1f;
        ParticleSystem currentRefuelEffect = activeRefuelingEffect.GetComponentInChildren<ParticleSystem>();
        AudioSource currentRefuelSound = activeRefuelingEffect.GetComponentInChildren<AudioSource>();
        currentRefuelEffect.Stop();
        currentRefuelSound.Stop();
        yield return new WaitForSeconds(destroyAfter);

        Destroy(activeRefuelingEffect);
        destroyingRefuelEffect = false;
    }


    private void RefuelingRocket()
    {
        if (currentFuel < maxFuel)  // check if current fuel is less than max fuel
        {
            fuelCounter += refuelingSpeed * Time.deltaTime; // increase fuelCounter, fuelCounter is a counter which track current state of fuel
        }
        else  // if max fuel
        {
            alreadyRefueled = true;
            refueling = false;  // drop refueling pad
            state = State.Flying;  //  change state from refueling to flying
            StopRefuelEffect();  // stop refueling effect
            statusLight.TurnOff();  // change color of refueling pad status light to red (not Active refueling pad)
            refuelingPadLight.gameObject.SetActive(false);  // turn off refueling pad spot light
            noFuel = false;
        }
    }

    private void UseFuel()
    {
        if (currentFuel > 0)
        {
            fuelCounter += fuelUseSpeed * Time.deltaTime;  // increase fuelCounter value while pressing Thrust button (around 0.03 to 0.1 per space hit)
        }
        else if (currentFuel <= 0)
        {
            noFuel = true;
            StartNoFuelSequence();
        }
    }

    private void UpdateFuelValue()
    {
        var counterUpdateValue = 1;  // value responsible for Fuel Bar update rate
        if (fuelCounter >= counterUpdateValue)  // if fuel counter hit value 1 (used 1 fuel unit) than decrase bar by unit and reset counter
        {
            fuelCounter -= counterUpdateValue;  // reset counter value from hitted 1 to 0

            switch (state)  // switch on state if flying -> decrase currentFuel, if refueling -> increase currentFuel
            {
                case State.Flying:
                    currentFuel -= counterUpdateValue;  // update fuel value by decreasing currentFuel value
                    break;
                case State.Refueling:
                    currentFuel += counterUpdateValue;  // update fuel value by increasing currentFUel value
                    break;
                case State.Transistioning:
                    break;
                default:
                    break;
            }

        }
        fuelSlider.value = currentFuel;  // update bar value
    }

    private void ChangeRefuelSpeed()
    {
        if (currentFuel >= 90)
        {
            refuelingSpeed = 10f;
        }
        else if (currentFuel >= 50 && currentFuel < 90)
        {
            refuelingSpeed = 25f;
        }
        else if (currentFuel >= 30 && currentFuel < 50)
        {
            refuelingSpeed = 40f;
        }
        else
        {
            refuelingSpeed = 55f;
        }
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
        StopThrusting(); // stop thrusting when player hit pad
        FreezeRigidbody(true);
        landing = true;
    }

    private void Landing()
    {
        float landingSpeedFactor = .03f;
        if (transform.rotation.z >= .01f || transform.rotation.z <= -.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), landingSpeedFactor);
            transform.position = Vector3.Lerp(transform.position, landingPosition, landingSpeedFactor);
        }
        else
        {
            StopLandingSequence();
        }
    }

    private void StopLandingSequence()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);  // finish rotating by set zero values (after previous landing rotation a small nubers near zero was left)
        FreezeRigidbody(false);  // unfreeze rocket object
        landing = false;  // drop landing flag to cancel rotation
        rocketOnRefuelingPad = true;
        if (state == State.Refueling)
        {
            SpawnRefuelEffect();
        }
        if (state == State.Transistioning)
        {
            // do nothin -> leave state == State.Transistioning
        }
        else
        {
            // TODO make some notification about already used refueling pad
            state = State.Flying;
        }
    }

    private void AssignPadPosition(Collision collision)
    {
        Vector3 padPosition = collision.gameObject.transform.position;
        padPosition.y = padPosition.y + 2;
        landingPosition = padPosition;
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

    private void DestroyRocket()
    {
        Vector3 deathPosition = transform.position;
        Quaternion deathRotation = transform.rotation;
        Instantiate(rocketPartsObject, deathPosition, deathRotation);
        RenderMesh(false);
    }

    private void RenderMesh(bool renderMesh)
    {
        GameObject[] objectMeshes = GameObject.FindGameObjectsWithTag("RocketPart");
        foreach (var item in objectMeshes)
        {
            switch (renderMesh)
            {
                case true:
                    item.GetComponent<Renderer>().enabled = true;
                    break;
                case false:
                    item.GetComponent<Renderer>().enabled = false;
                    break;
                default:
                    break;
            }
        }
    }

    private void DestroyCollectible(Collider collision)
    {
        Destroy(collision.gameObject);
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
