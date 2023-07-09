using System.Collections.Generic;
using _01.Scripts.Cells;
using _01.Scripts.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _01.Scripts.Managers
{
    public class CellManager : MonoBehaviour
    {
        public static Dictionary<Vector2Int, Cell> Cells { get; } = new();
        public void Awake()
        {
            var parentTrm = new GameObject("Cells").transform;
            var cell = Addressables.LoadAssetAsync<GameObject>("Cell");
            cell.Completed += handle =>
            {
                var cellPrefab = handle.Result;

                for (var i = 0; i < 8; i++)
                {
                    for (var j = 0; j < 8; j++)
                    {
                        var cellInstance = Object.Instantiate(cellPrefab, parentTrm);
                        var gridPosition = new Vector2Int(i, j);
                        var worldPosition = Util.GridToPosition(gridPosition);
                        cellInstance.transform.position = worldPosition;
                        cellInstance.name = $"Cell ({((char)(i + 'a')).ToString().ToUpper()}{j + 1})";
                        var cellComponent = cellInstance.GetComponent<Cell>();
                        Cells.Add(gridPosition, cellComponent);
                    }
                }

                GameManager.IsGameStart = true;
            };
        }
    }
}