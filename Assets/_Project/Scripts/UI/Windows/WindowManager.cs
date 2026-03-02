using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.UI.Windows
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private AppWindow[] windows;
        [SerializeField] private WindowType startWindow = WindowType.Home;
        
        public WindowType CurrentWindow { get; private set; }

        private void Start()
        {
            OpenWindow(startWindow);
        }

        public void OpenWindow(WindowType type)
        {
            if (CurrentWindow == type) return;
            
            CurrentWindow = type;
            foreach (var window in windows)
            {
                if (window.Type == type)
                    window.Open();
                else
                    window.Close();
            }
        }
    }
}