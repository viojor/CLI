using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase_Controller{
    
    //We will use this two arrays to determine the position of the map indicated via commands.
    protected readonly char[] columns_name = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
    protected readonly char[] rows_name = new char[] { '0', '1', '2', '3', '4' };
    protected readonly string[] turrets_type = new string[] { "antivirus", "firewall", "honeypot", "ips" };

    protected const int ONE_ELEMENT_COMMAND = 1;
    protected const int TWO_ELEMENTS_COMMAND = 2;
    protected const int THREE_ELEMENTS_COMMAND = 3;

    protected const int VALID_POSITION_LENGTH = 2;

    protected Errors_Controller errors_controller;

    public virtual string Command_Name { set; get; }

    public virtual void ExecuteCommand(string[] command_splited){

        this.errors_controller = new Errors_Controller();
    }

    protected bool IsValueInArray<T>(T value_to_search, T[] array_values){

        for (int i = 0; i < array_values.Length; i++){
            if (array_values[i].Equals(value_to_search)){

                return true;
            }
        }
        return false;
    }

    protected int IndexValueInArray<T>(T value_to_search, T[] array_values){

        for (int i = 0; i < array_values.Length; i++){
            if (array_values[i].Equals(value_to_search)){

                return i;
            }
        }
        return -1;
    }
}
