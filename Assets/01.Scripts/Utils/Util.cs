using UnityEngine;

namespace _01.Scripts.Utils
{
    public static class Util
    {
        public static Vector3 GridToPosition(Vector2Int gridPosition)
        {
            return new Vector2(gridPosition.x, gridPosition.y) * 2.5f;
        }

        public static Vector2Int PositionToGrid(Vector3 position)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x / 2.5f), Mathf.RoundToInt(position.y / 2.5f));
        }
        
        public static Color WhiteColor => new(0.9450981f, 0.8666667f, 0.654902f, 1);
        public static Color BlackColor => new(0.6078432f, 0.3647059f, 0.172549f, 1);
    }
}