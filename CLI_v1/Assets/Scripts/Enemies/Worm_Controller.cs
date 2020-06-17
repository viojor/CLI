using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm_Controller : EnemyBase_Controller{

    private const float SECONDS_FOR_SPECIAL_ABILITY = 3.0f;
    private const int CORRUPTION_INCREMENT = 40;

    private float seconds_passed = 0;

    [SerializeField]
    private GameObject worm_egg_go = null;

    protected override void UseSpecialAbility(){
        
        //Increase the total enemies count cause a new worm has been created.
        base.ui_controller.IncreaseEnemiesCount(1);

        GameObject worm_egg = GameObject.Instantiate(worm_egg_go, this.transform.position, Quaternion.identity);
        worm_egg.GetComponent<EggWorm_Controller>().SetPathIndex(this.GetPathIndex());
    }

    protected override void Start(){

        base.Start();

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

    protected override int Corruption_percent_increase {

        get => CORRUPTION_INCREMENT;
        set => base.Corruption_percent_increase = CORRUPTION_INCREMENT;
    }
}
