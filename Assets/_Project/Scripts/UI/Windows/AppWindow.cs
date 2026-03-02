using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.UI.Windows
{
    public class AppWindow : MonoBehaviour
    {
        [SerializeField] private WindowType type;

        public WindowType Type => type;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}