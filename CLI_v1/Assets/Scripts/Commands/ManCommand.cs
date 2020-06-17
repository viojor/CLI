using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManCommand : CommandBase_Controller{

    private InputField IF_Info;

    public override string Command_Name{

        get => "man";
        set => this.Command_Name = "man";
    }

    public override void ExecuteCommand(string[] command_splited){

        base.ExecuteCommand(command_splited);
        //man (1).
        if (command_splited.Length == ONE_ELEMENT_COMMAND){

            //We need to find first the IF_Info.
            this.GetCanvasIF_Info();

            string commands_info = "";
            Command_Info[] array_commands = JsonParser.FromJson<Command_Info>(Resources.Load<TextAsset>("commandsInfo").text);
            for (int i = 0; i < array_commands.Length; i++){
                
                commands_info = commands_info + "Command Name: " + array_commands[i].CommandName + "\n";
                commands_info = commands_info + "     Structure: " + array_commands[i].Structure + "\n";
                commands_info = commands_info + "     Description: " + array_commands[i].Description + "\n";
                commands_info = commands_info + "     Examples: " + array_commands[i].Example + "\n\n";
            }
            //At the end, we display the data of the array.
            this.IF_Info.text = commands_info;
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
