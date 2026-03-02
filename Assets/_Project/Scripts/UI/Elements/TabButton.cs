using _Project.Scripts.UI.Windows;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.Elements
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private WindowType targetWindow;
        
        private WindowManager _windowManager;
        private Button _button;

        [Inject]
        public void Construct(WindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _windowManager.OpenWindow(targetWindow);
        }
    }
}