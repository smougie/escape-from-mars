using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private string[] cheatStrings = { "playg", "tutorial"};
    private string strToCheck = "";
    private int index = 0;
    private Event currentEvent;

    void OnGUI()
    {
        currentEvent = Event.current;
        CheckPlayerInput(currentEvent);
        CheckCurrentCheatString();
        if (currentEvent.type == EventType.KeyDown)
        {
            print(currentEvent.keyCode.ToString());
        }
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
        foreach (var item in cheatStrings)
        {
            if (index < item.Length)
            {
                if (e.keyCode.ToString().ToLower() == item[index].ToString())
                {
                    ConcatenateString(e);
                }
            }
        }
    }

    private void ConcatenateString(Event e)
    {
        strToCheck += e.keyCode.ToString().ToLower();
        index++;
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
        index = 0;
        strToCheck = "";
    }
}
