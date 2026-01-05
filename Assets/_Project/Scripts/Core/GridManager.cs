using _Project.Scripts.View;
using Zenject;

namespace _Project.Scripts.Core
{
    public class GridManager : IInitializable
    {
        private readonly TileView.Pool _tilePool;

        public GridManager(TileView.Pool tilePool)
        {
            _tilePool = tilePool;
        }
        
        public void Initialize()
        {
            _tilePool.Spawn();
        }
    }
}
