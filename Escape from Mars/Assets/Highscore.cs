using System.Linq;
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
    private List<string> playerNames = new List<string>();
    private List<int> playerScores = new List<int>();
    private int scoresLimit = 10;

    void Start()
    {
        saveManagerRef = GetComponent<SaveManager>();
        if (saveManagerRef.HighscoreRecordEmpty())
        {
            ClearColums();
        }
        else
        {
            //print(saveManagerRef.GetHighscoreBase());  //TODO remove if no need
            CreateHighscoresLists();
            ClearColums();
            FillPlayerNames();
            FillPlayerScores();
            FillPlayerPlaces();
        }
    }

    private void ClearColums()
    {
        places.text = "";
        names.text = "";
        scores.text = "";
    }

    private Dictionary<string, int> ConvertScoresToInt()
    {
        Dictionary<string, string> notSortedHighscores = saveManagerRef.GetHighscoresDict();
        Dictionary<string, int> parsedHighscores = new Dictionary<string, int>();

        foreach (var item in notSortedHighscores)
        {
            parsedHighscores.Add(item.Key, int.Parse(item.Value));
        }

        return parsedHighscores;
    }

    private void CreateHighscoresLists()
    {
        var counter = 0;
        Dictionary<string, int> notSortedHighscores = ConvertScoresToInt();
        Dictionary<string, int> sortedHighscores = new Dictionary<string, int>();


        foreach (KeyValuePair<string, int> item in notSortedHighscores.OrderByDescending(key => key.Value))
        {
            playerNames.Add(item.Key);
            playerScores.Add(item.Value);
            counter++;
            if (counter == scoresLimit) { break; }
        }
    }

    private void FillPlayerNames()
    {
        var counter = 0;

        foreach (var item in playerNames)
        {
            if (counter == playerNames.Count - 1)
            {
                names.text += $"{item}";
            }
            else
            {
                names.text += $"{item}\n";
                counter++;
            }
        }
    }

    private void FillPlayerScores()
    {
        var counter = 0;

        foreach (var item in playerScores)
        {
            if (counter == playerScores.Count - 1)
            {
                scores.text += $"{item}";
            }
            else
            {
                scores.text += $"{item}\n";
                counter++;
            }
        }
    }

    private void FillPlayerPlaces()
    {
        for (int i = 1; i <= scoresLimit; i++)
        {
            if (i == scoresLimit)
            {
                places.text += $"{i}";
            }
            else
            {
                places.text += $"{i}\n";
            }
        }
    }
}
