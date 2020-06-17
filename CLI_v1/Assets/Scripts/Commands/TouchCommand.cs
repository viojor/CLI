using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchCommand : CommandBase_Controller{

    private MapGenerator map_instance;
    private GameObject[] array_turret_go;

    private UI_Controller ui_controller;

    private const int DEFAULT_COINS = 9999;
    private const int DEFAULT_LIMIT_TURRETS = 9999;

    public override string Command_Name{
        
        get => "touch";
        set => this.Command_Name = "touch";
    }
    
    public override void ExecuteCommand(string[] command_splited){

        base.ExecuteCommand(command_splited);

        this.map_instance = MapGenerator.GetMapInstance();
        this.ui_controller = GameObject.Find("UI").GetComponent<UI_Controller>();

        //touch turretType initialPosition (3).
        if (command_splited.Length == THREE_ELEMENTS_COMMAND){
            if (this.IsTypeCorrect(command_splited[1])){
                if (this.IsPositionCorrect(command_splited[2].ToCharArray())){

                    //First, we need to load the TurretGO.
                    this.LoadArrayTurretsGO();

                    int index_array_turrets = this.IndexValueInArray(command_splited[1], base.turrets_type);
                    GameObject turret_indicated = this.array_turret_go[index_array_turrets];

                    TurretBase_Controller turret_base_controller = turret_indicated.GetComponent<TurretBase_Controller>();
                    //We need to check if we have enough coins and if we havent exceeded the turrets limit.
                    if (this.HaveEnoughCoins(turret_base_controller)){
                        if (this.AreTurretsBelowLimit()){

                            GameObject square = this.GetSquareInPosition(command_splited[2].ToCharArray());
                            if (square.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Empty){

                                this.CreateTurretInPosition(command_splited[2].ToCharArray(), turret_indicated);
                                if (this.ui_controller != null){

                                    this.ui_controller.IncreaseTurrets(1); //We have to increase the turrets counter.
                                    this.ui_controller.DecreaseCoins(turret_base_controller.Turret_cost); //We need to decrease the amount of coins.

                                }
                            }
                            else{
                                if (square.GetComponent<Square_Controller>().GetSquareState() == Square_Controller.SquareStates.Path){

                                    errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.PathSquare);
                                }
                                else{

                                    errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.OccupiedSquare);
                                }
                            }
                        }
                        else{

                            errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.ExceedTurretsLimit);
                        }
                    }
                    else{

                        errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.NotEnoughtCoins);
                    }
                }
                else{

                    errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.InexistentPosition);
                }
            }
            else{

                errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.InexistentType);
            }
        }
        else{

            errors_controller.SetErrorMessage(Errors_Controller.ErrorsType.CommandStructInvalid);
        }
    }

    private bool IsTypeCorrect(string type_indicated){

        return this.IsValueInArray(type_indicated, base.turrets_type);
    }

    private bool IsPositionCorrect(char[] array_char_position){
        
        return array_char_position.Length == VALID_POSITION_LENGTH && 
            (this.IsValueInArray(array_char_position[0], base.columns_name) && this.IsValueInArray(array_char_position[1], base.rows_name));
    }

    private void LoadArrayTurretsGO(){

        this.array_turret_go = Resources.LoadAll("Prefabs/Turrets", typeof(GameObject)).Cast<GameObject>().ToArray();
    }
    
    private bool HaveEnoughCoins(TurretBase_Controller turret_controller){

        int total_coins = DEFAULT_COINS;
        if(this.ui_controller != null){

            total_coins = this.ui_controller.GetTotalCoins();
        }

        int turret_cost = turret_controller.Turret_cost;
        return total_coins >= turret_cost;
    }

    private bool AreTurretsBelowLimit(){

        int limit_turret = DEFAULT_LIMIT_TURRETS;
        int current_turret = 0;

        if (this.ui_controller != null){

            limit_turret = this.ui_controller.GetTurretsLimit();
            current_turret = this.ui_controller.GetCurrentTurrets();
        }
        return limit_turret > current_turret;
    }

    private void CreateTurretInPosition(char[] array_char_position, GameObject turret_indicated){
        
        GameObject square = this.GetSquareInPosition(array_char_position);
        GameObject turret = GameObject.Instantiate(turret_indicated, square.transform.position, Quaternion.identity);

        turret.GetComponent<TurretBase_Controller>().SetTurretPosition(array_char_position);
        square.GetComponent<Square_Controller>().SetSquareState(Square_Controller.SquareStates.Turret);
        square.GetComponent<Square_Controller>().SetSquaresTurret(turret);
    }

    private GameObject GetSquareInPosition(char[] array_char_position){
        
        int position_row = this.IndexValueInArray(array_char_position[1], base.rows_name);
        int position_column = this.IndexValueInArray(array_char_position[0], base.columns_name);
        
        //We dont check the values of positionRow and positionColumn cause we already know that both values are valids (IsPositionCorrect).
        return this.map_instance.GetMapSquare(position_row, position_column);
    }
}
