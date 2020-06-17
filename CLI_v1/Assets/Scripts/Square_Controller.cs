using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square_Controller : MonoBehaviour{

    [SerializeField]
    private Sprite[] array_square_sprite = null;
    private SpriteRenderer sprite_renderer;
    
    private GameObject turret;

    private SquareStates squareState = SquareStates.Empty;
    public enum SquareStates{

        Empty,
        Path,
        Turret
    }

    private void Start(){

        int sprite_index = Random.Range(0, this.array_square_sprite.Length);
        
        this.sprite_renderer = GetComponent<SpriteRenderer>();
        if(this.sprite_renderer != null){

            this.sprite_renderer.sprite = this.array_square_sprite[sprite_index];
        }
    }

    public void DeleteSquaresTurret(){

        this.turret.GetComponent<TurretBase_Controller>().DestroyTurret();
    }

    public void SetSquaresTurret(GameObject new_turret){

        this.turret = new_turret;
    }

    public GameObject GetSquaresTurret(){

        return this.turret;
    }    

    public SquareStates GetSquareState(){

        return this.squareState;
    }

    public void SetSquareState(SquareStates new_state){

        this.squareState = new_state;
    }
}
