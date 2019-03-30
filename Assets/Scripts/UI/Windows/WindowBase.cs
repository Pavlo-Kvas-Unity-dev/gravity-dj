using System;
using UnityEngine;

namespace GravityDJ.UI
{
    public class WindowBase : MonoBehaviour, IWindow
    {
        private Action onCloseAction;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Open(Action onCloseAction = null)
        {
            gameObject.SetActive(true);
            this.onCloseAction = onCloseAction;
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
            onCloseAction?.Invoke();
        }
    }
}