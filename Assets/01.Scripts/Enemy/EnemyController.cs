using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    protected CommonAIState _currentState;

    [SerializeField]
    protected EnemyDataSO _enemyData;
    public EnemyDataSO EnemyData => _enemyData;

    protected EnemyHealth _enemyHealth;
    public EnemyHealth EnemyHealth => _enemyHealth;

    private Transform _targetTrm;
    public Transform TargetTrm => _targetTrm;

    private NavAgentMovement _navMovement;
    public NavAgentMovement NavMovement => _navMovement;

    private AgentAnimator _agentAnimator;  //1
    public AgentAnimator AgentAnimator => _agentAnimator; //2

    private EnemyVFXManager _vfxManager;
    public EnemyVFXManager VFXManager => _vfxManager;

    protected virtual void Awake()
    {
        List<CommonAIState> states = new List<CommonAIState>();
        transform.Find("AI").GetComponentsInChildren<CommonAIState>(states);

        //�� ������Ʈ�� ���� �¾��� ���⼭ �� ����
        states.ForEach(s => s.SetUp(transform));

        _navMovement = GetComponent<NavAgentMovement>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>(); //3
        _vfxManager = GetComponent<EnemyVFXManager>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    protected virtual void Start()
    {
        _targetTrm = GameManager.Instance.PlayerTrm;
        //���߿� ���� ������ ���Ǿ�� ���氡���ϴ�.

        _navMovement.SetInitData(_enemyData.MoveSpeed);
        _enemyHealth.SetMaxHP(_enemyData.MaxHP);
    }
    
    public void ChangeState(CommonAIState nextState)
    {
        //������Ʈ ü���� 
        _currentState?.OnExitState();
        _currentState = nextState;
        _currentState?.OnEnterState();
    }

    void Update()
    {
        if (_enemyHealth.IsDead) return;
        _currentState?.UpdateState();
    }

    public UnityEvent OnAfterDead = null;

    public void SetDead()
    {
        _navMovement.StopNavigation();
        _agentAnimator.StopAnimator(true);
        _navMovement.KnockBack(() =>
        {
            _agentAnimator.StopAnimator(false);
            _agentAnimator.SetDead();
            UtilMono.Instance.AddDelayCoroutine(() => {
                OnAfterDead?.Invoke();
            }, 1f);
        });
    }
}
