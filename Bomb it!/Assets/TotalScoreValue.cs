using UnityEngine;
using TMPro;

public class TotalScoreValue : MonoBehaviour
{
    private SaveManager saveManagerRef;
    private TMP_Text totalScoreText;

    void Start()
    {
        saveManagerRef = GetComponent<SaveManager>();
        totalScoreText = GetComponent<TMP_Text>();
        UpdateTotalScoreValue();
    }

    private void UpdateTotalScoreValue()
    {
        totalScoreText.text = $"{saveManagerRef.GetTotalScore()}";
    }
}
