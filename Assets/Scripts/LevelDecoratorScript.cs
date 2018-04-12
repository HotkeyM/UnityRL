using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class MobSpawnRate
{
	public GameObject mob;
	public float weight;
}

public class LevelDecoratorScript : MonoBehaviour {

	public static void AddOneLadder()
	{
		Map map = ((Map)GameObject.Find ("Map").GetComponent<Map> ());
		Map.APoint point = map.GetRandomFloorTile ();
		GameObject ladder = (GameObject) Instantiate(Resources.Load("Prefabs/Ladder"));
		ladder.transform.position = new Vector3(point.x * Map.TileSize, 0.1f, point.y *  Map.TileSize);
	}

    public static void AddLockedLadder()
    {
        Map map = ((Map)GameObject.Find("Map").GetComponent<Map>());
        Map.APoint point = map.GetRandomFloorTile();
        GameObject ladder = (GameObject)Instantiate(Resources.Load("Prefabs/LockedDoor"));

        ladder.transform.position = new Vector3(point.x * Map.TileSize, 0.1f, point.y * Map.TileSize);
        point = map.GetRandomFloorTile();
        GameObject key = (GameObject)Instantiate(Resources.Load("Prefabs/Key"));
        key.transform.position = new Vector3(point.x * Map.TileSize, 0.1f, point.y * Map.TileSize);
    }

	public static void AddEnemies(MobSpawnRate [] rates, int numEnemies)
	{

		float sum = 0.0f;
		foreach (var v in rates)
			sum += v.weight;

		int type = 0;

		for (int j = 0; j < numEnemies; ++j)
		{
			
			
			float r = UnityEngine.Random.Range (0.0f, sum);
			float hs = 0;
			for (int i = 0; i < rates.Length; ++i) 
			{
				if (r <= (hs + rates [i].weight))
				{
					type = i;
					break;
				}
			}

			AddEnemy (rates [type].mob);
		}
	}

	public static void AddEnemy(GameObject mob)
	{
		Map map = ((Map)GameObject.Find ("Map").GetComponent<Map> ());
		Map.APoint point = map.GetRandomFloorTile ();
		GameObject ladder = (GameObject) Instantiate(mob);
		ladder.transform.position = new Vector3(point.x * Map.TileSize, 0.1f, point.y *  Map.TileSize);
	}

	public static void AddTreasures(int size)
	{
		Map map = ((Map)GameObject.Find ("Map").GetComponent<Map> ());

        LevelManagerScript lm = (LevelManagerScript)GameObject.Find("LevelManager").GetComponent<LevelManagerScript>();


        //int numTreasures = (int)(size * Random.Range(0.0f, LevelManagerScript.global.levelLootK) / 5.0f);

        int numTreasures = size / 5;

        string[] treasures = { "Prefabs/Apple" };

        for (int i = 0; i < numTreasures; ++i)
        {


            Map.APoint point = map.GetRandomFloorTile();
			GameObject ladder = (GameObject)Instantiate(Resources.Load(treasures[UnityEngine.Random.Range(0, treasures.Length)]));
            ladder.transform.position = new Vector3(point.x * Map.TileSize, 0.1f, point.y * Map.TileSize);
        }

		
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
