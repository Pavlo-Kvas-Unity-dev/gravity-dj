using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("Slash/Windows/Window Manager")]
    public class WindowManager : MonoBehaviour, IWindowManager
    {
        #region Fields

        /// <summary>
        ///   Preloaded windows.
        /// </summary>
        private readonly List<Window> preloadedWindows = new List<Window>();

        /// <summary>
        ///   Open and loading windows.
        /// </summary>
        private readonly List<Window> windows = new List<Window>();

        /// <summary>
        ///   Windows to preload for faster opening.
        /// </summary>
        public string[] PreloadedWindowIds;

        /// <summary>
        ///   Root object to anchor windows under.
        /// </summary>
        public Transform UIRoot;

        #endregion

        #region Events

        /// <summary>
        ///   Callback when a window was opened.
        /// </summary>
        public event Action<Window> WindowOpened;

        /// <summary>
        ///   Callback when a window was closed.
        /// </summary>
        public event Action<Window> WindowClosed;

        #endregion

        #region Properties

        /// <summary>
        ///   Window manager singleton.
        /// </summary>
        public static WindowManager Instance { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Closes the window with the specified window id, sending the specified return value to listeners.
        /// </summary>
        /// <param name="windowId">Id of window to close.</param>
        /// <param name="returnValue">Return value of the window.</param>
        /// <returns>True if the window was closed successfully; false, if it isn't open.</returns>
        public bool CloseWindow(string windowId, object returnValue = null)
        {
            var window = this.GetOpenWindow(windowId);
            if (window == null)
            {
                Debug.LogError("Can't close window with id '" + windowId + "', not found in open windows.", this);
                return false;
            }

            return this.CloseWindow(window, returnValue);
        }

        /// <summary>
        ///   Closes the specified window, sending the specified return value to listeners.
        /// </summary>
        /// <param name="window">Window to close.</param>
        /// <param name="returnValue">Return value of the window.</param>
        /// <returns>True if the window was closed successfully; false, if it isn't open.</returns>
        public bool CloseWindow(Window window, object returnValue = null)
        {
            if (!this.windows.Contains(window))
            {
                Debug.LogError("Can't close window '" + window + "', not found in open windows.", this);
                return false;
            }

            // Remove window.
            this.windows.Remove(window);

            Debug.Log("Closed window '" + window + "' with id '" + window.WindowId + "'.");

            // Notify listeners.
            this.OnWindowClosed(window, () => this.CleanupClosedWindow(window));

            if (window.OnClosed != null)
            {
                window.OnClosed(window, returnValue);
                window.OnClosed = null;
            }

            return true;
        }

        /// <summary>
        ///   Opens the window with the specified id. The id is equal to the scene name the window is stored.
        /// </summary>
        /// <param name="windowId">Id of window to open</param>
        /// <param name="context">Optional window data.</param>
        /// <param name="onClosedCallback">Callback when window was closed.</param>
        /// <param name="onOpenedCallback">Callback when window was opened.</param>
        /// <returns>Returns the object that holds the information about the window that should be opened.</returns>
        public Window OpenWindow(
            string windowId,
            object context = null,
            Action<Window, object> onClosedCallback = null,
            Action<Window> onOpenedCallback = null)
        {
            // Check if window already open.
            var window = this.GetOpenWindow(windowId);
            if (window != null)
            {
                Debug.LogWarning("Window with id '" + windowId + " already open.", this);
                return window;
            }

            // Check if preloaded.
            window = this.preloadedWindows.FirstOrDefault(preloadedWindow => preloadedWindow.WindowId == windowId);
            if (window == null)
            {
                // Create new window.
                window = new Window { WindowId = windowId };
            }

            window.Context = context;
            window.OnOpened = onOpenedCallback;
            window.OnClosed = onClosedCallback;

            this.OpenWindow(window);

            return window;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple window manager found, please use only one.", this);
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Start()
        {
            // Preload windows.
            foreach (var windowId in this.PreloadedWindowIds)
            {
                // Create window.
                var window = new Window { WindowId = windowId };

                // Load window.
                this.StartCoroutine(this.DoPreloadWindow(window));
            }
        }

        private void AnchorWindow(Window window)
        {
            if (this.UIRoot == null)
            {
                return;
            }

            // Anchor window under the UI root.
            foreach (var windowRoot in window.Roots)
            {
                // Adjust transform of window canvas.
                var windowRectTransform = windowRoot as RectTransform;
                if (windowRectTransform == null)
                {
                    continue;
                }

                var windowCanvas = windowRoot.GetComponent<Canvas>();
                if (windowCanvas == null)
                {
                    continue;
                }

                // Re-parent transform.
                windowRoot.SetParent(this.UIRoot, false);

                // Make sure that sort order is still considered.
                windowCanvas.overrideSorting = true;

                // Stretch window canvas to root canvas.
                windowRectTransform.anchorMin = Vector2.zero;
                windowRectTransform.anchorMax = Vector2.one;
                windowRectTransform.sizeDelta = Vector2.zero;
                windowRectTransform.localScale = Vector3.one;
                windowRectTransform.anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        ///   Do after close actions were made (e.g. animation).
        /// </summary>
        /// <param name="window">Window to finish closing.</param>
        private void CleanupClosedWindow(Window window)
        {
            // Only hide if preloaded.
            if (this.preloadedWindows.Contains(window))
            {
                window.Hide();
            }
            else
            {
                this.DoUnloadWindow(window);
            }
        }

        private IEnumerator DoLoadWindow(Window window)
        {
            // Load scene.
            var loadSceneAsync = SceneManager.LoadSceneAsync(window.WindowId, LoadSceneMode.Additive);
            yield return loadSceneAsync;

            // Check if window scene loaded.
            window.Scene = SceneManager.GetSceneByName(window.WindowId);
            if (!window.Scene.IsValid())
            {
                Debug.LogWarningFormat(
                    "Couldn't open window '{0}' because scene doesn't exist in build settings. Available scenes: {1}",
                    window.WindowId,
                    SceneManager.GetAllScenes()
                        .Select(scene => scene.name)
                        .Aggregate(
                            string.Empty,
                            (text, item) => (string.IsNullOrEmpty(text) ? string.Empty : text + ", ") + item));

                yield break;
            }

            // Get new window roots.
            var windowRoots =
                FindObjectsOfType<Transform>()
                    .Where(
                        existingTransform =>
                            existingTransform.parent == null && existingTransform.gameObject.scene == window.Scene)
                    .ToArray();

            // Setup window.
            window.Roots = windowRoots;
            window.Loaded = true;

            Debug.Log("Loaded window '" + window + "' with id '" + window.WindowId + "'.");

            // Anchor window under UI root.
            this.AnchorWindow(window);
        }

        private IEnumerator DoOpenWindow(Window window)
        {
            // Add window to open windows.
            this.windows.Add(window);

            if (!window.Loaded)
            {
                yield return this.DoLoadWindow(window);
            }
            else
            {
                // Show window.
                window.Show();
            }

            // Check if loading was successful.
            if (window.Loaded)
            {
                // Notify listeners.
                this.OnWindowOpened(window);

                var handler = window.OnOpened;
                if (handler != null)
                {
                    handler(window);
                }
            }
            else
            {
                // Remove from open windows.
                this.windows.Remove(window);
            }
        }

        private IEnumerator DoPreloadWindow(Window window)
        {
            // Load window.
            yield return this.DoLoadWindow(window);

            if (window.Loaded)
            {
                // Hide window.
                window.Hide();

                // Add to preload windows.
                this.preloadedWindows.Add(window);
            }
        }

        private void DoUnloadWindow(Window window)
        {
            // Remove window roots that may have been anchored to the UI root.
            foreach (var windowRoot in window.Roots)
            {
                if (windowRoot.gameObject != null)
                {
                    Destroy(windowRoot.gameObject);
                }
            }

            // Allow other referencers to check whether this window is still open.
            window.Loaded = false;

            this.StartCoroutine(this.UnloadScene(window.Scene.buildIndex));
        }

        private Window GetOpenWindow(string windowId)
        {
            return this.windows.FirstOrDefault(openWindow => openWindow.WindowId == windowId);
        }

        private void OnWindowClosed(Window window, Action cleanupWindowAction)
        {
            var handler = this.WindowClosed;
            if (handler != null)
            {
                handler(window);
            }

            // Check for handler that takes care about closing the window.
            var finishClosingHandler =
                window.Roots.Select(root => root.GetComponentInChildren<IWindowClosingHandler>())
                    .FirstOrDefault(closingHandler => closingHandler != null);
            if (finishClosingHandler != null)
            {
                finishClosingHandler.OnWindowClosed(cleanupWindowAction);
            }
            else
            {
                cleanupWindowAction();
            }
        }

        private void OnWindowOpened(Window window)
        {
            var handler = this.WindowOpened;
            if (handler != null)
            {
                handler(window);
            }
        }

        private void OpenWindow(Window window)
        {
            this.StartCoroutine(this.DoOpenWindow(window));
        }

        private IEnumerator UnloadScene(int buildIndex)
        {
            // Workaround: Waiting a bit before unloading scene 
            // (http://forum.unity3d.com/threads/unity-hangs-on-scenemanager-unloadscene.380116/)
            yield return new WaitForSeconds(0.1f);

            // Unload window scene.
            SceneManager.UnloadScene(buildIndex);
        }

        #endregion
    }