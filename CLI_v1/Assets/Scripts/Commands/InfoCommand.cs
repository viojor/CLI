using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCommand : CommandBase_Controller{
    
    private InputField IF_Info;

    public override string Command_Name {

        get => "info";
        set => this.Command_Name = "info";
    }
    
    public override void ExecuteCommand(string[] command_splited){

        base.ExecuteCommand(command_splited);

        //info (1).
        if (command_splited.Length == ONE_ELEMENT_COMMAND){

            //We need to find first the IF_Info.
            this.GetCanvasIF_Info();

            string turrets_info = "";
            Turret_Info[] array_turrets = JsonParser.FromJson<Turret_Info>(Resources.Load<TextAsset>("turretsInfo").text);
            for (int i = 0; i < array_turrets.Length; i++){

                turrets_info = turrets_info + "Turret Type: " + array_turrets[i].TurretName + "\n";
                turrets_info = turrets_info + "     Attacks Number: " + array_turrets[i].AttacksNumber + "\n";
                turrets_info = turrets_info + "     Corruption Percent: " + array_turrets[i].CorruptionPercent + "\n";
                turrets_info = turrets_info + "     Attack Speed: " + array_turrets[i].AttackSpeed + "\n";
                turrets_info = turrets_info + "     Gold: " + array_turrets[i].Gold + "\n";
                turrets_info = turrets_info + "     Build Delay: " + array_turrets[i].BuildDelay + "\n";
                turrets_info = turrets_info + "     Attacks Damage: " + array_turrets[i].AttacksDamage + "\n\n";
            }
            //At the end, we display the data of the array.
            this.IF_Info.text = turrets_info;
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
}
