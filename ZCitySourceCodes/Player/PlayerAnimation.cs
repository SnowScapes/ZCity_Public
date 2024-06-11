using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerController controller;

    private void Start()
    {
        controller.MoveEvent += MoveAnim;
        controller.AttackEvent += AttackAnim;
        controller.AimEvent += AimAnim;
    }

    private void MoveAnim(float speed)
    {
        _animator.SetFloat("MoveSpeed", speed);
    }

    private void AttackAnim()
    {
        _animator.SetTrigger("Attack");
    }

    private void AimAnim(bool aiming)
    {
        _animator.SetBool("Aim", aiming);
    }

    public void DieAnim()
    {
        _animator.SetTrigger("Die");
    }
}
