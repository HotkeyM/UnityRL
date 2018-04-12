using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public delegate void Decorator();
public delegate void DecoratorSize(int size);

[System.Serializable]
public class Level
{

	public Level() { }

	public int levelNumber;
	public string name;
	public bool generated = true;
	public int levelSize = 15;
	public int numRooms = 5;
	public int asterK = 50;

	public GameObject levelPrefab = null;

	//public GameObject[] mobsPrefabs;

	public float damageK = 1.0f;
	public float speedK = 1.0f;
	public int numEnemies = 3;

	public MobSpawnRate[] mobs = new MobSpawnRate[] { new MobSpawnRate {mob = (GameObject)Resources.Load("Prefabs/Mob") , weight = 1.0f} };

	public Decorator decorations = LevelDecoratorScript.AddOneLadder;
	public DecoratorSize decorationSize = LevelDecoratorScript.AddTreasures;

    
}

[System.Serializable]
public class GlobalSettings
{
	public int numContinues = 5;

	public int numEnemies = 0;

	public float mobAttackK = 1.0f;
	public float mobSpeedK = 1.0f;
	public float mobLootK = 1.0f;
	public float levelLootK = 1.0f;
}

public class LevelManagerScript : MonoBehaviour {




    public static GlobalSettings global;

    public Map mapScript;

    public static int currentLevel = 0;


    public List<Level> levels = new List<Level>();

    void Awake()
    {
		//DontDestroyOnLoad (this);
		if (global == null) global = new GlobalSettings ();

        if (GameObject.Find("Hero") == null)
        {
            Instantiate(Resources.Load("Prefabs/Hero")).name = "Hero";
        }
        else
        {
            Debug.Log("Уже есть Герой");
        }


    }



	// Use this for initialization
	void Start () {

        FillLevelsList();

		Debug.Log ("current level to load = " + currentLevel.ToString ());

        if (currentLevel < levels.Count)
        {
           
            if (levels[currentLevel].generated)
            {
				Debug.Log ("Этот уровень процедурный");
                //mapScript.StartMap(levels[currentLevel].levelSize, levels[currentLevel].numRooms, levels[currentLevel].numEnemies);

				mapScript.StartMap (levels [currentLevel].levelSize, levels [currentLevel].numRooms);
				LevelDecoratorScript.AddEnemies (levels [currentLevel].mobs, levels [currentLevel].numEnemies);

				Debug.Log (global);
				Debug.Log (levels [currentLevel]);
				global.numEnemies = levels [currentLevel].numEnemies;

				levels [currentLevel].decorations ();
                levels[currentLevel].decorationSize( levels[currentLevel].levelSize);

                if (levels[currentLevel].levelPrefab != null) Instantiate(levels[currentLevel].levelPrefab);
            }
            else
            {
				Instantiate(levels[currentLevel].levelPrefab);
                ////Добавить поведение для фиксированных уровней
            }

            foreach (var v in GameObject.FindGameObjectsWithTag("Mob"))
            {
                ShooterScript ss = (ShooterScript)v.GetComponent<ShooterScript>();
                if (ss) ss.Cooldown = ss.Cooldown / levels[currentLevel].damageK;
                MoveScript ms = (MoveScript)v.GetComponent<MoveScript>();
                if (ms) ms.speed = ms.speed * levels[currentLevel].speedK;
            }
        }
		else
		{
        // победа
        Debug.Log("END GAME");
		SceneManager.LoadScene ("WinScene");
		}

	}

    public void StartNextLevel()
    {
        Instantiate(Resources.Load("Prefabs/loadingscreen"));

        currentLevel = currentLevel + 1;

        Application.LoadLevel(Application.loadedLevel);
    }
	
    void FillLevelsList()
    {
        
        levels.Add (new Level {
			levelNumber = -1,
			name = "begin story",
			generated = false,
			levelPrefab = (GameObject)(Resources.Load ("Levels/LevelBegin"))
		});

        levels.Add(new Level { levelNumber = 0, name = "start", levelPrefab = (GameObject)(Resources.Load("Dialogs/DialogGopnik")) , numEnemies = 1, mobs = new MobSpawnRate[] { new MobSpawnRate { mob = (GameObject)Resources.Load("Prefabs/MobKamikaze"), weight = 1.0f } }, decorations = LevelDecoratorScript.AddLockedLadder});

        levels.Add(new Level { levelNumber = 1, name = "first blood", numEnemies = 50, levelSize = 50, numRooms = 35, mobs = new MobSpawnRate[] { new MobSpawnRate { mob = (GameObject)Resources.Load("Prefabs/MobKamikaze"), weight = 1.0f }, new MobSpawnRate { mob = (GameObject)Resources.Load("Prefabs/MobWalker"), weight = 1.0f }, new MobSpawnRate { mob = (GameObject)Resources.Load("Prefabs/MobShooter"), weight = 1.0f }, new MobSpawnRate { mob = (GameObject)Resources.Load("Prefabs/Mob"), weight = 3.0f } } });


        levels.Add(new Level { levelNumber = 2, name = "tripple penetration", numEnemies = 3 });
        levels.Add(new Level { levelNumber = 3, name = "fastnfurios", numEnemies = 1, speedK = 2.0f, damageK = 2.0f });
        levels.Add(new Level { levelNumber = 4, name = "2fast2furios", levelSize = 20, numRooms = 7, numEnemies = 2, speedK = 2.0f, damageK = 2.0f });
        levels.Add(new Level { levelNumber = 5, name = "big world", levelSize = 30, numRooms = 15, numEnemies = 30, speedK = 2.0f });

    }

    // Update is called once per frame
    void Update () {
	
	}
}
