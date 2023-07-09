using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Managers;
using _01.Scripts.Units;
using _01.Scripts.Utils;
using UnityEngine;

namespace _01.Scripts.Cells
{
    public class Cell : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int gridPosition = Vector2Int.zero;
        public Vector2Int GridPosition => gridPosition;
        
        [SerializeField]
        private Unit unitOnBlock = null;
        
        private List<Cell> _crossCells = new List<Cell>();
        private List<Cell> _nearCells = new List<Cell>();
        private void Awake()
        {
            gridPosition = Util.PositionToGrid(transform.position);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.IsGameStart);
        }

        public List<Cell> GetCrossCells()
        {
            throw null!;
        }
        
        public List<Cell> GetNearCells()
        {
            var result = new List<Cell>();
            for (var i = -8; i < 8; i++)
            {
                var cell = Define.GetCell(gridPosition + new Vector2Int(i, 0));
                if(cell != null)
                    result.Add(cell);
            }
            for (var i = -8; i < 8; i++)
            {
                var cell = Define.GetCell(gridPosition + new Vector2Int(0, i));
                if(cell != null)
                    result.Add(cell);
            }

            return result;
        }
        
        public void SetUnit(Unit unit)
        {
            unitOnBlock = unit;
        }
    }
}