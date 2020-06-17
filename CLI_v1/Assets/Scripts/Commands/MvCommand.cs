using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MvCommand : CommandBase_Controller{

    private MapGenerator map_instance;

    public override string Command_Name{

        get => "mv";
        set => this.Command_Name = "mv";
    }

    public override void ExecuteCommand(string[] command_splited){

        base.ExecuteCommand(command_splited);

        this.map_instance = MapGenerator.GetMapInstance();

        //mv initialPosition finalPosition (3).
        if (command_splited.Length == THREE_ELEMENTS_COMMAND){
            if (this.IsPositionCorrect(command_splited[1].ToCharArray()) && this.IsPositionCorrect(command_splited[2].ToCharArray())){

                GameObject initial_square = this.GetSquareInPosition(command_splited[1].ToCharArray());
                GameObject final_square = this.GetSquareInPosition(command_splited[2].ToCharArray());

                //We can move the turret only if the target square is empty.
                if(final_square.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Empty){
                    if (initial_square.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Turret){

                        //Get the turret of the initial square.
                        GameObject turret = initial_square.GetComponent<Square_Controller>().GetSquaresTurret();
                        //Change the state of the initial square from busy to empty.
                        initial_square.GetComponent<Square_Controller>().SetSquareState(Square_Controller.SquareStates.Empty);
                        //Set the turret's position with the final square's position.
                        turret.transform.position = final_square.transform.position;
                        turret.GetComponent<TurretBase_Controller>().SetTurretPosition(command_splited[2].ToCharArray());
                        //Change the state of the final square from empty to busy.
                        final_square.GetComponent<Square_Controller>().SetSquareState(Square_Controller.SquareStates.Turret);
                        //Set the turret gameobject into the final square.
                        final_square.GetComponent<Square_Controller>().SetSquaresTurret(turret);
                    }
                    else{
                        if(initial_square.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Empty){

                            errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.EmptySquare);
                        }
                        else{

                            errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.PathSquare);
                        }
                    }
                }
                else{
                    if (final_square.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Turret) { 

                        errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.OccupiedSquare);
                    }
                    else{

                        errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.PathSquare);
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
