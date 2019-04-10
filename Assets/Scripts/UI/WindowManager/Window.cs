using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Window
{
    #region Properties

    /// <summary>
    ///   Data the window needs, e.g. for initialization.
    /// </summary>
    public object Context { get; set; }

    /// <summary>
    ///   Indicates if the window is loaded.
    /// </summary>
    public bool Loaded { get; set; }

    /// <summary>
    ///   Callback when window was closed.
    ///   Parameter 1: Closed window.
    ///   Parameter 2: Return value of window.
    /// </summary>
    public Action<Window, object> OnClosed { get; set; }

    /// <summary>
    ///   Callback when window was opened.
    ///   Parameter 1: Opened window.
    /// </summary>
    public Action<Window> OnOpened { get; set; }

    /// <summary>
    ///   Root transforms of loaded window scene.
    /// </summary>
    public Transform[] Roots { get; set; }

    /// <summary>
    ///   Scene the window was loaded from.
    /// </summary>
    public Scene Scene { get; set; }

    /// <summary>
    ///   Unique id of window.
    /// </summary>
    public string WindowId { get; set; }

    #endregion

    #region Public Methods and Operators

    public void Hide()//todo
    {
        foreach (var root in this.Roots)
        {
            root.gameObject.SetActive(false);
        }
    }

    public void Show()//todo
    {
        foreach (var root in this.Roots)
        {
            root.gameObject.SetActive(true);
        }
    }

    #endregion
}