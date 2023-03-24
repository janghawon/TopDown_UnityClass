using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunScriptBehaviour : StateMachineBehaviour 
{
    private PlayerVFXManager _playerVFXManager;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_playerVFXManager == null)
        {
            _playerVFXManager = animator.transform.parent.GetComponent<PlayerVFXManager>();
        }
        _playerVFXManager.UpdateFootStep(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerVFXManager?.UpdateFootStep(false);
    }
}
