using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antivirus_Controller : TurretBase_Controller{

    protected override string Turret_type{

        get => "Antivirus";
        set => base.Turret_type = "Antivirus";
    }

    protected override float Fire_rate{

        get => 2.0f;
        set => base.Fire_rate = 2.0f;
    }
    
    public override int Turret_cost{

        get => 30;
        set => this.Turret_cost = 30;
    }
}
