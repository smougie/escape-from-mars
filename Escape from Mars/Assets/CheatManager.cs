using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private string[] cheatStrings = { "playg", "playo", "tutorial"};
    private string strToCheck = "";
    private int inputIndex = 0;
    private int cheatIndex = 0;
    private Event currentEvent;

    void OnGUI()
    {
        currentEvent = Event.current;
        CheckPlayerInput(currentEvent);
        CheckCurrentCheatString();
        print(strToCheck);
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
            if (inputIndex < cheatStrings[i].Length)
            {
                if (e.keyCode.ToString().ToLower() == cheatStrings[i][inputIndex].ToString())
                {
                    ConcatenateString(e);
                    return;
                }
            }
        }
        ResetValues();
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
                print($"{strToCheck} cheat entered - enabling assigned cheat mode! >;->");
                ResetValues();
            }
        }
    }

    private void ResetValues()
    {
        Debug.Log("Reseting values");
        inputIndex = 0;
        strToCheck = "";
    }
}
