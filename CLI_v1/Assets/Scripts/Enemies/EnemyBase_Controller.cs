using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase_Controller : MonoBehaviour{

    private const float MIN_DISTANCE_POSITIONS = 0.01f;

    [SerializeField]
    private float enemy_speed = 1.0f;
    private int enemy_lives = 3;

    private List<Transform> path;
    private int path_index = 0;

    protected UI_Controller ui_controller;

    protected virtual int Corruption_percent_increase { get; set; }

    public Bar_Controller health_bar;
    public Bar_Controller special_ability_bar;
    
    //Public
    public void GotHitByAShoot(int damage){
        
        this.ChangeSpriteColorDamage();
        this.DecreaseEnemyLive(damage);
    }

    public void SetPathIndex(int path_index){

        this.path_index = path_index;
    }

    public int GetPathIndex(){

        return this.path_index;
    }

    //Protected
    protected virtual void UseSpecialAbility(){
        
    }

    protected virtual void Start(){

        this.path = MapGenerator.GetMapInstance().GetPath();

        this.ui_controller = GameObject.Find("UI").GetComponent<UI_Controller>();

        //Initially, the health bar has to be full.
        this.health_bar.SetMaxValue(this.enemy_lives);
        this.health_bar.SetCurrentValue(this.enemy_lives);

        //Initially, the special ability bar has to be empty.
        //We need to establish the max value on the children (can change between childrens).
        this.special_ability_bar.SetCurrentValue(0);
    }

    protected virtual void Update(){

        this.MoveEnemy();
    }

    protected void DestroyEnemy(){

        if(this.ui_controller != null){

            this.ui_controller.DecreaseEnemiesCount(1);
        }
        GameObject.Destroy(gameObject);
    }

    //Private
    private void MoveEnemy(){

        if (this.path_index < this.path.Count){

            Transform next_position = this.path[this.path_index];
            transform.position = Vector2.MoveTowards(transform.position, next_position.position, this.enemy_speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, next_position.position) < MIN_DISTANCE_POSITIONS){

                this.path_index = this.path_index + 1;
            }
        }
        else{ //We are at the end of the map.

            this.ui_controller.IncreaseCorruptionPercent(this.Corruption_percent_increase);
            this.DestroyEnemy();
        }
    }

    private void ChangeSpriteColorDamage(){

        //We have to change the color of the enemy's sprite when a shoot hit him.
        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = new Color(1f, 0.6f, 0.6f);

        StartCoroutine(this.ReturnSpriteNormalColorCoroutine());
    }

    private IEnumerator ReturnSpriteNormalColorCoroutine(){

        yield return new WaitForSeconds(0.2f);

        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = new Color(1f, 1f, 1f);
    }

    private void DecreaseEnemyLive(int damage){

        if (this.enemy_lives > damage){

            this.enemy_lives = this.enemy_lives - damage;
            //We have to update the health bar with the current health of the enemy.
            health_bar.SetCurrentValue(this.enemy_lives);
        }
        else{

            this.DestroyEnemy();
        }
    }
}
