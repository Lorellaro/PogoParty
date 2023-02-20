using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnCollision : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string triggerName;

    private void OnCollisionEnter(Collision collision)
    {
        anim.ResetTrigger(triggerName);
        anim.SetTrigger(triggerName);
    }

    public void resetTrigger()
    {
        anim.ResetTrigger(triggerName);
    }
}
