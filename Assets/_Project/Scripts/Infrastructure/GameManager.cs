using System;
using _Project.Scripts.GamePlay.Grid;
using _Project.Scripts.GamePlay.LevelsLogic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
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
            _gridManager.SpawnInitialTiles();
        }
    }
}