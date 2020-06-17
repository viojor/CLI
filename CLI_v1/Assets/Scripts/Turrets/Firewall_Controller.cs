using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall_Controller : TurretBase_Controller{

    protected override string Turret_type {

        get => "Firewall";
        set => this.Turret_type = "Firewall";
    }

    protected override float Fire_rate{

        get => 1.5f;
        set => base.Fire_rate = 1.5f;
    }
    
    public override int Turret_cost {

        get => 20;
        set => this.Turret_cost = 20;
    }
}
