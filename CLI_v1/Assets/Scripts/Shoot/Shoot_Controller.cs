using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Controller : MonoBehaviour{

    private const float MIN_DISTANCE_SHOOT_TARGET = 0.01f;

    [SerializeField]
    private float shoot_Speed = 4.0f;
    [SerializeField]
    private int damage = 1;

    private Vector3 target_Position;
    
    public void SetTargetPosition(Vector3 targetPosition){

        this.target_Position = targetPosition;
    }
    
    private void Update(){

        transform.position = Vector3.MoveTowards(transform.position, this.target_Position, this.shoot_Speed * Time.deltaTime);
        //If the shoot arrive to the target position but the enemy isnt already there, we have to destroy the instance.
        if (Vector3.Distance(transform.position, this.target_Position) < MIN_DISTANCE_SHOOT_TARGET){ 

            this.DestroyShoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){

        //The shoot can only collision with the collider for the body (circle), not with the collider used for the special ability (box).
        if (collision.tag.Equals("Enemy") && this.IsBodyCollider(collision)){

            //Before destroying the instance we have to decrease the live of the enemy.
            EnemyBase_Controller enemy_controller = collision.GetComponent<EnemyBase_Controller>();
            enemy_controller.GotHitByAShoot(this.damage);

            this.DestroyShoot();
        }
    }

    private bool IsBodyCollider(Collider2D collision) {

        return collision.GetType() == typeof(CircleCollider2D);
    }

    private void DestroyShoot(){

        GameObject.Destroy(gameObject);
    }
}
