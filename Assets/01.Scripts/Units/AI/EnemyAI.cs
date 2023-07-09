using System;
using _01.Scripts.Managers;
using UnityEngine;

namespace _01.Scripts.Units.AI
{
    public class EnemyAI : MonoBehaviour
    {
        private Unit unit = null;
        
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        
        private void Update()
        {
            if (!GameManager.IsGameStart) return;
            
            if (unit.IsSelected)
            {
                var nextPos = unit.GridPosition;
                
                unit.Move();
            }
            else
            {
                unit.Select();
            }
        }
    }
}