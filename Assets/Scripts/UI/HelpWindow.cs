using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpWindow : MonoBehaviour, IWindow
{
    private Action onCloseAction;

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onCloseAction?.Invoke();
    }
    
    public void Open(Action onCloseAction = null)
    {
        gameObject.SetActive(true);
        this.onCloseAction = onCloseAction;
    }
}
