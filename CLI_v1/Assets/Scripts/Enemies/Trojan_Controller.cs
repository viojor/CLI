using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trojan_Controller : EnemyBase_Controller{

    private const float SECONDS_FOR_SPECIAL_ABILITY = 3.0f;
    private const int CORRUPTION_INCREMENT = 20;

    private float seconds_passed = 0;

    //[SerializeField]
    //private Sprite hidden_troyan_sprite = null;
    //private Sprite base_troyan_sprite;

    private CircleCollider2D body_collider;
    private SpriteRenderer sprite_renderer;

    private Animator animator;

    protected override void UseSpecialAbility(){

        this.body_collider = GetComponent<CircleCollider2D>();
        this.sprite_renderer = GetComponent<SpriteRenderer>();

        //We need to disable the body collider (circle) and change the sprite of the enemy.
        this.body_collider.enabled = false;
        this.animator.SetBool("isHidden", true);
        //Before changing the sprite we need to save the actual sprite.
        //this.base_troyan_sprite = this.sprite_renderer.sprite;
        //this.sprite_renderer.sprite = hidden_troyan_sprite;

        StartCoroutine(this.ReturnNormalStateCoroutine());
    }

    protected override void Start(){

        base.Start();

        this.animator = this.GetComponent<Animator>();

        //We set the max value of the special ability bar.
        base.special_ability_bar.SetMaxValue(SECONDS_FOR_SPECIAL_ABILITY);
        //We call the coroutine of the special ability.
        StartCoroutine(this.SpecialAbilityCoroutine());
    }

    private IEnumerator SpecialAbilityCoroutine(){

        yield return new WaitForSeconds(SECONDS_FOR_SPECIAL_ABILITY);

        this.UseSpecialAbility();
    }

    private IEnumerator ReturnNormalStateCoroutine(){

        yield return new WaitForSeconds(SECONDS_FOR_SPECIAL_ABILITY);

        //We have to enable the collider again and change the sprite to its normal state.
        this.body_collider.enabled = true;
        this.animator.SetBool("isHidden", false);
        //this.sprite_renderer.sprite = this.base_troyan_sprite;

        StartCoroutine(this.SpecialAbilityCoroutine());
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
