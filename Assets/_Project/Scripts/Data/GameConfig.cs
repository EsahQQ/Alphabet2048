using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/New GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public GameObject tilePrefab;
        public int rowsCount = 4;
        public int columnsCount = 4;
        
        public float cellSize = 1.0f;
        public float spacing = 0.1f;
        public GameObject slotPrefab;
    }
}
