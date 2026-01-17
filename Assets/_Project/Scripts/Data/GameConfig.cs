using System.Collections.Generic;
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
        public GameObject backgroundPrefab;
        
        public List<LetterData> alphabet; 
        
        public Gradient colorGradient; 
        
        [ContextMenu("Generate Alphabet")]
        private void GenerateAlphabet()
        {
            alphabet = new List<LetterData>();
            var chars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            
            for (int i = 0; i < chars.Length; i++)
            {
                var data = new LetterData();

                data.character = chars[i];
                
                float time = (float)i / (chars.Length - 1);
                data.bgColor = colorGradient.Evaluate(time);

                alphabet.Add(data);
            }
        }
    }
}
