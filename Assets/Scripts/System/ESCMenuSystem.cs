using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenuSystem
{
    bool isShowable;
    Action OnESCMenu;

    public void Initialize(Action OnESCMenu)
    {
        this.OnESCMenu = OnESCMenu;
    }

    public void OnPlayGame()
    {
        isShowable = true;
    }

    public void OnWaitSelectPlayMode()
    {
        isShowable = false;
    }

    public void ESCMenu()
    {
        if (isShowable == false) return;

        OnESCMenu?.Invoke();
        if (SelectPlayModeUI.Instance.gameObject.activeSelf == false)
            SelectPlayModeUI.Instance.ShowUI();
        else
            SelectPlayModeUI.Instance.CloseUI();
    }
}
