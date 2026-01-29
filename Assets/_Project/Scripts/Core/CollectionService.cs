    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Zenject;

    namespace _Project.Scripts.Core
    {
        public class CollectionService : IInitializable, IDisposable
        {
            public event EventHandler<int> OnNewLetterDiscovered;
            
            private readonly GridManager _gridManager;

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
                IncreaseLettersCount(levelIndex);
                if (IsLetterUnlocked(levelIndex))
                    return;

                PlayerPrefs.SetInt($"Letter_Unlocked_{levelIndex}", 1);
                PlayerPrefs.Save();
                
                Debug.Log($"НОВАЯ БУКВА ОТКРЫТА: {levelIndex}");
                
                OnNewLetterDiscovered?.Invoke(this, levelIndex);
            }

            private bool IsLetterUnlocked(int levelIndex)
            {
                return PlayerPrefs.GetInt($"Letter_Unlocked_{levelIndex}", 0) == 1;
            }

            private void ClearProgress()
            {
                PlayerPrefs.DeleteAll();
            }

            private void IncreaseLettersCount(int levelIndex)
            {
                int currentCount = PlayerPrefs.GetInt($"Letter_Count_{levelIndex}", 0);
                PlayerPrefs.SetInt($"Letter_Count_{levelIndex}", currentCount + 1);
            }
        }
    }