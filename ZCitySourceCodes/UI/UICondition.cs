using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    public Image health;
    public Image stamina;
    public Image hunger;
    public Image water;

    [SerializeField] private PlayerCondition _condition;
    [SerializeField] private PlayerStatHandler _playerStatHandler;

    private void Update()
    {
        hunger.fillAmount = _condition.CurHunger / _condition.MaxHunger;
        water.fillAmount = _condition.CurWater / _condition.MaxWater;
        stamina.fillAmount = _condition.CurStamina / _condition.MaxStamina;

        // Calculate the percentage of current health over max health
        float healthPercentage = (float)_playerStatHandler.currentHP / _playerStatHandler.stat.HP;
        health.fillAmount = healthPercentage;
    }
}
