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

    private void MoveCameraToNextPoint()
    {
        StartCoroutine(MovingCamera());
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
}
