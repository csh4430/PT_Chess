using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _01.Scripts.Decals;
using _01.Scripts.Managers;
using _01.Scripts.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _01.Scripts.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int gridPosition = Vector2Int.zero;
        public Vector2Int GridPosition => gridPosition;
        
        [SerializeField] private bool isEnemy = false;
        public bool IsEnemy => isEnemy;

        [SerializeField]
        private bool isSelected = false;
        
        public bool IsSelected => isSelected;
        
        [SerializeField]
        private bool isMoving = false;
        
        public event Action OnSelected = null;
        public event Action DeSelected = null;
        
        private GameObject decalPrefab = null;
        
        private SpriteRenderer spriteRenderer = null;
        
        private Dictionary<Vector2, GameObject> _decals = new Dictionary<Vector2, GameObject>();
        private Dictionary<Vector2, GameObject> _killDecals = new Dictionary<Vector2, GameObject>();


        private void Awake()
        {
            OnSelected += OnSelect;
            DeSelected += DestroyDecals;
            GameManager.Units.Add(this);
            var decal = Addressables.LoadAssetAsync<GameObject>("Decal");
            decal.Completed += handle =>
            {
                decalPrefab = handle.Result;
            };
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.IsGameStart);
            transform.position = Util.GridToPosition(GridPosition);
            Define.GetCell(gridPosition).SetUnit(this);
        }

        private void Update()
        {
            if (isSelected)
            {
                CreateDecals();
                CreateKillDecal();
            }
            else DeSelected?.Invoke();
        }

        public void Init()
        {
            gridPosition = Util.PositionToGrid(transform.position);
            transform.position = Util.GridToPosition(GridPosition);
            Define.GetCell(gridPosition).SetUnit(this);
            isMoving = false;
            spriteRenderer.sortingOrder = 0;
        }

        private void OnMouseDown()
        {
            Select();
        }

        public void Select()
        {
            OnSelected?.Invoke();
        }

        protected virtual bool CheckEnemy(out List<Unit> enemyUnits)
        {
            enemyUnits = new List<Unit>();
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (Mathf.Abs(i) != Mathf.Abs(j)) continue;
                    var gridPosition = GridPosition + new Vector2Int(i, j);
                    var unit = Define.GetUnit(gridPosition);
                        
                    if(unit != null && unit.IsEnemy != isEnemy)
                    {
                        enemyUnits.Add(unit);
                    }
                }
            }

            return enemyUnits.Count > 0;
        }

        protected virtual void OnSelect()
        {
            if (isMoving) return;
            isSelected = !isSelected;
        }

        public virtual void DeSelect()
        {
            isSelected = false;
            DeSelected?.Invoke();
        }

        protected virtual void CreateDecals()
        {
            
            Transform decalTrm;
            if(transform.Find("Decals"))
            {
                decalTrm = transform.Find("Decals");
            }
            else
                decalTrm = new GameObject("Decals").transform;
            decalTrm.SetParent(transform);
            GameObject decalInstance = null;
            Decal decalComponent = null;
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (Mathf.Abs(i) == Mathf.Abs(j))
                    {
                        continue;
                    }
                        
                    var gridPosition = GridPosition + new Vector2Int(i, j);
                    if (Define.GetCell(gridPosition) == null)
                    {
                        if (_decals.ContainsKey(gridPosition))
                        {
                            Destroy(_decals[gridPosition]);
                            _decals.Remove(gridPosition);
                        }
                        continue;
                    }
                    if(Define.GetUnit(gridPosition)) 
                    {
                        if (_decals.ContainsKey(gridPosition))
                        {
                            Destroy(_decals[gridPosition]);
                            _decals.Remove(gridPosition);
                        }
                        continue;
                    }
                    if (_decals.ContainsKey(gridPosition)) continue;
                    decalInstance = Instantiate(decalPrefab, decalTrm);
                    decalComponent = decalInstance.GetComponent<Decal>();
                    decalComponent.Init(this, gridPosition, false, !isEnemy);
                    _decals.Add(gridPosition, decalInstance);
                }
            }
        }

        protected virtual void CreateKillDecal()
        {
            Transform decalTrm;
            if(transform.Find("KillDecals"))
            {
                decalTrm = transform.Find("KillDecals");
            }
            else
                decalTrm = new GameObject("KillDecals").transform;
            decalTrm.SetParent(transform);
            GameObject decalInstance = null;
            Decal decalComponent = null;

            if (!CheckEnemy(out var enemyUnits))
            {
                foreach (var decal in _killDecals)
                {
                    Destroy(decal.Value);
                }
                _killDecals.Clear();
                return;
            }
            foreach (var enemyUnit in enemyUnits)
            {
                if (_killDecals.ContainsKey(enemyUnit.GridPosition)) continue;
                decalInstance = Instantiate(decalPrefab, decalTrm);
                decalComponent = decalInstance.GetComponent<Decal>();
                decalComponent.Init(this, enemyUnit.GridPosition, true, !isEnemy);
                _killDecals.Add(enemyUnit.GridPosition, decalInstance);
            }
            
        }

        protected virtual void DestroyDecals()
        {
            var decals = transform.Find("Decals");
            var killDecals = transform.Find("KillDecals");
            if (decals == null) return;
            if (killDecals == null) return;
            _decals.Clear();
            _killDecals.Clear();
            Destroy(killDecals.gameObject);
            Destroy(decals.gameObject);
        }

        public void Move()
        {
            isMoving = true;
            spriteRenderer.sortingOrder = 1;
        }

        public void Move(Vector2 pos)
        {
            var allDecals = _decals.Concat(_killDecals);
            var nextDecal = allDecals.FirstOrDefault((x) => x.Key == pos).Value;
            if (nextDecal == null) return;
            var decal = nextDecal.GetComponent<Decal>();
            if (decal == null) return;
            decal.Move();
        }
    }
}