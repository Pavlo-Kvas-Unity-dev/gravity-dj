using System;

public interface IWindowManager
{
    #region Events

    /// <summary>
    ///   Callback when a window was opened.
    /// </summary>
    event Action<Window> WindowOpened;

    /// <summary>
    ///   Callback when a window was closed.
    /// </summary>
    event Action<Window> WindowClosed;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///   Closes the window with the specified window id, sending the specified return value to listeners.
    /// </summary>
    /// <param name="windowId">Id of window to close.</param>
    /// <param name="returnValue">Return value of the window.</param>
    /// <returns>True if the window was closed successfully; false, if it isn't open.</returns>
    bool CloseWindow(string windowId, object returnValue = null);

    /// <summary>
    ///   Closes the specified window, sending the specified return value to listeners.
    /// </summary>
    /// <param name="window">Window to close.</param>
    /// <param name="returnValue">Return value of the window.</param>
    /// <returns>True if the window was closed successfully; false, if it isn't open.</returns>
    bool CloseWindow(Window window, object returnValue = null);

    /// <summary>
    ///   Opens the window with the specified id. The id is equal to the scene name the window is stored.
    /// </summary>
    /// <param name="windowId">Id of window to open</param>
    /// <param name="context">Optional window data.</param>
    /// <param name="onClosedCallback">Callback when window was closed.</param>
    /// <param name="onOpenedCallback">Callback when window was opened.</param>
    /// <returns>Returns the object that holds the information about the window that should be opened.</returns>
    Window OpenWindow(
        string windowId,
        object context = null,
        Action<Window, object> onClosedCallback = null,
        Action<Window> onOpenedCallback = null);

    #endregion
}