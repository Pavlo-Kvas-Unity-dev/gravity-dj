using System;

public interface IWindowClosingHandler
{
    #region Public Methods and Operators

    /// <summary>
    ///   Called when the window was closed.
    /// </summary>
    /// <param name="closingFinishedCallback">This callback has to be called when the handler has finished with its actions.</param>
    void OnWindowClosed(Action closingFinishedCallback);

    #endregion
}