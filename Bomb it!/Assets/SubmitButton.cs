using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    [SerializeField] Color startColor;
    [SerializeField] Color finishColor;
    private Color newColor;
    private ColorBlock tempButtonColor;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();        
    }

    void Update()
    {
        tempButtonColor = button.colors;
        print(Mathf.PingPong(Time.time, 1f));
        newColor = Color.Lerp(startColor, finishColor, Mathf.PingPong(Time.time, 1f));
        tempButtonColor.normalColor = newColor;
        button.colors = tempButtonColor;
    }
}
