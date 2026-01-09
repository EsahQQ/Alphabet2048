using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.View;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core
{
    public class GridManager : IInitializable
    {
        private readonly TileView.Pool _tilePool;
        private readonly GameConfig _gameConfig;
        private readonly Transform _slotsContainer;
        private TileView[,] _tiles;
        private Vector2 _startPosition;
        

        public GridManager(TileView.Pool tilePool, GameConfig gameConfig, Transform slotsContainer)
        {
            _tilePool = tilePool;
            _gameConfig = gameConfig;
            _slotsContainer = slotsContainer;
        }
        
        public void Initialize()
        {
            int rows = _gameConfig.rowsCount;
            int columns = _gameConfig.columnsCount;
            _tiles = new TileView[rows, columns];
            
            float width = columns * _gameConfig.cellSize + (columns - 1) * _gameConfig.spacing;
            float height = rows * _gameConfig.cellSize + (rows - 1) * _gameConfig.spacing;
            
            _startPosition = new Vector2(
                -width / 2 + _gameConfig.cellSize / 2,
                -height / 2 + _gameConfig.cellSize / 2
            );

            CreateGridBackground();

            SpawnRandomTile();
            SpawnRandomTile();
        }

        public void Move(Direction direction)
        {
            Vector2Int dirVector = GetDirectionVector(direction);

            bool moved = false;

            var xRange = (direction == Direction.Right) ? new[]{3,2,1,0} : new[]{0,1,2,3};
            var yRange = (direction == Direction.Up)    ? new[]{3,2,1,0} : new[]{0,1,2,3};

            foreach (int x in xRange)
            {
                foreach (int y in yRange)
                {
                    TileView tile = _tiles[x, y];
                    
                    if (tile == null) continue;

                    Vector2Int nextCell = new Vector2Int(x, y);
                    Vector2Int farthestEmpty = nextCell;

                    while (true)
                    {
                        nextCell += dirVector;

                        if (nextCell.x < 0 || nextCell.x >= _gameConfig.columnsCount ||
                            nextCell.y < 0 || nextCell.y >= _gameConfig.rowsCount) 
                            break;
                        
                        if (_tiles[nextCell.x, nextCell.y] != null) 
                            break;
                        
                        farthestEmpty = nextCell;
                    }

                    if (farthestEmpty != new Vector2Int(x, y))
                    {
                        _tiles[x, y] = null; 
                        _tiles[farthestEmpty.x, farthestEmpty.y] = tile;
                        tile.MoveTo(GetWorldPosition(farthestEmpty.x, farthestEmpty.y));
                        moved = true;
                    }
                }
            }

            if (moved)
            {
                SpawnRandomTile(); 
            }
        }
        
        private Vector2Int GetDirectionVector(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:  return new Vector2Int(-1, 0);
                case Direction.Right: return new Vector2Int(1, 0);
                case Direction.Up:    return new Vector2Int(0, 1);
                case Direction.Down:  return new Vector2Int(0, -1);
                default: return Vector2Int.zero;
            }
        }
        
        private Vector3 GetWorldPosition(int x, int y)
        {
            float xPos = _startPosition.x + x * (_gameConfig.cellSize + _gameConfig.spacing);
            float yPos = _startPosition.y + y * (_gameConfig.cellSize + _gameConfig.spacing);
            
            return new Vector3(xPos, yPos, 0);
        }

        private void CreateGridBackground()
        {
            for (int x = 0; x < _gameConfig.columnsCount; x++)
            {
                for (int y = 0; y < _gameConfig.rowsCount; y++)
                {
                    var slot = Object.Instantiate(_gameConfig.slotPrefab, _slotsContainer);
                    slot.transform.localPosition = GetWorldPosition(x, y);
                }
            }
        }

        private void SpawnRandomTile()
        {
            var emptyCells = new List<Vector2Int>();
    
            for (int x = 0; x < _gameConfig.columnsCount; x++)
            {
                for (int y = 0; y < _gameConfig.rowsCount; y++)
                {
                    if (_tiles[x, y] == null)
                    {
                        emptyCells.Add(new Vector2Int(x, y));
                    }
                }
            }
            
            if (emptyCells.Count == 0) return;

            var coords = emptyCells[Random.Range(0, emptyCells.Count)];

            var tile = _tilePool.Spawn();
            tile.transform.localPosition = GetWorldPosition(coords.x, coords.y);
            tile.Spawn();

            _tiles[coords.x, coords.y] = tile;
        }
    }
}
