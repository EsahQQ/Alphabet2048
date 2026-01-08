using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.View
{
    public class TileView : MonoBehaviour
    {
        public class Pool : MonoMemoryPool<TileView> {}

        public void MoveTo(Vector3 targetPosition)
        {
            transform.DOLocalMove(targetPosition, 0.2f).SetEase(Ease.Linear);
        }
    }
}
