using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour{
    
    private const float max_x = 2.425f;
    private const float min_x = -6.575f;
    private const float max_y = 2.465f;
    private const float min_y = -1.535f;

    private const int columns_number = 10;
    private const int rows_number = 5;

    private Color PATH_COLOR = new Color(0.8f, 0.8f, 0.8f, 1f);

    private GameObject[,] map = new GameObject[rows_number, columns_number];
    
    [SerializeField]
    private GameObject square_go = null;
    [SerializeField]
    private Sprite[] letters_sprites = null;
    [SerializeField]
    private Sprite[] numbers_sprites = null;

    private List<Transform> path = new List<Transform>();
    private Level_Info info_level_selected;
    private InitialSquare initial_square_path;
    
    private static MapGenerator map_instance;
    
    private void Start(){

        this.info_level_selected = LevelsMenu_Controller.info_level_selected;
        this.GenerateMapsBoard();
        this.GenerateExternalBoard();
        this.GeneratePath();
    }

    private void GenerateMapsBoard(){

        for(int i = 0; i < rows_number; i++){ //y
            for(int j = 0; j < columns_number; j++){ //x

                //Our square has a dimension of 1x1, so we should multiply this value with the i and j values (1*i = i and 1*j = j).
                Vector3 square_position = new Vector3(min_x + j, max_y - i, 0);
                this.map[i, j] = Instantiate(square_go, square_position, Quaternion.identity);
            }
        }
    }

    private void GenerateExternalBoard(){

        GameObject external_square_go = new GameObject();
        external_square_go.AddComponent<SpriteRenderer>();
        //Letters -> Columns
        for(int i = 0; i < this.letters_sprites.Length; i++){

            Vector3 letter_position = new Vector3(min_x+i, max_y+1, 0);
            external_square_go.GetComponent<SpriteRenderer>().sprite = this.letters_sprites[i];
            Instantiate(external_square_go, letter_position, Quaternion.identity);
        }

        //Nums -> Rows
        for(int j = 0; j < this.numbers_sprites.Length; j++){

            Vector3 number_position = new Vector3(min_x-1, max_y-j, 0);
            external_square_go.GetComponent<SpriteRenderer>().sprite = this.numbers_sprites[j];
            Instantiate(external_square_go, number_position, Quaternion.identity);
        }
    }

    private void GeneratePath(){

        InitialSquare initial_square = this.info_level_selected.InitialSquare;

        this.initial_square_path = new InitialSquare();
        initial_square_path.Column = initial_square.Column;
        initial_square_path.Row = initial_square.Row;

        this.map[initial_square.Row, initial_square.Column].GetComponent<Square_Controller>().SetSquareState(Square_Controller.SquareStates.Path);
        this.map[initial_square.Row, initial_square.Column].GetComponent<SpriteRenderer>().color = PATH_COLOR;
        this.path.Add(this.map[initial_square.Row, initial_square.Column].transform);

        string[] array_directions = info_level_selected.Path;
        this.ProcessPathDirections(array_directions, initial_square);
    }

    private void ProcessPathDirections(string[] path_directions, InitialSquare initial_square){
        
        for (int i = 0; i < path_directions.Length; i++){

            this.GetNextPathSquare(path_directions[i], initial_square);
        }
    }
    
    private void GetNextPathSquare(string new_direction, InitialSquare initial_square){

        if (new_direction.Equals("N")){ //North -> row - 1.

            initial_square.Row = initial_square.Row - 1;
        }
        else if (new_direction.Equals("S")){ //South -> row + 1.

            initial_square.Row = initial_square.Row + 1;
        }
        else if (new_direction.Equals("E")){ //East -> column + 1.

            initial_square.Column = initial_square.Column + 1;
        }
        else{ //West -> column - 1.

            initial_square.Column = initial_square.Column - 1;
        }
        
        this.map[initial_square.Row, initial_square.Column].GetComponent<Square_Controller>().SetSquareState(Square_Controller.SquareStates.Path);
        this.path.Add(this.map[initial_square.Row, initial_square.Column].transform);
        this.map[initial_square.Row, initial_square.Column].GetComponent<SpriteRenderer>().color = PATH_COLOR;
    }
    
    public List<Transform> GetPath(){

        return this.path;
    }

    public InitialSquare GetEnemiesInitialSquare(){

        return this.initial_square_path;
    }

    public GameObject GetMapSquare(int row, int column){

        return this.map[row, column];
    }

    public List<GameObject> GetAllTurrets(){

        List<GameObject> turrets_list = new List<GameObject>();
        for (int i = 0; i < rows_number; i++){
            for (int j = 0; j < columns_number; j++){

                Square_Controller current_square_controller = this.map[i, j].GetComponent<Square_Controller>();
                if (current_square_controller.GetSquareState() == Square_Controller.SquareStates.Turret){

                    GameObject turret = current_square_controller.GetSquaresTurret();
                    turrets_list.Add(turret);
                }
            }
        }
        return turrets_list;
    }
    
    public string GetInfoAllTurrets(){

        List<GameObject> turrets_list = this.GetAllTurrets();
        string turrets_info = "";

        for (int i = 0; i < turrets_list.Count; i++){

            turrets_info = turrets_info + turrets_list[i].GetComponent<TurretBase_Controller>().GetTurretInfo();
            turrets_info = turrets_info + "\n";
        }
        return turrets_info;
    }

    public string GetInfoOneTypeTurrets(string type_to_search){

        List<GameObject> turrets_list = this.GetAllTurrets();
        string turrets_info = "";

        for (int i = 0; i < turrets_list.Count; i++){

            GameObject turret_square = turrets_list[i];
            if (turret_square.GetComponent<TurretBase_Controller>().GetTurretType().ToLower().Equals(type_to_search)){

                turrets_info = turrets_info + turret_square.GetComponent<TurretBase_Controller>().GetTurretInfo();
                turrets_info = turrets_info + "\n";
            }
        }
        return turrets_info;
    }

    public void DeleteAllTurrets(){

        for (int i = 0; i < rows_number; i++){
            for (int j = 0; j < columns_number; j++){

                this.DeleteSquareTurret(i, j);
            }
        }
    }

    private void DeleteSquareTurret(int row_square, int column_square){

        Square_Controller current_square_controller = this.map[row_square, column_square].GetComponent<Square_Controller>();
        if (current_square_controller.GetSquareState() == Square_Controller.SquareStates.Turret){

            //We need to change the square's state.
            current_square_controller.SetSquareState(Square_Controller.SquareStates.Empty);
            current_square_controller.DeleteSquaresTurret();
        }
    }

    //Singleton.
    public static MapGenerator GetMapInstance(){

        if(map_instance == null){

            map_instance = GameObject.FindObjectOfType<MapGenerator>();
            if(map_instance == null){

                GameObject map_container = new GameObject("MapInstance");
                map_instance = map_container.AddComponent<MapGenerator>();
            }
        }
        return map_instance;
    }

    protected MapGenerator(){

    }
}
