using System;
using System.Linq;
using UnityEngine;

/// <summary>
///   Handles window closing and adjust the animator of the window accordingly.
/// </summary>
public class WindowAnimator : MonoBehaviour, IWindowClosingHandler
{
    #region Fields

    /// <summary>
    ///   Animator of window.
    /// </summary>
    public Animator Animator;

    /// <summary>
    ///   Animator parameter that indicates if window is open.
    /// </summary>
    public string AnimatorOpenedParameter = "is_open";

    /// <summary>
    ///   Callback to be invoked when animation sends event that it is finished.
    /// </summary>
    private Action closeFinishedCallback;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///   Called when the window was closed.
    /// </summary>
    /// <param name="closingFinishedCallback">This callback has to be called when the handler has finished with its actions.</param>
    public void OnWindowClosed(Action closingFinishedCallback)
    {
        if (this.Animator != null
            && this.Animator.parameters.Any(parameter => parameter.name == this.AnimatorOpenedParameter))
        {
            this.closeFinishedCallback = closingFinishedCallback;
            this.Animator.SetBool(this.AnimatorOpenedParameter, false);
        }
        else
        {
            closingFinishedCallback();
        }
    }

    /// <summary>
    ///   Called by an animation event when the close animation was finished.
    /// </summary>
    public void OnWindowCloseFinished()
    {
        if (this.closeFinishedCallback != null)
        {
            this.closeFinishedCallback();
            this.closeFinishedCallback = null;
        }
    }

    #endregion
}