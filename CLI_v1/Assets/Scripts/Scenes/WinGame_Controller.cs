using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame_Controller : MonoBehaviour{

    public void ChangeToMainMenu(){

        Scene_Controller.ChangeScene(Scene_Controller.MAIN_MENU);
    }
}
