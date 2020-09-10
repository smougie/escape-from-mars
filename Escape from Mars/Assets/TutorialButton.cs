using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] float flashSpeed = 1f;
    [SerializeField] float period = 1f;
    private Button button;
    private ColorBlock tempColor;
    private Color newColor;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        tempColor = button.colors;
        newColor = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * flashSpeed, period));
        tempColor.normalColor = newColor;
        button.colors = tempColor;
    }

    public void LoadTutorialLevel()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
