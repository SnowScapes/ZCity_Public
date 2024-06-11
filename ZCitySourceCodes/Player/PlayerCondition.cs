using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerStatHandler _statHandler;
    
    public float MaxHunger;
    public float CurHunger;
    public float MaxStamina;
    public float CurStamina;
    public float MaxWater;
    public float CurWater;

    [SerializeField] private float StaminaConsume;
    [SerializeField] private float StaminaRestore;
    [SerializeField] private float HungerConsume;
    [SerializeField] private float WaterConsume;
    [SerializeField] private float HpConsume;

    private WaitForSeconds reduceDelay = new WaitForSeconds(1f);
    
    private void Start()
    {
        InitCondition();
        StartCoroutine(ReduceWater());
        StartCoroutine(ReduceHunger());
    }

    private void Update()
    {
        if (_controller.SprintMode && _controller.moveInput.magnitude != 0)
        {
            CurStamina -= StaminaConsume * Time.deltaTime;
            if (CurStamina <= 0)
                _controller.SprintMode = false;
        }
        else if (!_controller.SprintMode && CurStamina < MaxStamina)
        {
            CurStamina = CurStamina > MaxStamina ? MaxStamina : CurStamina + StaminaRestore * Time.deltaTime;
        }
    }

    private void InitCondition()
    {
        CurHunger = MaxHunger;
        CurStamina = MaxStamina;
        CurWater = MaxWater;
    }

    private IEnumerator ReduceHunger()
    {
        while (true)
        {
            CurHunger = CurHunger > 0 ? CurHunger - HungerConsume : _statHandler.currentHP - HpConsume;
            if (CurHunger < 0)
                CurHunger = 0;
            yield return reduceDelay;
        }
    }

    private IEnumerator ReduceWater()
    {
        while (true)
        {
            CurWater = CurWater > 0 ? CurWater - WaterConsume : _statHandler.currentHP - HpConsume;
            if (CurWater < 0)
                CurWater = 0;
            yield return reduceDelay;
        }
    }

    public void IncreaseHugner(float value)
    {
        CurHunger += value;
        if (CurHunger > MaxHunger)
            CurHunger = MaxHunger;
    }
    
    public void IncreaseWater(float value)
    {
        CurWater += value;
        if (CurWater > MaxWater)
            CurWater = MaxWater;
    }
}
