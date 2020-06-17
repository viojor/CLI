using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggWorm_Controller : EnemyBase_Controller{

    private const float SECONDS_FOR_SPECIAL_ABILITY = 2.0f;

    private float seconds_passed = 0;

    [SerializeField]
    private GameObject worm_go = null;

    private CircleCollider2D body_collider;

    protected override void UseSpecialAbility(){

        GameObject worm = GameObject.Instantiate(worm_go, this.transform.position, Quaternion.identity);
        worm.GetComponent<Worm_Controller>().SetPathIndex(this.GetPathIndex());
        base.DestroyEnemy();
    }

    protected override void Start(){

        this.body_collider = GetComponent<CircleCollider2D>();

        //We need to disable the body collider (circle).
        this.body_collider.enabled = false;

        //We set the max value and the starting value of the special ability bar.
        base.special_ability_bar.SetMaxValue(SECONDS_FOR_SPECIAL_ABILITY);
        base.special_ability_bar.SetCurrentValue(0);

        //We call the coroutine of the special ability.
        StartCoroutine(this.SpecialAbilityCoroutine());
    }

    private IEnumerator SpecialAbilityCoroutine(){

        yield return new WaitForSeconds(SECONDS_FOR_SPECIAL_ABILITY);

        this.UseSpecialAbility();
    }

    protected override void Update(){
        
        //We need to update the value of the special ability bar.
        this.seconds_passed = this.seconds_passed + Time.deltaTime;
        if (this.seconds_passed > SECONDS_FOR_SPECIAL_ABILITY){

            this.seconds_passed = 0;
        }
        base.special_ability_bar.SetCurrentValue(this.seconds_passed);
    }
}
