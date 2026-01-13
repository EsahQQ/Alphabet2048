using TMPro;
using UnityEngine;

namespace _Project.Scripts.Utils
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;
        private float _deltaTime = 0.0f;
        
        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            var fps = 1.0f / _deltaTime;
            fpsText.text = Mathf.CeilToInt(fps).ToString();
        }
    }
}