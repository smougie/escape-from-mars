using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoManager : MonoBehaviour
{
    [SerializeField] GameObject tipsWindow;
    [SerializeField] GameObject gameTips;
    [SerializeField] GameObject checkSign;
    private bool tipsActive;
    
    void Start()
    {
        tipsActive = false;
        if (tipsWindow != null)
        {
            tipsWindow.SetActive(false);
        }
        if (gameTips != null)
        {

        }
        if (checkSign != null)
        {
            checkSign.SetActive(false);
        }
    }

    void Update()
    {
        if (tipsActive)
        {
            checkSign.SetActive(true);
            tipsWindow.SetActive(true);
            gameTips.SetActive(false);
        }
        else
        {
            checkSign.SetActive(false);
            tipsWindow.SetActive(false);
            gameTips.SetActive(true);
        }
    }

    public void SwitchTipsState()
    {
        tipsActive = !tipsActive;
    }
}
