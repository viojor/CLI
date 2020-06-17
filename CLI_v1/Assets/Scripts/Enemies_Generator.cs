using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Generator : MonoBehaviour{

    private const float SECONDS_BETWEEN_WAVES = 12.0f;
    private const float SECONDS_BETWEEN_ENEMIES = 2.5f;
    private const float SECONDS_BEFORE_PORTAL_APPEAR = 2.0f;

    [SerializeField]
    private int enemies_per_wave;
    private int current_enemies = 0;

    private int total_waves;
    private int current_wave = 0;

    [SerializeField]
    private GameObject[] enemies_array = null;

    [SerializeField]
    private GameObject portal_go = null;
    private GameObject portal_instance;

    private Level_Info info_level_selected;

    private void Start(){

        this.info_level_selected = LevelsMenu_Controller.info_level_selected;

        this.enemies_per_wave = this.info_level_selected.EnemiesPerWave;
        this.total_waves = this.info_level_selected.TotalWaves;

        this.PlaceGeneratorBeginningPath();
        StartCoroutine(this.CreatePortalCoroutine());
    }

    private void PlaceGeneratorBeginningPath(){

        MapGenerator map = MapGenerator.GetMapInstance();
        InitialSquare enemies_initial_square = map.GetEnemiesInitialSquare();
        transform.position = map.GetMapSquare(enemies_initial_square.Row, enemies_initial_square.Column).transform.position;
    }

    private IEnumerator CreatePortalCoroutine(){

        yield return new WaitForSeconds(SECONDS_BEFORE_PORTAL_APPEAR);

        this.portal_instance = Instantiate(portal_go, gameObject.transform.position, Quaternion.identity);
        StartCoroutine(this.SpawnWaveCoroutine());
    }

    private IEnumerator SpawnWaveCoroutine(){

        while(this.current_wave < this.total_waves){
            
            StartCoroutine(this.SpawnEnemyCoroutine());

            yield return new WaitForSeconds(SECONDS_BETWEEN_WAVES);

            this.current_wave++;
        }
        GameObject.Destroy(this.portal_instance);
    }

    private IEnumerator SpawnEnemyCoroutine(){

        //We need to instantiate enemies until we hit the limit of the wave.
        while (this.current_enemies < this.enemies_per_wave){

            yield return new WaitForSeconds(SECONDS_BETWEEN_ENEMIES);

            int array_index = Random.Range(0, this.enemies_array.Length);
            GameObject enemy_selected = this.enemies_array[array_index];
            //We have to create the enemies in the position of the generator.
            Instantiate(enemy_selected, transform.position, Quaternion.identity);

            this.current_enemies++;
        }
        //We need to reset the value of the variable for the others waves.
        this.current_enemies = 0; 
    }
}
