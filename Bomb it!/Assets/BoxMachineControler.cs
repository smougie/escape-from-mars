using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMachineControler : MonoBehaviour
{
    [SerializeField] float delayStartTime;
    [SerializeField] float pauseTime;
    [SerializeField] GameObject edge;
    [SerializeField] GameObject[] boxes;

    [SerializeField] bool moveMachine;
    [SerializeField] GameObject[] stopPoints;
    [SerializeField] float moveSpeed;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private List<Vector3> dropPoints = new List<Vector3>();
    private int index = 0;

    void Start()
    {
        if (moveMachine)
        {
            startPosition = transform.position;
            CreateDropPointsList();
        }
        else
        {
            StartCoroutine(BoxCreationInterval());
        }
    }

    void Update()
    {
        if (moveMachine)
        {
            CheckMachinePosition();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(dropPoints[index].x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }

    private void CreateBox()
    {
        var randomNumber = Random.Range(0, 5);
        Instantiate(boxes[randomNumber], transform.position, Quaternion.identity);
        StartCoroutine(EdgeAnimation());
    }

    private void CheckMachinePosition()
    {
        if (transform.position.x == dropPoints[index].x)
        {
            if (index < dropPoints.Count - 1)
            {
                StartCoroutine(BoxCreationWhileMoving(index+1));
                print("increasing index value");
            }
            else
            {
                print("reseting index value");
                StartCoroutine(BoxCreationWhileMoving(0));
            }
        }
    }

    private void CreateDropPointsList()
    {
        dropPoints.Add(startPosition);
        if (stopPoints.Length > 0)
        {
            foreach (var stopPoint in stopPoints)
            {
                dropPoints.Add(stopPoint.transform.position);
            }
        }
    }

    IEnumerator BoxCreationWhileMoving(int indexNum)
    {
        yield return new WaitForSeconds(1.5f);
        index = indexNum;
    }

    IEnumerator BoxCreationInterval()
    {
        yield return new WaitForSeconds(delayStartTime);
        while (true)
        {
            CreateBox();
            yield return new WaitForSeconds(pauseTime);
        }
    }

    IEnumerator EdgeAnimation()
    {
        var startTime = Time.time;
        while (edge.transform.localPosition.z < .5f)
        {
            edge.transform.localPosition = Vector3.MoveTowards(edge.transform.localPosition, new Vector3(edge.transform.localPosition.x, edge.transform.localPosition.y, .5f), Time.deltaTime);
            yield return null;
        }
        while (edge.transform.localPosition.z > 0)
        {
            edge.transform.localPosition = Vector3.MoveTowards(edge.transform.localPosition, new Vector3(edge.transform.localPosition.x, edge.transform.localPosition.y, 0), Time.deltaTime * 5f);
            yield return null;
        }
        var endTime = Time.time;
    }
}
