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
    [SerializeField] bool normalMode, reverseMode, randomMode;
    [SerializeField] GameObject[] stopPoints;
    [SerializeField] float moveSpeed;
    [SerializeField] bool applySlow;
    private float slowFactor = 1f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private List<Vector3> dropPoints = new List<Vector3>();
    private int index = 0;
    private bool createBoxCalled = false;
    private bool moving = false;

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        if (moveMachine)
        {
            StartCoroutine(DelayMovingMachine());
        }
        else
        {
            StartCoroutine(BoxCreationInterval());
        }
    }

    void Update()
    {
        if (moving)
        {
            CheckMachinePosition();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(dropPoints[index].x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime * slowFactor);
        }
        if (createBoxCalled)
        {
            createBoxCalled = false;
            CreateBox();
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
            ChangeTargetPoint();
        }
    }

    private void ChangeTargetPoint()
    {
        moving = false;
        if (normalMode)
        {
            IncreaseIndex();
        }
        if (randomMode)
        {
            RandomIndex();
        }
        if (reverseMode)
        {
            ReverseIndex();
        }
    }

    private void IncreaseIndex()
    {
        if (index < dropPoints.Count - 1)
        {
            StartCoroutine(BoxCreationWhileMoving(index + 1));
            slowFactor = 1f;  // reset slow factor to 1f = speed is normal
        }
        else
        {
            StartCoroutine(BoxCreationWhileMoving(0));
            slowFactor = .25f;  // change slow factor to 10% speed while index is equal to 0 = when moving back
        }
    }

    private void RandomIndex()
    {
        int previousIndexValue = index;
        while (index == previousIndexValue)
        {
            index = Random.Range(0, dropPoints.Count);
        }
        StartCoroutine(BoxCreationWhileMoving(index));
    }

    private void ReverseIndex()
    {
        if (index < dropPoints.Count - 1)
        {
            StartCoroutine(BoxCreationWhileMoving(index + 1));
        }
        else
        {
            dropPoints.Reverse();
            StartCoroutine(BoxCreationWhileMoving(1));
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

    private void ManageMode()
    {
        normalMode = true;  
        if (randomMode)
        {
            reverseMode = false;
            normalMode = false;
        }
        if (reverseMode)
        {
            randomMode = false;
            normalMode = false;
        }
        if (normalMode)
        {
            randomMode = false;
            reverseMode = false;
        }
    }

    IEnumerator BoxCreationWhileMoving(int indexNum)
    {
        createBoxCalled = true;
        yield return new WaitForSeconds(1.2f + pauseTime);  // animation wait time
        index = indexNum;
        moving = true;
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

    IEnumerator DelayMovingMachine()
    {
        yield return new WaitForSeconds(delayStartTime);
        ManageMode();
        startPosition = transform.position;
        CreateDropPointsList();
        moving = true;
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
