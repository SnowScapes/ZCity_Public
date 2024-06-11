using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    [field:SerializeField] public int HP { get; set; }
    [field:SerializeField] public int Power { get; set; }
    [field:SerializeField] public float AtkDelay { get; set; }
    [field:SerializeField] public int WalkSpeed { get; set; }
    [field:SerializeField] public int SprintSpeed { get; set; }
    [field:SerializeField] public int SkillPoint { get; set; }
    [field:SerializeField] public float FieldOfView { get; set; }
    [field:SerializeField] public float ViewRange { get; set; }
    [field:SerializeField] public float AroundViewRange { get; set; }
}
