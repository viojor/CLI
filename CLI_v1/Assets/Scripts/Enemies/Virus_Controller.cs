using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus_Controller : EnemyBase_Controller{

    private const float SECONDS_FOR_SPECIAL_ABILITY = 3.0f;
    private const int CORRUPTION_INCREMENT = 30;

    private float seconds_passed = 0;

    private BoxCollider2D special_ability_collider;

    protected override void UseSpecialAbility(){

        this.special_ability_collider.enabled = true;
        //Once we have used the ability we have to disable, again, the collider.
        StartCoroutine(this.DisableSpecialAbilityColliderCoroutine());
    }

    protected override void Start(){

        base.Start();

        //We set the max value of the special ability bar.
        base.special_ability_bar.SetMaxValue(SECONDS_FOR_SPECIAL_ABILITY);

        //Initially, the collider used for the special ability's range have to be disabled.
        this.special_ability_collider = GetComponent<BoxCollider2D>();
        this.special_ability_collider.enabled = false;

        //We call the coroutine that will enable the collider.
        StartCoroutine(this.EnableSpecialAbilityColliderCoroutine());
    }

    private IEnumerator EnableSpecialAbilityColliderCoroutine(){

        //After X second we need to use the special ability.
        yield return new WaitForSeconds(SECONDS_FOR_SPECIAL_ABILITY);

        this.UseSpecialAbility();
    }

    private IEnumerator DisableSpecialAbilityColliderCoroutine(){

        //After 0.1 s we disable the collider.
        yield return new WaitForSeconds(0.1f);
        
        //We need to restart the counter.
        this.seconds_passed = 0;

        this.special_ability_collider.enabled = false;
        StartCoroutine(this.EnableSpecialAbilityColliderCoroutine());
    }

    protected override void Update(){

        base.Update();

        //We need to update the value of the special ability bar.
        this.seconds_passed = this.seconds_passed + Time.deltaTime;
        if(this.seconds_passed > SECONDS_FOR_SPECIAL_ABILITY){

            this.seconds_passed = 0;
        }
        base.special_ability_bar.SetCurrentValue(this.seconds_passed);
    }

    protected override int Corruption_percent_increase{

        get => CORRUPTION_INCREMENT;
        set => base.Corruption_percent_increase = CORRUPTION_INCREMENT;
    }
}
