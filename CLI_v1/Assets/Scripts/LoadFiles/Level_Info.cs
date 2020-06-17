using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level_Info{

    public string LevelName { get; set; }
    public InitialSquare InitialSquare { get; set; }
    public string[] Path { get; set; }
    public int MaxTurrets { get; set; }
    public int TotalWaves { get; set; }
    public int EnemiesPerWave { get; set; }
}

[System.Serializable]
public class InitialSquare{

    public int Row { get; set; }
    public int Column { get; set; }
}
