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

    [SerializeField] AudioSource audioSource1;  // thrust, refuel, finish, death, nofuel
    [SerializeField] AudioSource audioSource2;  // pick up
    [SerializeField] AudioClip thrustSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip finishSound = null;
    [SerializeField] AudioClip emptyFuelSound = null;
    [SerializeField] AudioClip refuelingSound = null;
    [SerializeField] AudioClip lifePickUpSound = null;
    [SerializeField] AudioClip alienPickUpSound = null;

    [SerializeField] ParticleSystem thrustParticles = null;
    [SerializeField] ParticleSystem RCSParticlesLeft = null;
    [SerializeField] ParticleSystem RCSParticlesRight = null;
    [SerializeField] ParticleSystem deathParticles = null;
    [SerializeField] ParticleSystem finishParticles = null;

    [SerializeField] GameObject rocketPartsObject = null;
    [SerializeField] GameObject refuelingEffect = null;
    [SerializeField] GameObject launchPadObject = null;
    private PadController padControllerRef;

    [SerializeField] bool refuelingOnLevel = false;
    [SerializeField] GameObject refuelingPad = null;
    [SerializeField] bool checkPoint = false;
    private bool checkPointReached = false;
    private StatusLight statusLight;
    private Light refuelingPadLight;

    private PauseMenu pauseMenuRef;
    private EndLevel endLevelRef;
    private GameObject canvasObject;

    Rigidbody rigidBody;
    float startVolume;
    bool landing = false;
    bool collisionsEnabled = true;
    Vector3 landingPosition;

    [SerializeField] float fuelUseSpeed = 1f;
    [SerializeField] int maxFuel = 100;
    private Slider fuelSlider;
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

    private CheckPointFlag checkPointFlag;

    private Vector3 rocketStartingPosition;
    private Vector3 checkPointPosition;

    private bool cameraTriggered;
    private Vector3 cameraTriggerRocketPosition;
    private Vector3 cameraTriggerLaunchPadPosition;

    void Start()
    {   
        UpdateSoundEffectsVolume();
        fuelSlider = GameObject.Find("Fuel Bar").GetComponent<Slider>();
        startingRotation = gameObject.transform.rotation;
        fuelSlider.maxValue = maxFuel;
        SetFuelStartingValue();
        rigidBody = GetComponent<Rigidbody>();
        audioSource1 = GetComponent<AudioSource>();
        startVolume = audioSource1.volume;

        state = State.Launching;
        if (refuelingOnLevel)
        {
            statusLight = refuelingPad.GetComponentInChildren<StatusLight>();
            refuelingPadLight = refuelingPad.GetComponentInChildren<Light>();
        }
        if (checkPoint)
        {
            checkPointFlag = refuelingPad.GetComponentInChildren<CheckPointFlag>();
        }

        gameManagerObject = GameObject.Find("Game Manager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        canvasObject = GameObject.Find("Canvas");
        pauseMenuRef = canvasObject.GetComponent<PauseMenu>();
        endLevelRef = canvasObject.GetComponent<EndLevel>();
        rocketStartingPosition = transform.position;
        padControllerRef = launchPadObject.GetComponent<PadController>();


        UpdateSoundEffectsVolume();  // Update sound levels
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
        CheckSoundChanges();
    }

    private void DebugKeys()
    {
        Collider[] objectsColliders = GetComponentsInChildren<Collider>();
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartSuccessSequence();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            RespawnRocket();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;  // simple toggle
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            currentFuel = 10;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            print("Clearing Highscores...");
            PlayerPrefs.DeleteKey("Highscores");
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
                    AssignCheckPointPosition();
                    VerifyCheckPointStatus();
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
        if (state == State.Transistioning)
        {
            return;
        }

        switch (trigger.transform.tag)
        {
            case "Life":
                if (gameManager.CanCollectLife())
                {
                    DestroyCollectible(trigger);
                    gameManager.IncreaseLife();
                    audioSource2.PlayOneShot(lifePickUpSound);

                }
                break;
            case "Collectible":
                DestroyCollectible(trigger);
                gameManager.IncreaseCollectiblesCount();
                audioSource2.PlayOneShot(alienPickUpSound);
                break;
            case "Camera Trigger":
                cameraTriggered = true;
                cameraTriggerRocketPosition = trigger.transform.position;
                cameraTriggerRocketPosition.y = cameraTriggerRocketPosition.y + 2;
                cameraTriggerLaunchPadPosition = trigger.transform.position;
                // TODO assign vector3 of respawn point rocket/launch pad here
                // camera trigger object position -> launch pad position and y + 2 rocket position
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transistioning;
        StartLandingSequence();
        finishParticles.Play();  // play level finish particles
        thrustParticles.Stop();  // stop playing thrusting particles
        RCSParticlesStop();
        ManageAudio(finishSound);  // control audio
        gameManager.CalculateLevelScore();  // calculate level score without saving them
        StartCoroutine(EndLevelWindowDelay(false));  // show endLevelWindow with delay
    }

    // TODO this method is previous StartDeathSequence
    private void StartGameOverSequence()
    {
        state = State.Transistioning;
        deathParticles.Play();
        thrustParticles.Stop();
        RCSParticlesStop();
        ManageAudio(deathSound);
        StartCoroutine(EndLevelWindowDelay(true));  // show endLevelWindow with delay
        DestroyRocket();
        Invoke("ClearRocketParts", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        gameManager.DecreaseLife();
        if (gameManager.Alive())
        {
            state = State.Transistioning;
            deathParticles.Play();
            thrustParticles.Stop();
            RCSParticlesStop();
            ManageAudio(deathSound);
            DestroyRocket();
            Invoke("RespawnRocket", levelLoadDelay);
        }
        else
        {
            StartGameOverSequence();
        }
        
    }

    private void RespawnRocket()
    {
        FreezeRigidbody(true);  // to avoid situation when rocket collides with pad and forces push rocket in weird positions
        if (checkPointReached)
        {
            transform.position = checkPointPosition;
        }
        else if (cameraTriggered)
        {
            // TODO if camera trigger -> save vector3 position of camera trigger and assign respawn position for rocket and launch pad, than close floodgate
            transform.position = cameraTriggerRocketPosition;
            padControllerRef.gameObject.transform.position = cameraTriggerLaunchPadPosition;
            padControllerRef.PadActive(true);

        }
        else
        {
        padControllerRef.PadActive(true);  // respawn launch pad
        transform.position = rocketStartingPosition;  // move rocket object to starting position
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);  // set rotation to 0
        SetFuelStartingValue();
        state = State.Launching;  // change game state
        RenderMesh(true);  // render rocket object (stopped rendering mesh of rocket object in DestroyRocket() method)
        ClearRocketParts();  // Destroy Rocket Parts Object
        if (noFuel)
        {
            noFuel = false;
        }
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

    private void FreezeRocketParts()
    {
        var rocketParts = GameObject.Find("Rocket Ship Parts(Clone)");
        foreach (Transform childObject in rocketParts.transform)
        {
            childObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
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
        VerifyCheckPointStatus();
        AssignCheckPointPosition();
        if (!alreadyRefueled)  // check if player already refuel rocket
        {
            state = State.Refueling;
            refueling = true;  // raise refueling flag,
        }
        StartLandingSequence();  // start auto landing sequence
    }

    private void VerifyCheckPointStatus()
    {
        if (checkPoint && !checkPointReached)
        {
            checkPointReached = true;
            checkPointFlag.RaiseFlag();
        }
    }

    private void AssignCheckPointPosition()
    {
        checkPointPosition = landingPosition;
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
            // TODO access status light while colliding with pad
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
        audioSource1.volume = startVolume;
        audioSource1.Stop();
        audioSource1.PlayOneShot(audioClip);
        StartCoroutine(FadeOut(audioSource1, levelLoadDelay));
    }

    public void UpdateSoundEffectsVolume()
    {
        startVolume = SoundEffectsVolume();
        audioSource2.volume = SoundEffectsVolume();
    }

    private float SoundEffectsVolume()
    {
        return PlayerPrefs.GetFloat(OptionsValues.soundEffectsVolumeStr);
    }

    private void CheckSoundChanges()
    {   
        if (pauseMenuRef.updateSoundValues)
        {
            UpdateSoundEffectsVolume();
            pauseMenuRef.updateSoundValues = false;
        }
    }

    private void PlayRefuelSound()
    {
        StopAllCoroutines();  // stop FadeOut coroutine which can fadeout finish/death audio clip
        audioSource1.volume = startVolume;
        audioSource1.Stop();
        audioSource1.PlayOneShot(refuelingSound);
    }

    private void StartLandingSequence()
    {
        StopThrusting(); // stop thrusting when player hit pad
        RCSParticlesStop();
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
            state = State.Refueling;
            SpawnRefuelEffect();
        }
        else if (state == State.Transistioning)
        {
            // do nothing -> leave state == State.Transistioning
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

    public void RepeatLevel()
    {
        gameManager.ResetLevelValues();
        gameManager.ResetScoresValues();
        endLevelRef.DisableEndLevelWindow();
        pauseMenuRef.DestroyLeftObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Rotate object, reacting only to one statement at time
    private void RespondToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;  // rotate speed
        rigidBody.angularVelocity = Vector3.zero;  // remove rotation due to physics
  
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
            RCSParticlesRight.Play();
            RCSParticlesLeft.Stop();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            RCSParticlesLeft.Play();
            RCSParticlesRight.Stop();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            RCSParticlesRight.Stop();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            RCSParticlesLeft.Stop();
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
        StartCoroutine(FadeOut(audioSource1, .5f));
        thrustParticles.Stop();
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource1.isPlaying)
        {
            audioSource1.PlayOneShot(thrustSound);
        }
        if (!thrustParticles.isPlaying)
        {
            thrustParticles.Play();
        }
    }

    private void RCSParticlesStop()
    {
        RCSParticlesLeft.Stop();
        RCSParticlesRight.Stop();
    }

    private void StartFlyingSequence()
    {
        if (Input.GetKey(KeyCode.Space))
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

    public void PauseAudio(bool pause)
    {
        if (pause)
        {
            audioSource1.enabled = false;
            audioSource2.enabled = false;
        }
        else
        {
            audioSource1.enabled = true;
            audioSource2.enabled = true;
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

    IEnumerator EndLevelWindowDelay(bool gameOver)
    {
        yield return new WaitForSeconds(levelLoadDelay);
        endLevelRef.ShowEndLevelWindow(gameOver);
    }
}
