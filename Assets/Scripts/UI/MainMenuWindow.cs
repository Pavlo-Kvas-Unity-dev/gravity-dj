using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour, IWindow
{
    private Action onPlay;
    private Action onExit;
    private Action onHelp;

    public void Open(Action onPlay, Action onHelp, Action onExit)
    {
        this.onPlay = onPlay;
        this.onHelp = onHelp;
        this.onExit = onExit;
        Open();
    }
    
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayClicked()
    {
        Close();
        onPlay?.Invoke();
    }

    public void OnHelpClicked()
    {
        onHelp?.Invoke();
    }

    public void OnExitClicked()
    {
        onExit?.Invoke();
    }
}
