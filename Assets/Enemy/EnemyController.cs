using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CommonAIState _currentState;

    [SerializeField] protected EnemyDataSO _enemyData;
    public EnemyDataSO EnemyData => _enemyData;

    protected EnemyHealth _enemyHealth;
    public EnemyHealth EnemyHealth => _enemyHealth;

    private Transform _targetTrm;
    public Transform TargetTrm => _targetTrm;

    private NavAgentMovement _navMovement;
    public NavAgentMovement NavMovement => _navMovement;

    private AgentAnimator _agentAnimator;
    public AgentAnimator AgentAnimator => _agentAnimator;

    private EnemyVFXManager _vfxManager;
    public EnemyVFXManager VFXManager => _vfxManager;

    protected virtual void Awake()
    {
        List<CommonAIState> states = new List<CommonAIState>();
        transform.Find("AI").GetComponentsInChildren<CommonAIState>(states);

        states.ForEach(s => s.SetUp(transform));
        _navMovement = GetComponent<NavAgentMovement>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _vfxManager = GetComponent<EnemyVFXManager>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    protected virtual void Start()
    {
        _targetTrm = GameManager.Instance.PlayerTrm;

        _navMovement.SetInitData(_enemyData.MoveSpeed);
        _enemyHealth.SetMaxHP(_enemyData.MaxHP);
    }

    public void ChangeState(CommonAIState nextState)
    {
        _currentState?.OnExitState();
        _currentState = nextState;
        _currentState?.OnEnterState();
    }

    private void Update()
    {
        _currentState?.UpdateState();
    }
}
