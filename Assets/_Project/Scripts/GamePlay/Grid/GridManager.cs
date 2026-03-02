using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.GamePlay.LevelsLogic;
using _Project.Scripts.GamePlay.View;
using _Project.Scripts.Utils;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Project.Scripts.GamePlay.Grid
{
    public class GridManager : IInitializable
    {
        private readonly TileView.Pool _tilePool;
        private readonly GameConfig _gameConfig;
        private readonly LevelPicker _levelPicker;
        private readonly Transform _slotsContainer;
        private TileView[,] _tiles;
        private Vector2 _startPosition;

        public event EventHandler<int> OnLetterSpawned;

        public GridManager(TileView.Pool tilePool, GameConfig gameConfig, Transform slotsContainer, LevelPicker levelPicker)
        {
            _tilePool = tilePool;
            _gameConfig = gameConfig;
            _slotsContainer = slotsContainer;
            _levelPicker = levelPicker;
        }
        
        public void Initialize()
        {
            var rows = _gameConfig.rowsCount;
            var columns = _gameConfig.columnsCount;
            _tiles = new TileView[rows, columns];
            
            var width = columns * _gameConfig.cellSize + (columns - 1) * _gameConfig.spacing;
            var height = rows * _gameConfig.cellSize + (rows - 1) * _gameConfig.spacing;
            
            _startPosition = new Vector2(
                -width / 2 + _gameConfig.cellSize / 2,
                -height / 2 + _gameConfig.cellSize / 2
            );

            CreateGridBackground();
            SpawnInitialTiles();
        }

        public void Move(Direction direction)
        {
            var dirVector = GetDirectionVector(direction);

            bool moved = false;
            
            var mergedTiles = new HashSet<TileView>();

            var xRange = (direction == Direction.Right) ? new[]{3,2,1,0} : new[]{0,1,2,3};
            var yRange = (direction == Direction.Up)    ? new[]{3,2,1,0} : new[]{0,1,2,3};

            foreach (var x in xRange)
                foreach (var y in yRange)
                {
                    var tile = _tiles[x, y];
                    
                    if (tile == null) continue;

                    var nextCell = new Vector2Int(x, y);
                    var farthestEmpty = nextCell;
                    var currentPos = new Vector2Int(x, y);

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
                    
                    var mergeCandidatePos = farthestEmpty + dirVector;
                    
                    if (IsInsideGrid(mergeCandidatePos))
                    {
                        var targetTile = _tiles[mergeCandidatePos.x, mergeCandidatePos.y];
                        
                        if (targetTile != null && targetTile.Level == tile.Level && !mergedTiles.Contains(targetTile))
                        {
                            tile.MoveToAndDestroy(GetWorldPosition(mergeCandidatePos.x, mergeCandidatePos.y), _tilePool, () =>
                            {
                                targetTile.IncreaseLevel();
                                OnLetterSpawned?.Invoke(this, targetTile.Level);
                            });
                            mergedTiles.Add(targetTile);
                            _tiles[currentPos.x, currentPos.y] = null;
                            moved = true;
                            continue; 
                        }
                    }

                    if (farthestEmpty == new Vector2Int(x, y)) continue;
                    
                    _tiles[x, y] = null; 
                    _tiles[farthestEmpty.x, farthestEmpty.y] = tile;
                    tile.MoveTo(GetWorldPosition(farthestEmpty.x, farthestEmpty.y));
                    moved = true;
                }
            

            if (moved)
            {
                SpawnRandomTile(); 
            }
        }

        public void Reset()
        {
            for (var x = 0; x < _tiles.GetLength(0); x++)
                for (var y = 0; y < _tiles.GetLength(1); y++)
                {
                    if (_tiles[x, y] == null) continue;
                    
                    _tilePool.Despawn(_tiles[x, y]);
                    _tiles[x, y] = null;
                }
        }
        
        public void SpawnInitialTiles()
        {
            SpawnRandomTile();
            SpawnRandomTile();
        }
        
        private bool IsInsideGrid(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _gameConfig.columnsCount &&
                   pos.y >= 0 && pos.y < _gameConfig.rowsCount;
        }
        
        private Vector2Int GetDirectionVector(Direction dir)
        {
            return dir switch
            {
                Direction.Left => new Vector2Int(-1, 0),
                Direction.Right => new Vector2Int(1, 0),
                Direction.Up => new Vector2Int(0, 1),
                Direction.Down => new Vector2Int(0, -1),
                _ => Vector2Int.zero
            };
        }
        
        private Vector3 GetWorldPosition(int x, int y)
        {
            var xPos = _startPosition.x + x * (_gameConfig.cellSize + _gameConfig.spacing);
            var yPos = _startPosition.y + y * (_gameConfig.cellSize + _gameConfig.spacing);
            
            return new Vector3(xPos, yPos, 0);
        }

        private void CreateGridBackground()
        {
            var background = Object.Instantiate(_gameConfig.backgroundPrefab, _slotsContainer);
            background.transform.localScale = new Vector3(_gameConfig.columnsCount + _gameConfig.spacing * (_gameConfig.columnsCount + 1), _gameConfig.rowsCount + _gameConfig.spacing * (_gameConfig.rowsCount + 1), 1);
            for (var x = 0; x < _gameConfig.columnsCount; x++)
                for (var y = 0; y < _gameConfig.rowsCount; y++)
                {   
                    var slot = Object.Instantiate(_gameConfig.slotPrefab, _slotsContainer);
                    slot.transform.localPosition = GetWorldPosition(x, y);
                }
        }

        private void SpawnRandomTile()
        {
            var emptyCells = new List<Vector2Int>();
    
            for (var x = 0; x < _gameConfig.columnsCount; x++)
                for (var y = 0; y < _gameConfig.rowsCount; y++)
                {
                    if (_tiles[x, y] == null)
                    {
                        emptyCells.Add(new Vector2Int(x, y));
                    }
                }
            
            if (emptyCells.Count == 0) return;

            var coords = emptyCells[Random.Range(0, emptyCells.Count)];

            var tile = _tilePool.Spawn();
            tile.transform.localPosition = GetWorldPosition(coords.x, coords.y);
            tile.Spawn();

            _tiles[coords.x, coords.y] = tile;

            var startOffset = _levelPicker.CurrentStartLetterNumber;

            tile.SetLevel(startOffset);
            
            OnLetterSpawned?.Invoke(this, tile.Level);
        }
    }
}
