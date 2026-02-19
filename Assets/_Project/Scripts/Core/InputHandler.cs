using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core
{
    public class InputHandler : ITickable
    {
        private readonly GridManager _gridManager;
        private readonly WindowManager _windowManager;
        private Vector2 _startTouchPosition;
        private bool _isSwiping;
        private const float SwipeThreshold = 50f;

        public InputHandler(GridManager gridManager, WindowManager windowManager)
        {
            _gridManager = gridManager;
            _windowManager = windowManager;
        }

        public void Tick()
        {
            if (_windowManager.CurrentWindow != WindowType.Home)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                _startTouchPosition = Input.mousePosition;
                _isSwiping = true;
            }
            else if (Input.GetMouseButtonUp(0) && _isSwiping)
            {
                _isSwiping = false;
                ProcessSwipe(Input.mousePosition);
            }
        }
        
        private void ProcessSwipe(Vector2 endTouchPosition)
        {
            Vector2 vector = endTouchPosition - _startTouchPosition;
            if (vector.magnitude < SwipeThreshold) return; 

            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
            {
                _gridManager.Move(vector.x > 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                _gridManager.Move(vector.y > 0 ? Direction.Up : Direction.Down);
            }
        }
    }
}