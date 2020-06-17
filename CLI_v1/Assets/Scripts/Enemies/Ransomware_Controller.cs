using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ransomware_Controller : EnemyBase_Controller{

    private const float SECONDS_FOR_SPECIAL_ABILITY = 7.0f;
    private const int CORRUPTION_INCREMENT = 10;

    private float seconds_passed = 0;

    private MapGenerator map_instance;

    protected override void UseSpecialAbility(){

        List<GameObject> turrets_list = this.map_instance.GetAllTurrets();
        if (turrets_list.Count > 0){
            
            int random_index = Random.Range(0, turrets_list.Count);

            GameObject turret_target = turrets_list[random_index];
            turret_target.GetComponent<TurretBase_Controller>().CorruptTurret(100.0f);
        }
    }

    protected override void Start(){

        base.Start();
        
        this.map_instance = MapGenerator.GetMapInstance();

        //We set the max value of the special ability bar.
        base.special_ability_bar.SetMaxValue(SECONDS_FOR_SPECIAL_ABILITY);
        //We call the coroutine of the special ability.
        StartCoroutine(this.SpecialAbilityCoroutine());
    }

    private IEnumerator SpecialAbilityCoroutine(){

        yield return new WaitForSeconds(SECONDS_FOR_SPECIAL_ABILITY);

        this.UseSpecialAbility();
    }

    protected override void Update(){

        base.Update();

        //We need to update the value of the special ability bar.
        this.seconds_passed = this.seconds_passed + Time.deltaTime;
        if (this.seconds_passed > SECONDS_FOR_SPECIAL_ABILITY){

            this.seconds_passed = 0;
        }
        base.special_ability_bar.SetCurrentValue(this.seconds_passed);
    }

    protected override int Corruption_percent_increase{

        get => CORRUPTION_INCREMENT;
        set => base.Corruption_percent_increase = CORRUPTION_INCREMENT;
    }
}
