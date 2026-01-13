using System;
using _Project.Scripts.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.View
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private float scaleDuration = 0.2f;
        [SerializeField] private TextMeshPro charText;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private GameConfig _gameConfig;
        
        public int Level { get; private set; }

        [Inject]
        public void Construct(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            Level = 0;
            spriteRenderer.color = _gameConfig.alphabet[Level].bgColor;
        }
        
        public class Pool : MonoMemoryPool<TileView> {}

        public void MoveTo(Vector3 targetPosition)
        {
            transform.DOLocalMove(targetPosition, moveDuration).SetEase(Ease.Linear);
        }

        public void Spawn()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.Linear);
        }

        public void IncreaseLevel()
        {
            Level++;
            UpdateVisuals();
            
            transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 10, 1);
        }

        private void UpdateVisuals()
        {
            if (Level >= _gameConfig.alphabet.Count) 
            {
                Debug.LogWarning("Конец алфавита");
                return; 
            }

            var data = _gameConfig.alphabet[Level]; 
            charText.text = data.character.ToString();

            if (spriteRenderer) 
                spriteRenderer.color = data.bgColor;
        }
        
        public void MoveToAndDestroy(Vector3 targetPosition, TileView.Pool pool, Action onComplete)
        {
            spriteRenderer.sortingOrder = 10; 

            transform.DOLocalMove(targetPosition, moveDuration).OnComplete(() =>
            {
                spriteRenderer.sortingOrder = 0;
                Reset();
				
                pool.Despawn(this);
                
                onComplete?.Invoke();
            });
        }
        private void Reset()
        {
            Level = 0;
            spriteRenderer.color = _gameConfig.alphabet[Level].bgColor;
            charText.text = _gameConfig.alphabet[Level].character.ToString();
        }
    }   
}
