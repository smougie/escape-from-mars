using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketCollectible : MonoBehaviour
{
    private GameManager gameManagerRef;
    private GameObject canvasObject;
    private LifePromptControler lifePromptControler;

    void Start()
    {
        canvasObject = GameObject.Find("Canvas").gameObject;
        lifePromptControler = canvasObject.GetComponent<LifePromptControler>();
        gameManagerRef = GameObject.Find("Game Manager").gameObject.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            if (gameManagerRef.CurrentlyMaxLife())
            {
                lifePromptControler.EnableLifePrompt();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            lifePromptControler.StartFadeOut();
        }
    }
}
    