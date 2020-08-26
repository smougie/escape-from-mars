using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMachineControler : MonoBehaviour
{
    [SerializeField] float delayStartTime;
    [SerializeField] float pauseTime;
    [SerializeField] GameObject edge;
    [SerializeField] GameObject[] boxes;

    void Start()
    {
        StartCoroutine(BoxCreationInterval());
    }

    private void CreateBox()
    {
        var randomNumber = Random.Range(0, 5);
        Instantiate(boxes[randomNumber], transform.position, Quaternion.identity);
        StartCoroutine(EdgeAnimation());
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
