using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroy : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.gameObject.TryGetComponent(out Unit unit))
        {
            unit.Disable();
        }
        else
        {
            Destroy(animator.transform.gameObject);
        }
    }
}
