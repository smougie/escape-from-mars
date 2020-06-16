using UnityEngine;
using UnityEngine.UI;

public class SliderControler : MonoBehaviour
{
    [SerializeField] private Sprite yellowBar;
    [SerializeField] private Sprite redBar;
    private Image currentImage;

    void Start()
    {
        currentImage = GetComponent<Image>();
    }

    void Update()
    {
        if (currentImage.fillAmount <= .3f)
        {
            currentImage.sprite = redBar;
        }
        else
        {
            currentImage.sprite = yellowBar;
        }
    }
}
