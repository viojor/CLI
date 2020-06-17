using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Animator_Controller : StateMachineBehaviour{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo state_info, int layer_index){

        SpriteRenderer thunder_sprite = animator.GetComponent<SpriteRenderer>();
        if (thunder_sprite != null){
            
            thunder_sprite.enabled = true;
        }
    }
}
