using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Errors_Controller{

    private UI_Controller ui_controller;

    public enum ErrorsType{

        NotEnoughtCoins,
        ExceedTurretsLimit,
        EmptySquare,
        OccupiedSquare,
        PathSquare,
        InexistentPosition,
        InexistentType,
        CommandStructInvalid,
        InexistentCommand
    }
    
    public void SetErrorMessage(ErrorsType error_type){

        this.ui_controller = GameObject.Find("UI").GetComponent<UI_Controller>();

        string error_message = "";
        switch (error_type){

            case ErrorsType.NotEnoughtCoins:

                error_message = "Not Enought Coins";
                break;
            case ErrorsType.ExceedTurretsLimit:

                error_message = "Max Turrets Num Created";
                break;
            case ErrorsType.EmptySquare:

                error_message = "Empty Square";
                break;
            case ErrorsType.OccupiedSquare:

                error_message = "Occupied Square";
                break;
            case ErrorsType.PathSquare:

                error_message = "Path Square";
                break;
            case ErrorsType.InexistentPosition:

                error_message = "Inexistent Position";
                break;
            case ErrorsType.InexistentType:

                error_message = "Inexistent Type";
                break;
            case ErrorsType.CommandStructInvalid:

                error_message = "Invalid Command Struct";
                break;
            case ErrorsType.InexistentCommand:

                error_message = "Inexistent Command";
                break;
            default:

                error_message = "Undefined Error";
                break;
        }
        this.ui_controller.ShowErrorMessage(error_message);
    }
}
