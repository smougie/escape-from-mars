using UnityEngine;
using TMPro;


public class UnderlineText : MonoBehaviour
{
    private TMP_Text buttonText;
    private string startingText;
    private string currentText;

    void Start()
    {
        buttonText = gameObject.GetComponent<TMP_Text>();
        startingText = buttonText.text;
    }

    public void SetUnderlineText()
    {
        currentText = buttonText.text;
        buttonText.text = "<u>" + currentText;
    }

    public void RemoveUnderlineText()
    {
        buttonText = gameObject.GetComponent<TMP_Text>();
        buttonText.text = startingText;
    }
}
