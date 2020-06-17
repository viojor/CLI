using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RmCommand : CommandBase_Controller{

    private MapGenerator map_instance;

    private UI_Controller ui_controller;

    public override string Command_Name{

        get => "rm";
        set => this.Command_Name = "rm";
    }

    public override void ExecuteCommand(string[] command_splited){

        base.ExecuteCommand(command_splited);

        this.map_instance = MapGenerator.GetMapInstance();
        this.ui_controller = GameObject.Find("UI").GetComponent<UI_Controller>();

        //rm (1).
        if (command_splited.Length == ONE_ELEMENT_COMMAND){

            int total_turrets = map_instance.GetAllTurrets().Count;
            map_instance.DeleteAllTurrets();
            if(this.ui_controller != null){

                this.ui_controller.DecreaseTurrets(total_turrets);
            }
        }
        //rm position (2).
        else if(command_splited.Length == TWO_ELEMENTS_COMMAND){
            if (this.IsPositionCorrect(command_splited[1].ToCharArray())){
                
                GameObject square_indicated = this.GetSquareInPosition(command_splited[1].ToCharArray());
                if (square_indicated.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Turret){

                    int turret_cost = square_indicated.GetComponent<Square_Controller>().GetSquaresTurret().GetComponent<TurretBase_Controller>().Turret_cost;
                    square_indicated.GetComponent<Square_Controller>().SetSquareState(Square_Controller.SquareStates.Empty);
                    square_indicated.GetComponent<Square_Controller>().DeleteSquaresTurret();

                    if (this.ui_controller != null){

                        this.ui_controller.DecreaseTurrets(1);

                        int returnedCoins = turret_cost/2;
                        this.ui_controller.IncreaseCoins(returnedCoins);
                    }
                }
                else{
                    if(square_indicated.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Path){

                        errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.PathSquare);
                    }
                    else{

                        errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.EmptySquare);
                    }
                }
            }
            else{

                errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.InexistentPosition);
            }
        }
        else{

            errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.CommandStructInvalid);
        }
    }

    private bool IsPositionCorrect(char[] array_char_position){

        return array_char_position.Length == VALID_POSITION_LENGTH &&
            (this.IsValueInArray(array_char_position[0], base.columns_name) && this.IsValueInArray(array_char_position[1], base.rows_name));
    }
    
    private GameObject GetSquareInPosition(char[] array_char_position){

        int position_row = this.IndexValueInArray(array_char_position[1], base.rows_name);
        int position_column = this.IndexValueInArray(array_char_position[0], base.columns_name);

        //We dont check the values of positionRow and positionColumn cause we already know that both values are valids (IsPositionCorrect).
        return this.map_instance.GetMapSquare(position_row, position_column);
    }
}
