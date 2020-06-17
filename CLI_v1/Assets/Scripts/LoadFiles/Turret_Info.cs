using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turret_Info{

    public string TurretName { get; set; }
    public int AttacksNumber { get; set; }
    public int CorruptionPercent { get; set; }
    public int AttackSpeed { get; set; }
    public int Gold { get; set; }
    public int BuildDelay { get; set; }
    public int AttacksDamage { get; set; }
}
