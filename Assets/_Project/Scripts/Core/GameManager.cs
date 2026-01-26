using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        private LevelPicker _levelPicker;
        private GridManager _gridManager;
        
        [Inject]
        public void Construct (LevelPicker levelPicker, GridManager gridManager)
        {
            _levelPicker = levelPicker;
            _levelPicker.OnLevelChanged += ChangeLevel;
            
            _gridManager =  gridManager;
        }
        

        private void ChangeLevel(object sender, EventArgs e)
        {
            Restart();
        }

        private void Restart()
        {
            _gridManager.Reset();
            _gridManager.Start();
        }
    }
}