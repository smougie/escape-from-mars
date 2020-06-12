using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Missile : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 1.5f;

    [SerializeField] AudioClip thrustSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip finishSound = null;

    [SerializeField] ParticleSystem thrustParticles = null;
    [SerializeField] ParticleSystem deathParticles = null;
    [SerializeField] ParticleSystem finishParticles = null;

    [SerializeField] Light[] objectLights = null;

    [SerializeField] GameObject rocketPartsObject = null;

    [SerializeField] bool destroyOnDeath = true;

    enum State { Alive, Transcending, Dying};
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)  // ignore collisions if not in Alive state
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                // destroy player gameObject
                break;
        }
    }

    private void StartSuccessSequence()
    {
        // TODO freeze ship position to avoid falling down from landing platform
        state = State.Transcending;
        finishParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
        thrustParticles.Stop();
        ManageAudio(finishSound);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
        thrustParticles.Stop();
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

    // stop current audioSource (thrusting), play death/finish clip, fade out audio death/finish clip in same time as level transcend
    private void ManageAudio(AudioClip audioClip)
    {
        StopAllCoroutines();  // stop FadeOut coroutine which can fadeout finish/death audio clip
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(FadeOut(audioSource, levelLoadDelay));
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);  // TODO allow for more that just 2 levels
    }

    // Rotate object, reacting only to one statement at time
    private void RespondToRotateInput()
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
    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))  // can adjust speed while rotating
        {
            ApplyThrust(thrustThisFrame);
        }
        else if (Input.GetKeyUp(KeyCode.Space))  // stop playing audio by fading out audio sound
        {
            StartCoroutine(FadeOut(audioSource, .5f));
            thrustParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
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

    // TODO still need??? Make destroyOnDeath in inspector if destroyOnDeath than dont disable light
    private void DisableLight()
    {
        foreach (var item in objectLights)
        {
            item.enabled = false;
        }
    }

    // TODO working on this system, disable light function still necessary???
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
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
