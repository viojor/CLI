using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretBase_Controller : MonoBehaviour{

    private const float MAX_CORRUPTION_PERCENT = 100.0f;
    private const float CORRUPTION_PERCENT_INCREASE = 20.0f;

    [SerializeField]
    private GameObject shoot_go = null;
    private Queue<GameObject> target_enemies_queue = new Queue<GameObject>();
    private GameObject current_target = null;
    
    protected virtual float Fire_rate { get; set; }
    private float next_fire = 0.0f;
    
    private bool is_corrupted = false;
    private float corruption_percent = 0.0f;

    public virtual int Turret_cost { get; set; }

    protected virtual string Turret_type { get; set; }
    private string turret_position = ""; 

    public Bar_Controller corruption_bar;

    [SerializeField]
    private Animator child_thunder_animator = null;

    private void Start(){

        //Initially, the corruption bar has to be empty.
        this.corruption_bar.SetMaxValue(MAX_CORRUPTION_PERCENT);
        this.corruption_bar.SetCurrentValue(this.corruption_percent);
    }

    private void OnTriggerEnter2D(Collider2D collision){

        //If an enemy enter in the turret's range we have to add it to the targets queue.
        if (collision.tag.Equals("Enemy")){
            //We only had the enemy to the queue if the collider is the collider for the body (circle), not the collider for the special ability.
            if (this.IsBodyCollider(collision)){
                
                this.target_enemies_queue.Enqueue(collision.gameObject);
                this.SetEnemyTarget();
            }
            //If the collider is the collider for the special ability (box) we need to increase the corruption percent.
            else if (this.IsAbilityCollider(collision)){

                this.CorruptTurret(CORRUPTION_PERCENT_INCREASE);
            }
        }
    }

    private bool IsBodyCollider(Collider2D collision){

        return collision.GetType() == typeof(CircleCollider2D);
    }

    private bool IsAbilityCollider(Collider2D collision){

        return collision.GetType() == typeof(BoxCollider2D);
    }

    private void SetEnemyTarget(){

        if(this.IsTargetsQueueNotEmpty()){

            this.current_target = this.target_enemies_queue.Peek();
        }
    }

    private bool IsTargetsQueueNotEmpty(){

        return this.target_enemies_queue.Count > 0;
    }

    private void ChangeSpriteColorCorruption(){

        //We have to change the color of the turret's sprite when the corruption percent increase.
        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = new Color(0.6f, 0.6f, 0.9f);
        
        this.child_thunder_animator.SetBool("isBeingCorrupted", true);

        //If the turret is not fully corrupted we have to change the sprite to its normal color.
        if(this.corruption_percent < 100.0f){
            
            StartCoroutine(this.ReturnSpriteNormalColorCoroutine());
        }
    }

    private IEnumerator ReturnSpriteNormalColorCoroutine(){

        yield return new WaitForSeconds(0.2f);

        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = new Color(1f, 1f, 1f);
    }

    public void CorruptTurret(float corruption_amount){

        this.IncreaseCorruptionPercent(corruption_amount);
        this.ChangeSpriteColorCorruption();
    }

    private void IncreaseCorruptionPercent(float corruption_amount){

        //We increase the corruption percent.
        this.corruption_percent = this.corruption_percent + corruption_amount;
        //We need to update the corruption bar.
        this.corruption_bar.SetCurrentValue(this.corruption_percent);
        //If the new percent is equal or higher to the max percent our turret will be corrupted.
        if (this.corruption_percent >= MAX_CORRUPTION_PERCENT){

            //We maintain the percent to the maximum (cant be higher). 
            this.corruption_percent = MAX_CORRUPTION_PERCENT;
            this.is_corrupted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        
        //If an enemy die or get out of the turret's range we have to eliminate it from the targets queue (only count the body collider).
        if (collision.tag.Equals("Enemy") && this.IsBodyCollider(collision)){

            this.ChangeEnemyTarget();
        }
    }

    private void ChangeEnemyTarget(){

        if (this.IsTargetsQueueNotEmpty()){ //Check if the queue is empty.

            this.target_enemies_queue.Dequeue();
            this.SetEnemyTarget();
        }
    }

    private void Update(){
        
        //We will be able to shoot if our queue of enemies is not empty OR the turret is not corrupted.
        if(this.IsTargetsQueueNotEmpty() && this.is_corrupted == false){

            this.Shoot();
        }
    }

    protected void Shoot(){

        if (this.TimeBetweenShootsPassed()){ //Delay between shoots.

            float turret_x = transform.position.x;
            float turret_y = transform.position.y;
            
            //We create the shoot instance.
            GameObject shoot_instance = Instantiate(this.shoot_go, transform.position, Quaternion.identity);
            Shoot_Controller shoot_Controller = shoot_instance.GetComponent<Shoot_Controller>();
            if (shoot_Controller != null && this.current_target != null){

                shoot_Controller.SetTargetPosition(this.current_target.transform.position);
            }
            //The turret will not be able to shoot during an amount of time equals to the value of fireRate.
            this.next_fire = Time.time + this.Fire_rate;
        }
    }

    private bool TimeBetweenShootsPassed(){

        return Time.time > this.next_fire;
    }

    public void DestroyTurret(){

        GameObject.Destroy(gameObject);
    }

    public string GetTurretType(){

        return this.Turret_type;
    }

    public void SetTurretPosition(char[] array_char_turret_position){

        string turret_position = "";
        for(int i = 0; i < array_char_turret_position.Length; i++){

            turret_position = turret_position + char.ToUpper(array_char_turret_position[i]);
        }
        this.turret_position = turret_position;
    }

    public string GetTurretInfo(){

        string info = "";
        info = info + "Turret (Type: " + this.Turret_type + ", Position: " + this.turret_position + "): " + this.corruption_percent + "%";

        return info;
    }
}
