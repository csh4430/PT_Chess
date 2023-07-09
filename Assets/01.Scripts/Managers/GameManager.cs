using System.Collections.Generic;
using _01.Scripts.Units;
using UnityEngine;

namespace _01.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsGameStart = false;
        
        public static List<Unit> Units { get; } = new();
    }
}
