using UnityEngine;

namespace GravityDJ.UI.WindowManager
{
    public class CloseWindow : MonoBehaviour
    {
        #region Fields
        
        /// <summary>
        /// Id of window to close
        /// </summary>
        private string WindowId;

        #endregion

        #region Public Methods and Operators

        public void Execute()
        {
            if (WindowManager.Instance != null)
            {
                if (!string.IsNullOrEmpty(this.WindowId))
                {
                    WindowManager.Instance.CloseWindow(this.WindowId);
                }
                else
                {
                    Debug.LogWarning("No window id set.", this);
                }
            }
            else
            {
                Debug.LogWarning("No window manager found.", this);
            }
        }

        #endregion

        #region Methods

        protected void Awake()
        {
            // Use own scene to close.
            if (string.IsNullOrEmpty(this.WindowId))
            {
                this.WindowId = this.transform.gameObject.scene.name;
            }
        }

        #endregion
    }
}