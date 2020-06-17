using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LsCommand : CommandBase_Controller{

    private MapGenerator map_instance;
    private InputField IF_Info;
    
    public override string Command_Name{

        get => "ls";
        set => this.Command_Name = "ls";
    }

    public override void ExecuteCommand(string[] command_splited){

        base.ExecuteCommand(command_splited);

        string info_turrets = "";

        this.map_instance = MapGenerator.GetMapInstance();
        this.GetCanvasIF_Info();

        //ls (1).
        if (command_splited.Length == ONE_ELEMENT_COMMAND){
            
            info_turrets = this.map_instance.GetInfoAllTurrets();
            this.IF_Info.text = info_turrets;
        }
        // ls turretType (2).
        else if (command_splited.Length == TWO_ELEMENTS_COMMAND){
            if (this.IsTypeCorrect(command_splited[1])){
                
                info_turrets = this.map_instance.GetInfoOneTypeTurrets(command_splited[1].ToLower());
                this.IF_Info.text = info_turrets;
            }
            else{

                errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.InexistentType);
            }
        }
        else{

            errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.CommandStructInvalid);
        }
    }

    private void GetCanvasIF_Info(){

        InputField[] inputs_fields = GameObject.Find("UI").GetComponentsInChildren<InputField>();
        for (int i = 0; i < inputs_fields.Length; i++){
            if (inputs_fields[i].name.Equals("IF_Info")){

                this.IF_Info = inputs_fields[i];
            }
        }
    }

    private bool IsTypeCorrect(string type_indicated){

        return this.IsValueInArray(type_indicated, base.turrets_type);
    }
}
