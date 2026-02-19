using UnityEngine;

namespace _Project.Scripts.Core
{
    public class AppWindow : MonoBehaviour
    {
        [SerializeField] private WindowType _type;

        public WindowType Type => _type;

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