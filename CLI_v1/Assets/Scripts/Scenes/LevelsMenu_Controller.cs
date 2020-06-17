using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Newtonsoft.Json.Linq;

public class LevelsMenu_Controller : MonoBehaviour{

    public static Level_Info info_level_selected;

    private Level_Info[] array_levels;

    private void Start(){
        
        this.array_levels = JsonParser.FromJson<Level_Info>(Resources.Load<TextAsset>("levelsInfo").text);
    }

    public void LoadLevelInfo(int levelSelected){
        
        info_level_selected = this.array_levels[levelSelected - 1];
        Scene_Controller.ChangeScene(Scene_Controller.LEVEL);
    }
    
    private void Awake(){

        DontDestroyOnLoad(gameObject);
    }
}
