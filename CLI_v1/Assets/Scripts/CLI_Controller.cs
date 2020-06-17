using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CLI_Controller : MonoBehaviour{

    private const int COMMANDS_HISTORY_LIMIT = 5;

    [SerializeField]
    private InputField IF_CLI = null;

    private readonly string[] array_commands_allowed = new string[] { "ls", "man", "touch", "rm", "mv", "info" };

    private readonly CommandBase_Controller[] array_commands_classes = new CommandBase_Controller[] { new InfoCommand(), new LsCommand(), new ManCommand(),
        new MvCommand(), new RmCommand(), new TouchCommand() };

    private List<string> commands_history;
    private int history_index;

    private Errors_Controller errors_controller;

    private void Start(){
        
        this.commands_history = new List<string>();
        this.errors_controller = new Errors_Controller();

        //The IF_CLI must be activated since the beginning.
        this.IF_CLI.ActivateInputField();

        //Initially, the info panel has to show the commands info.
        this.ExecuteCommand("man".Split(' '));
    }

    private void Update(){

        //UpArrow -> Previous command.
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            
            if (this.history_index > 0){

                this.history_index--;
                this.IF_CLI.text = this.commands_history[this.history_index];
            }
        }
        //DownArrow -> Next command.
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            
            if (this.history_index < this.commands_history.Count - 1){

                this.history_index++;
                this.IF_CLI.text = this.commands_history[this.history_index];
            }
        }
    }

    //Will be executed when we introduce a command in the IF_CLI.
    public void ReadCommandInput(){

        string input = this.IF_CLI.text;

        //Check if the input is not empty (" ").
        if (input != ""){

            string[] input_split = input.Split(' ');
            string[] input_to_lower_split = this.StringArrayToLower(input_split);

            //Check if the command, the first part of the splited input, is on the list (arrayCommandsAllowed). 
            if (this.ValueIsInsideArray(input_to_lower_split[0], array_commands_allowed)){
                
                this.ExecuteCommand(input_to_lower_split);
            }
            else{

                this.errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.InexistentCommand);
            }

            //Insert the command in the history (the command might be wrong, but we still stock it).
            this.AddCommandHistory(input);

            //Clear the command line.
            this.IF_CLI.text = "";
        }

        //After an execution, we have to enable the IF_CLI again.
        this.IF_CLI.ActivateInputField();
    }

    private string[] StringArrayToLower(string[] array_normal_string){

        string[] array_lower_string = new string[array_normal_string.Length];
        for(int i = 0; i < array_normal_string.Length; i++){

            array_lower_string[i] = array_normal_string[i].ToLower();
        }
        return array_lower_string;
    }

    private bool ValueIsInsideArray(string current_value, string[] array_of_values){
        
        for (int i = 0; i < array_of_values.Length; i++){
            if (array_of_values[i].Equals(current_value)){

                return true;
            }
        }
        return false;
    }

    private void ExecuteCommand(string[] input_split){

        for(int i = 0; i < this.array_commands_classes.Length; i++){
            if (this.array_commands_classes[i].Command_Name.Equals(input_split[0])){
                
                this.array_commands_classes[i].ExecuteCommand(input_split);
            }
        }
    }
        
    private void AddCommandHistory(string command){

        //We only stock the last X commands.
        //If the history is full, we have to delete the first element (the oldest command) and we add the new one on the end.
        //If the history is not full, we only have to add the new command on the end.
        if (this.commands_history.Count == COMMANDS_HISTORY_LIMIT){

            this.commands_history.RemoveAt(0);
        }
        this.commands_history.Add(command);

        //We need to restart the index of the history.
        this.history_index = this.commands_history.Count;
    }
}
