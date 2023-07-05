using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EnemyDropper : MonoBehaviour
{
    [SerializeField] private LayerMask _WhatIsGround;
    [SerializeField] private float _maxDistance = 20f;
    public UnityEvent OnDropComplete = null;
    private EnemyController _enemyController;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    public void ReadyToDrop(Vector3 pos)
    {
        SetNavInfo(false);
        transform.position = pos;
    }

    private void SetNavInfo(bool value)
    {
        _enemyController.NavMovement.enabled = value;
        _enemyController.NavMovement.NavAgent.updatePosition = value;
        _enemyController.NavMovement.NavAgent.updateRotation = value;
        _enemyController.NavMovement.NavAgent.enabled = value;
    }

    public bool Drop()
    {
        Vector3 currentPosition = transform.position;
        RaycastHit hit;

        bool isHit = Physics.Raycast(currentPosition, Vector3.down, out hit, _maxDistance, _WhatIsGround);

        if(isHit)
        {
            DropDecal decal = PoolManager.Instance.Pop("DropDecal") as DropDecal;
            decal.transform.position = hit.point + new Vector3(0, 2, 0);
            decal.SetUpSize(new Vector3(3, 3, 4));

            decal.StartSequence(() =>
            {
                decal.FadeOut(0.2f);
                transform.DOMove(hit.point, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    SetNavInfo(true);
                    OnDropComplete?.Invoke();
                });
                
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ReadyToDrop(new Vector3(6, 0,3));
            Drop();
        }
    }
}
