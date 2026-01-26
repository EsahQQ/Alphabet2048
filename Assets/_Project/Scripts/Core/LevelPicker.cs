using System;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class LevelPicker : MonoBehaviour
    {
        public int CurrentLevel { get; private set; } = 0;
        public int CurrentStartLetterNumber { get; private set; } = 0;
        
        public event EventHandler OnLevelChanged;

        public void LevelPlus()
        {
            CurrentLevel = Mathf.Min(CurrentLevel + 1, 2);
            CurrentStartLetterNumber = Mathf.Min(CurrentStartLetterNumber + 11, 22);
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public void LevelMinus()
        {
            CurrentLevel = Mathf.Max(CurrentLevel - 1, 0);
            CurrentStartLetterNumber = Mathf.Max(CurrentStartLetterNumber - 11, 0);
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}