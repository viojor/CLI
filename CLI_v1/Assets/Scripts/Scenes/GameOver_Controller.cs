using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_Controller : MonoBehaviour{

    private const float TIME_RESTART = 5.0f; //Wait 5s before changing the scene.

    private Text t_percent;
    private float time_cont;
    private int time_seconds;

    private void Start(){

        this.GetCanvasT_Percent();
        this.t_percent.text = "0%";

        this.time_cont = 0.0f;
        this.time_seconds = 0;
    }

    private void GetCanvasT_Percent(){

        Text[] array_texts = GameObject.Find("UI").GetComponentsInChildren<Text>();
        for (int i = 0; i < array_texts.Length; i++){
            if (array_texts[i].name.Equals("T_Percent")){

                this.t_percent = array_texts[i];
            }
        }
    }

    private void Update(){

        this.time_cont = this.time_cont + Time.deltaTime;
        this.time_seconds = (int)(this.time_cont % 60);
        if (this.time_cont >= TIME_RESTART){

            //Change scene
            Scene_Controller.ChangeScene(Scene_Controller.MAIN_MENU);
        }
        
        this.t_percent.text = (this.time_seconds*20).ToString() + "%";
    }
}
