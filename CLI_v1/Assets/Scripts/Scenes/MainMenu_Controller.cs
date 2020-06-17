using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Controller : MonoBehaviour{
    
    public void ChangeToLevelsMenu(){
        
        Scene_Controller.ChangeScene(Scene_Controller.LEVEL_MENU);
    }

    public void ExitGame(){

        Application.Quit();
    }
}
