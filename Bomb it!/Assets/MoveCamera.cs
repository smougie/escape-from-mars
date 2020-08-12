using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Vector3[] cameraPositions;
    [SerializeField] GameObject cameraObj;
    private int currentCameraIndex = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
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
        var startTime = Time.time;
        print("Start:" + startTime);
        while (cameraObj.transform.position.y <= cameraPositions[currentCameraIndex].y)
        {
            cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y + .15f, cameraObj.transform.position.z);
            //cameraObj.transform.position = Vector3.MoveTowards(cameraObj.transform.position, cameraPositions[currentCameraIndex], Time.unscaledDeltaTime * 50);
            yield return null;
        }
        Time.timeScale = 1f;
        print("Done in: " + (Time.unscaledTime - startTime));
        //currentCameraIndex++;
    }
}
