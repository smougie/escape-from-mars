using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnderlineText : MonoBehaviour
{
    private TMP_Text buttonText;
    private string currentText;

    void Start()
    {
        buttonText = gameObject.GetComponent<TMP_Text>();
    }

    public void SetUnderlineText()
    {
        currentText = buttonText.text;
        buttonText.text = "<u>" + currentText;
    }

    public void RemoveUnderlineText()
    {
        buttonText = gameObject.GetComponent<TMP_Text>();
        buttonText.text = currentText;
    }
}
