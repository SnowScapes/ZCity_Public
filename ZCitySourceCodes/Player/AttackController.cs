using UnityEngine;

public class AttackController : StateMachineBehaviour
{
    private MeshCollider bat;
    private bool first = true;
    private AttackControllerMono attackMono;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (first)
        {
            attackMono = animator.GetComponent<AttackControllerMono>();
            bat = attackMono.batCollider;
            first = false;
        }
        
        attackMono.StartCoroutine(attackMono.ColliderSet(bat));
    }
}