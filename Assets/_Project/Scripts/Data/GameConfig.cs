using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/New GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public GameObject tilePrefab;
        public int[] gridSize = new int[2];
    }
}
