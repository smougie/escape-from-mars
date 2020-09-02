using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Vector3 newCameraPosition;
    [SerializeField] GameObject cameraObj;
    [SerializeField] float cameraMovementSpeed;
    private Missile missileRef;
    private MeshRenderer objectRenderer;

    [SerializeField] bool closeGateOnEnter;
    [SerializeField] bool closeGateOnExit;
    [SerializeField] GameObject gateObject;
    [SerializeField] float gateScaleTarget;
    [SerializeField] float scaleFactor = 1f;

    private void Start()
    {        
        missileRef = GameObject.Find("Rocket Ship").GetComponent<Missile>();
        objectRenderer = GetComponent<MeshRenderer>();
        objectRenderer.enabled = false;  // hide object when game start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rocket")
        {
            MoveCameraToNextPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rocket") && closeGateOnExit)
        {
            StartCoroutine(ClosingGateOnTriggerExit());
        }
    }

    private void MoveCameraToNextPoint()
    {
        StartCoroutine(MovingCamera());
        if (closeGateOnEnter)
        {
            StartCoroutine(ClosingGateOnTriggerEnter());
        }
    }

    IEnumerator MovingCamera()
    {
        Time.timeScale = 0f;
        missileRef.PauseAudio(true);
        while (cameraObj.transform.position.y <= newCameraPosition.y)
        {
            cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y + cameraMovementSpeed, cameraObj.transform.position.z);
            yield return null;
        }
        missileRef.PauseAudio(false);
        Time.timeScale = 1f;
    }

    IEnumerator ClosingGateOnTriggerEnter()
    {
        while (gateObject.transform.localScale.x < gateScaleTarget)
        {
            gateObject.transform.localScale += new Vector3(Time.fixedDeltaTime * scaleFactor, 0, 0);
            yield return null;
        }
    }

    IEnumerator ClosingGateOnTriggerExit()
    {
        while (gateObject.transform.localScale.x < gateScaleTarget)
        {
            gateObject.transform.localScale += new Vector3(Time.deltaTime * scaleFactor, 0, 0);
            yield return null;
        }
    }
}
