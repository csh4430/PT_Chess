using System.Collections.Generic;
using System.Linq;
using _01.Scripts.Cells;
using _01.Scripts.Managers;
using _01.Scripts.Units;
using JetBrains.Annotations;
using UnityEngine;

namespace _01.Scripts.Utils
{
    public static class Define
    {
        public static Cell GetCell(Vector2Int gridPosition)
        {
            if(CellManager.Cells.TryGetValue(gridPosition, out var cell))
            {
                return cell;
            }

            return null;
        }
        
        public static Unit GetUnit(Vector2Int gridPosition)
        {
            return GameManager.Units.FirstOrDefault(unit => unit.GridPosition == gridPosition);
        }
    }
}