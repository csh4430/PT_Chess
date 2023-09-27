using System;
using _01.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01.Scripts.Units.AI
{
    public class EnemyAI : MonoBehaviour
    {
        private Unit unit = null;
        
        private void Awake()
        {
            unit = GetComponent<Unit>();

            int x = Random.Range(2, 8);
            int y = Random.Range(2, 8);

            unit.GridPosition = new Vector2Int(x, y);
        }
        
        private void Update()
        {
            if (!GameManager.IsGameStart) return;
            
            if (unit.IsSelected)
            {
                var nextPos = unit.GridPosition;
                int ranNum = Random.Range(1, 5);
                switch(ranNum)
                {
                    case 1:
                        nextPos = new Vector2Int(unit.GridPosition.x + 1, unit.GridPosition.y);
                    break;
                    case 2:
                        nextPos = new Vector2Int(unit.GridPosition.x - 1, unit.GridPosition.y);
                        break;
                    case 3:
                        nextPos = new Vector2Int(unit.GridPosition.x, unit.GridPosition.y + 1);
                        break;
                    case 4:
                        nextPos = new Vector2Int(unit.GridPosition.x, unit.GridPosition.y - 1);
                        break;
                }
                unit.Move(nextPos);
                unit.Move();
            }
            else
            {
                unit.Select();
            }
        }
    }
}