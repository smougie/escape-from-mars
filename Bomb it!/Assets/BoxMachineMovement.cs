using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMachineMovement : MonoBehaviour
{
    [SerializeField] GameObject[] stopPoints;
    [SerializeField] float moveSpeed;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private List<Vector3> dropPoints = new List<Vector3>();
    private int index = 0;

    void Start()
    {
        startPosition = transform.position;
        new WaitForSeconds(.6f);
        CreateDropPointsList();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(dropPoints[index].x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        CheckMachinePosition();
    }

    private void CheckMachinePosition()
    {
        if (transform.position.x == dropPoints[index].x)
        {
            new WaitForSeconds(.6f);
            if (index < dropPoints.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
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
}
