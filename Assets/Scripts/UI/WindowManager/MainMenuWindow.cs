using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GravityDJ.UI
{
    public class MainMenuWindow : MonoBehaviour, IWindow
    {
        private Action onPlay;
        private Action onResume;
        private Action onExit;
        private Action onHelp;

        [SerializeField] private Button playButton;
        [SerializeField] private Button resumeButton;
        
        [Inject] private SettingsWindow settingsWindow;

        void Awake()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            resumeButton.onClick.AddListener(OnResumeClicked);
        }

        public void Open(Action onPlay, Action onResume, Action onHelp, Action onExit)
        {
            this.onPlay = onPlay;
            this.onResume = onResume;
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

        private void OnPlayClicked()
        {
            Close();
            onPlay?.Invoke();
        }

        private void OnResumeClicked()
        {
            Close();
            onResume?.Invoke();
        }

        public void OnSettingsClicked()
        {
            settingsWindow.Open();
        }
        
        public void OnHelpClicked()
        {
            onHelp?.Invoke();
        }

        public void OnExitClicked()
        {
            onExit?.Invoke();
        }

        public void Init(bool isGamePlaying)
        {
            playButton.gameObject.SetActive(!isGamePlaying);
            resumeButton.gameObject.SetActive(isGamePlaying);
        }
    
    
    }
}
