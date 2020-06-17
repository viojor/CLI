using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPS_Controller : TurretBase_Controller{

    protected override string Turret_type{

        get => "Ips";
        set => base.Turret_type = "Ips";
    }

    protected override float Fire_rate {

        get => 0.8f;
        set => base.Fire_rate = 0.8f;
    }

    public override int Turret_cost{

        get => 50;
        set => this.Turret_cost = 50;
    }
}
