using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    [SerializeField] TMP_Text places;
    [SerializeField] TMP_Text names;
    [SerializeField] TMP_Text scores;
    private SaveManager saveManagerRef;
    private Dictionary<string, string> highscoreDict = new Dictionary<string, string>();

    void Start()
    {
        saveManagerRef = GetComponent<SaveManager>();
        if (saveManagerRef.HighscoreRecordEmpty())
        {
            ClearColums();  // TODO check when highscore record is empty
        }
        else
        {
            Debug.Log("found record");
            saveManagerRef.GetHighscoresDict();
        }
    }

    public void ClearColums()
    {
        places.text = "";
        names.text = "";
        scores.text = "";
    }
}
