using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour{

    private const int MAX_CORRUPTION_PERCENT = 100;
    private const int MAX_COINS = 9999;
    private const float SECONDS_BETWEEN_COINS = 5.0f;

    private const int COINS_INCREMENT_BY_TIME = 50;

    private const int MAX_STATUS_ALLGOOD_PERCENT = 40;
    private const int MAX_STATUS_PROBLEMS_PERCENT = 80;

    private const float SECONDS_SHOW_VAL_CHANGES = 1.0f;
    private const float SECONDS_SHOW_ERROR = 1.5f;

    private Level_Info info_level_selected;

    [SerializeField]
    private Text t_coins = null, t_turrets = null, t_corruption_percent = null, t_remaining_enemies = null;
    [SerializeField]
    private Text t_coins_changes = null, t_turrets_changes = null, t_remaining_enemies_changes = null;

    private float time_cont = 0.0f;

    [SerializeField]
    private Image i_status = null;
    [SerializeField]
    private Sprite status_AllGood = null, status_Problems = null, status_FatalError = null;

    [SerializeField]
    private Text t_errors = null;

    private void Start(){

        this.info_level_selected = LevelsMenu_Controller.info_level_selected;
        this.t_coins.text = "100";
        this.t_coins_changes.text = "";
        this.t_turrets.text = "0/" + this.info_level_selected.MaxTurrets;
        this.t_turrets_changes.text = "";
        this.t_remaining_enemies.text = (this.info_level_selected.TotalWaves * this.info_level_selected.EnemiesPerWave).ToString();
        this.t_remaining_enemies_changes.text = "";
        this.t_corruption_percent.text = "0%";
        this.t_errors.text = "";

        this.time_cont = 0;

        this.i_status.sprite = this.status_AllGood;
    }

    private void Update(){

        this.time_cont = this.time_cont + Time.deltaTime;
        if(this.time_cont >= SECONDS_BETWEEN_COINS){

            this.time_cont = 0;
            this.IncreaseCoins(COINS_INCREMENT_BY_TIME);
        }
    }

    public void IncreaseCorruptionPercent(int corruption_percent){

        string[] array_split = this.t_corruption_percent.text.Split('%');
        int actual_percent = int.Parse(array_split[0]);
        int new_percent = actual_percent + corruption_percent;

        this.t_corruption_percent.text = new_percent.ToString() + "%";

        this.ChangeStatusImage(new_percent);

        if(new_percent >= MAX_CORRUPTION_PERCENT){ //If the corruption percent raise until 100% or above, is game over.

            Scene_Controller.ChangeScene(Scene_Controller.GAME_OVER);
        }
    }
    
    private void ChangeStatusImage(int current_percent){

        if(current_percent < MAX_STATUS_ALLGOOD_PERCENT){

            this.i_status.sprite = this.status_AllGood;
        }
        else if(current_percent >= MAX_STATUS_ALLGOOD_PERCENT && current_percent < MAX_STATUS_PROBLEMS_PERCENT){

            this.i_status.sprite = this.status_Problems;
        }
        else{ // currentPercent > MAX_STATUS_PROBLEMS_PERCENT

            this.i_status.sprite = this.status_FatalError;
        }
    }

    public void IncreaseCoins(int amount_coins){

        int currents_coins = int.Parse(this.t_coins.text);
        int total_coins = currents_coins + amount_coins;
        if (total_coins < MAX_COINS){
            
            StartCoroutine(this.ShowIncrDecrValuesCoroutine("+" + amount_coins.ToString(), this.t_coins_changes));
        }
        else{ //totalCoins >= MAX_COINS

            total_coins = MAX_COINS;
            int diff_coins = total_coins - currents_coins;
            StartCoroutine(this.ShowIncrDecrValuesCoroutine("+" + diff_coins.ToString(), this.t_coins_changes));
        }
        this.t_coins.text = total_coins.ToString();
    }

    public void DecreaseCoins(int amount_coins){

        int currents_coins = int.Parse(this.t_coins.text);
        int total_coins = currents_coins - amount_coins;

        this.t_coins.text = total_coins.ToString();

        StartCoroutine(this.ShowIncrDecrValuesCoroutine("-" + amount_coins.ToString(), this.t_coins_changes));
    }

    public void IncreaseTurrets(int amount_turrets) {

        string[] array_split = this.t_turrets.text.Split('/');
        int currents_turrets = int.Parse(array_split[0]);
        currents_turrets = currents_turrets + amount_turrets;

        this.t_turrets.text = currents_turrets.ToString() + "/" + array_split[1];

        StartCoroutine(this.ShowIncrDecrValuesCoroutine("+" + amount_turrets, this.t_turrets_changes));
    }

    public void DecreaseTurrets(int amount_turrets){

        string[] array_split = this.t_turrets.text.Split('/');
        int currents_turrets = int.Parse(array_split[0]);
        currents_turrets = currents_turrets - amount_turrets;

        this.t_turrets.text = currents_turrets.ToString() + "/" + array_split[1];

        StartCoroutine(this.ShowIncrDecrValuesCoroutine("-" + amount_turrets.ToString(), this.t_turrets_changes));
    }

    public void IncreaseEnemiesCount(int amount_enemies){

        int currents_remaining_enemies = int.Parse(this.t_remaining_enemies.text);
        currents_remaining_enemies = currents_remaining_enemies + amount_enemies;

        this.t_remaining_enemies.text = currents_remaining_enemies.ToString();

        StartCoroutine(this.ShowIncrDecrValuesCoroutine("+" + amount_enemies.ToString(), this.t_remaining_enemies_changes));
    }

    public void DecreaseEnemiesCount(int amount_enemies){

        int currents_remaining_enemies = int.Parse(this.t_remaining_enemies.text);
        currents_remaining_enemies = currents_remaining_enemies - amount_enemies;

        this.t_remaining_enemies.text = currents_remaining_enemies.ToString();

        if(currents_remaining_enemies == 0){

            string[] array_split = this.t_corruption_percent.text.Split('%');
            int actual_percent = int.Parse(array_split[0]);
            if(actual_percent < 100){

                Scene_Controller.ChangeScene(Scene_Controller.WIN_GAME);
            }
        }
        StartCoroutine(this.ShowIncrDecrValuesCoroutine("-" + amount_enemies.ToString(), this.t_remaining_enemies_changes));
    }

    private IEnumerator ShowIncrDecrValuesCoroutine(string amount_changed, Text text_field){

        text_field.color = Color.red;
        text_field.text = amount_changed;
        yield return new WaitForSeconds(SECONDS_SHOW_VAL_CHANGES);
        text_field.text = ""; //Delete the data.
    }

    public int GetTotalCoins(){

        int currents_coins = int.Parse(this.t_coins.text);
        return currents_coins;
    }

    public int GetTurretsLimit(){

        string[] array_split = this.t_turrets.text.Split('/');
        int turret_limit = int.Parse(array_split[1]);

        return turret_limit;
    }

    public int GetCurrentTurrets(){

        string[] array_split = this.t_turrets.text.Split('/');
        int current_turrets = int.Parse(array_split[0]);

        return current_turrets;
    }

    public void ShowErrorMessage(string error_message){

        StartCoroutine(this.ShowErrorMessageCoroutine(error_message));
    }

    private IEnumerator ShowErrorMessageCoroutine(string error_message){

        this.t_errors.text = "Error: " + error_message;

        yield return new WaitForSeconds(SECONDS_SHOW_ERROR);
        this.t_errors.text = "";
    }
}
