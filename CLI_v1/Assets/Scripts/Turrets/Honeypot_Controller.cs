using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honeypot_Controller : TurretBase_Controller{

    protected override string Turret_type{

        get => "Honeypot";
        set => base.Turret_type = "Honeypot";
    }

    protected override float Fire_rate{

        get => 1.5f;
        set => base.Fire_rate = 1.5f;
    }

    public override int Turret_cost{

        get => 60;
        set => this.Turret_cost = 60;
    }
}

