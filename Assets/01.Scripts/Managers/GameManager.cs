using System.Collections.Generic;
using _01.Scripts.Units;
using _01.Scripts.Units.AI;
using UnityEngine;

namespace _01.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsGameStart = false;
        
        public static List<Unit> Units { get; } = new();
    
        public static List<GameObject> enemyList = new List<GameObject>();

        public GameObject clearPanel;

        private void Awake()
        {
            var enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemys)
            {
                enemyList.Add(enemy);
            }
        }

        private void Update()
        {
            bool isDie = true;
            foreach(var enemy in enemyList)
            {
                if (enemy != null) isDie = false;
            }
            if (isDie == true)
                clearPanel.SetActive(true);
        }
    }
}
