using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatHandler : MonoBehaviour, IDamage
{
    public PlayerStat stat;

    [field:SerializeField] public int currentHP { get; set; }
    
    private void Start()
    {
        InitStat();
    }

    private void InitStat()
    {
        currentHP = stat.HP;
    }
    
    public void HandleDamage(float damage)
    {
        currentHP -= (int)damage;
        AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.Hit);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.Die);
        GetComponent<PlayerAnimation>().DieAnim();
        GetComponent<PlayerInput>().enabled = false;
        GameManager.Instance.GameOver();
    }

    public void IncreaseHP(int value)
    {
        currentHP += value;
        if (currentHP > stat.HP)
            currentHP = stat.HP;
    }
}
