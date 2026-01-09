using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.View
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private float scaleDuration = 0.2f;
        
        public class Pool : MonoMemoryPool<TileView> {}

        public void MoveTo(Vector3 targetPosition)
        {
            transform.DOLocalMove(targetPosition, moveDuration).SetEase(Ease.Linear);
        }

        public void Spawn()
        {
            this.transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.Linear);
        }
    }
}
