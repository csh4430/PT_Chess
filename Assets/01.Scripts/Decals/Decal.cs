using System;
using _01.Scripts.Cells;
using _01.Scripts.Managers;
using _01.Scripts.Units;
using _01.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _01.Scripts.Decals
{
    public class Decal : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int gridPosition = Vector2Int.zero;
        public Vector2Int GridPosition => gridPosition;
        
        [SerializeField]
        private Cell cell = null;
        [SerializeField]
        private Unit owner = null;
        
        [SerializeField]
        private bool canKill = false;
        
        private GameObject _breakParticle = null;
        
        private bool _canTouch = true;

        private void Awake()
        {
            var particle = Addressables.LoadAssetAsync<GameObject>("Particles/Break");
            particle.Completed += handle =>
            {
                _breakParticle = handle.Result;
            };
        }

        public void Init(Unit owner, Vector2Int gridPosition, bool canKill, bool canTouch = true)
        {
            this.owner = owner;
            this.gridPosition = gridPosition;
            transform.position = Util.GridToPosition(gridPosition);
            cell = Define.GetCell(gridPosition);
            this.canKill = canKill;
            _canTouch = canTouch;
        }

        private void OnMouseDown()
        {
            if (_canTouch == false) return;
            Move();
        }

        public void Move()
        {
            owner.Move();
            var unit = Define.GetUnit(Util.PositionToGrid(cell.transform.position));
            if (owner.IsEnemy)
            {
                owner.transform.DOScaleY(1.25f, 0.5f).OnComplete(() =>
                {
                    owner.transform.DOScaleY(1f, 0.5f).OnComplete(() =>
                    {
                        owner.transform.DOJump(cell.transform.position, 1f, 1, 0.5f).OnComplete(() =>
                         {
                            owner.Init();
                            if (!canKill) return;
                            var particle = Instantiate(_breakParticle, owner.transform.position, Quaternion.Euler(-90, 0, 0));
                            particle.GetComponent<Renderer>().material.SetColor("_Color", unit.IsEnemy ? Util.BlackColor : Util.WhiteColor);
                            GameManager.Units.Remove(unit);
                            Destroy(unit.gameObject);
                        });
                    });
                });

                owner.DeSelect();
            }
            else
            {
                owner.transform.DOJump(cell.transform.position, 1f, 1, 0.5f).OnComplete(() =>
                {
                    owner.Init();
                    if (!canKill) return;
                    var particle = Instantiate(_breakParticle, owner.transform.position, Quaternion.Euler(-90, 0, 0));
                    particle.GetComponent<Renderer>().material.SetColor("_Color", unit.IsEnemy ? Util.BlackColor : Util.WhiteColor);
                    unit.Stop();
                    GameManager.Units.Remove(unit);
                    Destroy(unit.gameObject);
                });
                owner.DeSelect();
            }
        }
    }
}