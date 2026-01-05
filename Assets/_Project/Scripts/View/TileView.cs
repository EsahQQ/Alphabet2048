using UnityEngine;
using Zenject;

namespace _Project.Scripts.View
{
    public class TileView : MonoBehaviour
    {
        public class Pool : MonoMemoryPool<TileView> {}
    }
}
