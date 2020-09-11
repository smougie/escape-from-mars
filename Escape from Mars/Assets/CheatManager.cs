using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private string[] cheatStrings = { "playground"};
    private string strToCheck = "";
    private int inputIndex = 0;
    //private int cheatIndex = 0;
    private Event currentEvent;
    [SerializeField] GameObject playgroundButton;

    void OnGUI()
    {
        currentEvent = Event.current;
        CheckPlayerInput(currentEvent);
        CheckCurrentCheatString();
    }

    private void CheckPlayerInput(Event e)
    {
        if (ValidatePlayerInput(e))
        {
            CheckForKeyInCheatString(e);
        }
    }

    private bool ValidatePlayerInput(Event e)
    {
        bool isValid = false;
        if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1 && char.IsLetter(e.keyCode.ToString()[0]))
        {
            isValid = true;
        }
        return isValid;
    }
    
    private void CheckForKeyInCheatString(Event e)
    {
        for (int i = 0; i < cheatStrings.Length; i++)
        {
            if (inputIndex < cheatStrings[i].Length)  // if current input type index ("playg" == 3) is less than currently checking cheat code string length (3 < "playg".length)
            {
                if (e.keyCode.ToString().ToLower() == cheatStrings[i][inputIndex].ToString())  // if current input value is equal to currently checking chead string ("g" == "playg"[4])
                {
                    ConcatenateString(e);  // add current input value into stringToCheck and return from this method
                    return;
                }
            }
        }
        ResetValues();  // if current input value is not match any of cheat code string at indicated index than reset strToCheck value and inputIndex to start listening from beginning 
    }

    private void ConcatenateString(Event e)
    {
        strToCheck += e.keyCode.ToString().ToLower();
        inputIndex++;
    }

    private void CheckCurrentCheatString()
    {
        foreach (var item in cheatStrings)
        {
            if (strToCheck == item)
            {
                ActivateCheat(item);
                ResetValues();
            }
        }
    }

    private void ActivateCheat(string cheat)
    {
        switch (cheat)
        {
            case "playground":
                playgroundButton.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void ResetValues()
    {
        inputIndex = 0;
        strToCheck = "";
    }

    public void LoadPlayground()
    {
        SceneManager.LoadScene("Playground");
    }
}
