    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Zenject;

    namespace _Project.Scripts.Core
    {
        public class CollectionService : IInitializable, IDisposable
        {
            public event EventHandler<int> OnNewLetterDiscovered;
            
            private GridManager _gridManager;

            public CollectionService(GridManager gridManager)
            {
                _gridManager = gridManager;
            }
            
            public void Initialize()
            {
                _gridManager.OnLetterSpawned += UnlockLetter;
            }
            
            public void Dispose()
            {
                _gridManager.OnLetterSpawned -= UnlockLetter;
                ClearProgress();
            }
            
            private void UnlockLetter(object sender, int levelIndex)
            {
                if (IsLetterUnlocked(levelIndex))
                    return;

                PlayerPrefs.SetInt($"Letter_Unlocked_{levelIndex}", 1);
                PlayerPrefs.Save();
                
                Debug.Log($"НОВАЯ БУКВА ОТКРЫТА: {levelIndex}");
                
                OnNewLetterDiscovered?.Invoke(this, levelIndex);
            }

            public bool IsLetterUnlocked(int levelIndex)
            {
                return PlayerPrefs.GetInt($"Letter_Unlocked_{levelIndex}", 0) == 1;
            }
            
            public void ClearProgress()
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }