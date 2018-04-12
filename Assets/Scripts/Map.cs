using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Map : MonoBehaviour {

	public enum Tile {None, Floor, Wall, Solid, FloorTest};

	public GameObject SolidTile;
	public GameObject FloorTile;
	public GameObject FloorTest;
	public GameObject WallTile;



	public GameObject FloorRenderer;
	public GameObject Solidrenderer;

	public int AstarCoef = 10;

	public const float TileSize = 2.0f;

	Dictionary <APoint, TileObject> tileMap;

	public class TileObject: Object
	{

		public TileObject(int x, int y, Tile tileType, Color color)
		{
			c = color;
			t = tileType;

			if (data == null) data = new TileObject[40,40];
		}

		public virtual void Instantiate()
		{
		}

		Color c;
		Tile t;


		public static void ResizeMap(int x, int y)
		{
			 
			TileObject [,] d = new TileObject[x,y];
			if (data != null) data.CopyTo(d,0);
			data = d;
		}

		static TileObject [,] data;
		public static TileObject GetTile(int x, int y)
		{
			if (x < data.GetLength(0) && (y < data.GetLength(1)))
			{
				return data[x, y];
			}
			else return null;
		}
	}

	class Room
	{
		public int x;
		public int y;
		public int w;
		public int h;

		public Color color;

		public bool Intersect(Room r)
		{
			return !(r.x >= (x + w) || x >= (r.x + r.w) || r.y >= (y + h) || y >= (r.y + r.h));
		//	return !(r.x > (x + w) || x > (r.x + r.w) || r.y > (y + h) || y > (r.y + r.h));
		}
	}



	public class APoint : System.IComparable <APoint> {
		public int x, y, cost;

		public APoint()
		{

		}

		public APoint(int x, int y)
		{   
			this.x = x;
			this.y = y;
		}

		public int CompareTo(APoint p)
		{
			//APoint p = (APoint) other;
			if (this.cost  < p.cost) return -1;
			else if (this.cost  > p.cost) return 1;
			else return 0;
		}

		public bool Equal(APoint other)
		{
			if ((this.x == other.x) && (this.y == other.y)) return true;
			else return false;
		}
		

		public static bool operator==(APoint thi, APoint p) 
		{
			return thi.x == p.x && thi.y == p.y;
		}


		public static bool operator!=(APoint thi, APoint p) {
			return !(thi.x == p.x && thi.y == p.y);
		}
		
		public static bool operator<(APoint thi, APoint p) {
			return thi.cost > p.cost;
		}
		public static bool operator>(APoint thi, APoint p) {
			return thi.cost < p.cost;
		}
	}

	public class PriorityQueue <T> where T : System.IComparable <T>
	{
		private List <T> data;
		
		public PriorityQueue()
		{
			this.data = new List <T>();
		}

		public void Enqueue(T item)
		{
			data.Add(item);
			int ci = data.Count - 1;
			while (ci  > 0)
			{
				int pi = (ci - 1) / 2;
				if (data[ci].CompareTo(data[pi])  >= 0)
					break;
				T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
				ci = pi;
			}
		}

		public T Dequeue()
		{
			// Assumes pq isn't empty
			int li = data.Count - 1;
			T frontItem = data[0];
			data[0] = data[li];
			data.RemoveAt(li);
			
			--li;
			int pi = 0;
			while (true)
			{
				int ci = pi * 2 + 1;
				if (ci  > li) break;
				int rc = ci + 1;
				if (rc  <= li && data[rc].CompareTo(data[ci])  < 0)
					ci = rc;
				if (data[pi].CompareTo(data[ci])  <= 0) break;
				T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp;
				pi = ci;
			}
			return frontItem;
		}

		public int Count()
		{
			return data.Count;
		}
	}
	
	//const int maxWidth = 10;
		//const int maxHeight = 40;

		int m_width;
		int m_height;

		public List <GameObject> mapTiles = new List<GameObject>();
		public List <GameObject> GameItems = new List<GameObject>();	

		List <Room> rooms = new List<Room>();
		public Tile [,] m_data = {};

		Mesh m_mesh;

		public Map(int height, int width) 
		{
			m_width = width;
			m_height = height;

			m_data = new Tile[width,height];
		}

		

		public void Generate(int roomsCount, int colors = 1)
		{
			//rooms.Clear();
			for (int i = 0; i < roomsCount; i++)
				for (int j = 0; j < 100; j++)
			{
				int w = Random.Range(3,10);
				int h = Random.Range(3,10);

				Room room = new Room();
				room.x = Random.Range(3, m_width - w - 3);
				room.y = Random.Range (3, m_height - h - 3);
				room.w = w;
				room.h = h;

			Color [] colorList = {Color.white, Color.blue, Color.gray};

			//Color = colorList[(int)Mathf.Sqrt(Random.Range(0.0f, 1.0f * colors * colors))];

				//List <Room> inter = rooms.FindAll(r => (room.Intersect(r) == true));
				// то же самое, фактически, но я боюсь что-то LINQ
				List <Room> inter = rooms.FindAll(room.Intersect);
				if (inter.Count == 0)
				{
					rooms.Add(room);
				break;
				}

			}



	}

	/*public class Comparer : IComparer<>
	{
		public Int32 Compare(Data a, Data b)
		{
			if (a == null && b == null)
				return 0;
			if (a == null)
				return -1;
			if (b == null)
				return +1;
			return a.Priority.CompareTo(b.Priority);
		}
	}*/

	int CalcCost(APoint p, APoint finish)
	{
		if (m_data[p.x, p.y]  == Tile.Floor) return (p.x-finish.x)*(p.x-finish.x) + 
			(p.y-finish.y)*(p.y-finish.y);
		if (m_data[p.x, p.y] == Tile.Solid) return AstarCoef*(p.x-finish.x)*(p.x-finish.x) + 
			(p.y-finish.y)*(p.y-finish.y);
		else return 2;


	}

	void GeneratePassage(APoint start, APoint finish)
	{
		int  [,] parents = new int[m_width,m_height];

		for (int i = 0; i < m_width; i++) for (int j = 0; j < m_height; j++)
			parents[i,j] = -1;

//		SortedDictionary <int, APoint> active = new SortedList<int, APoint>(Comparer<int>.Default);
//		active.Add(start.cost, start);

		PriorityQueue <APoint> active = new PriorityQueue <APoint>();
		active.Enqueue(start);

		int [,] directions =  new int[4,2] {{1,0}, {0,1}, {-1,0}, {0,-1}};

		while (active.Count() != 0)
		{
			APoint point = active.Dequeue();

			//APoint point = active.
			//APoint point = active.Values[0];
			//active.
			if (point.Equal(finish)) break;

			for (int i = 0; i < 4; i++)
			{
				APoint p = new APoint();
				p.x = point.x - directions [i,0];
				p.y = point.y - directions [i,1];
				p.cost = 0;

				if (p.x < 0 || p.y < 0 || p.x >= m_width || p.y >= m_height)
					continue;

				if (parents[p.x, p.y] < 0)
				{
					p.cost = CalcCost(p, finish);
					active.Enqueue(p);

					parents [p.x, p.y] = i;
				}

			}
		}

		APoint pnt = finish;
		while (!(pnt.Equal(start)))
		{
			m_data[pnt.x, pnt.y] = Tile.FloorTest;

			pnt.x += directions[parents[pnt.x, pnt.y],0];
			pnt.y += directions[parents[pnt.x, pnt.y],1];//ну ёпта
		}



	}
	
	void FillMap()
	{
		for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
		
		{
			m_data[x,y] = Tile.Solid;

		}

		foreach (Room room in rooms)
		{
			for (int x = 0; x < room.w; ++x) for (int y = 0; y < room.h; ++y) {
				m_data[(room.x + x), (room.y + y)] = Tile.Floor;

				tileMap.Add(new APoint(x,y), new TileObject(x,y, Tile.Floor, room.color));
			}
		}
	}

	void GenerateWalls()
	{
		int [,] offsets = new int[8,2] {
			{-1,-1}, { 0,-1}, { 1,-1}, { 1, 0},
			{ 1, 1}, { 0, 1}, {-1, 1}, {-1, 0},
		};

		for (int x = 1; x < m_width - 1; ++x) for (int y = 1; y < m_height - 1; ++y) {
			if (m_data[x,y] == Tile.Solid) for (int i = 0; i < 8; ++i) {
				// если по соседству есть хоть одна клетка комнаты или коридора - размещаем стену (индекс 2)
				if (m_data[(x + offsets[i,0]),(y + offsets[i,1])] == Tile.Floor) {
					m_data[x, y] = Tile.Wall;
					break;
				}
			}
		}
	}

	void ClearTiles()
	{
		mapTiles.Clear();
	}

	void InstantiateMap()
	{
        // x - width (X), y - height (Z)
        for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
            {
                GameObject go;
                Tile t = m_data[x, y];
                switch (t)
                {
                    /*case Tile.Solid:
                         go = (GameObject)Instantiate(SolidTile, new Vector3(x*1.0f, 0.0f,y*1.0f),
                                                                Quaternion.identity);
                        mapTiles.Add(go);
                        break;
                */

                    /*	case Tile.Floor:
                            go = (GameObject)Instantiate(FloorTile, new Vector3(x*1.0f, 0.0f,y*1.0f),
                                                                    Quaternion.identity);
                            mapTiles.Add(go);
                            break;
                    */
                    case Tile.FloorTest:
                        //go = (GameObject)Instantiate(FloorTest, new Vector3(x*TileSize, 0.0f,y*TileSi),
                        //                             Quaternion.identity);
                        //mapTiles.Add(go);
                        break;
                    case Tile.Wall:
                        go = (GameObject)Instantiate(WallTile, new Vector3(x * TileSize, TileSize * 0.5f, y * TileSize),
                                                     Quaternion.identity);
                        mapTiles.Add(go);
                        break;

                }
            }

			FloorRendererScript fr = FloorRenderer.GetComponent("FloorRendererScript")
				as FloorRendererScript;
			fr.GenerateMesh(this.m_data, Tile.Floor,0.0f, TileSize);
			
			fr = Solidrenderer.GetComponent("FloorRendererScript")
				as FloorRendererScript;
			fr.GenerateMesh(this.m_data, Tile.Solid, 2.0f, TileSize);



			Debug.Log("Создано объектов - " + mapTiles.Count.ToString());
		
	}
		
	void GenerateAllPassages()
	{
		foreach (Room r in rooms)
		{
			APoint center1 = new APoint();
			center1.x = r.x + (r.w/2);
			center1.y = r.y + (r.h/2);
			center1.cost = 1;

			foreach (Room r2 in rooms)
			{
				APoint center2 = new APoint();
				center2.x = r2.x + (r2.w/2);
				center2.y = r2.y + (r2.h/2);
				center2.cost = 1;
				GeneratePassage(center1, center2);
			}
		}

		for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
			if (m_data[x,y] == Tile.FloorTest) m_data[x,y] = Tile.Floor;
	}

	void PrepareMesh()
	{
	//	mesh = GetComponent<MeshFilter> ().mesh;
	}

	void GenerateSolidMesh()
	{
	/*	List<Vector3> newVertices = new List<Vector3>();
		List<int> newTriangles = new List<int>();
		List<Vector2> newUV = new List<Vector2>();

		for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
		{
			if
		}
		*/
	}

	// Use this for initialization

	void InitLevel(int width = 40, int height = 40, int numRooms = 10)
	{
		m_width = width;
		m_height = height;

		m_data = new Tile[m_width,m_height];
		
		rooms = new List<Room>();
		
		Generate(numRooms);
		FillMap();
		GenerateAllPassages();
		GenerateWalls();
		
		ClearTiles();
		InstantiateMap();
	}

	public List<Vector3> GetAStarPath(Vector3 src, Vector3 dest)
	{

		List<Vector3> path = new List<Vector3> ();
		
		APoint start = new APoint (Mathf.RoundToInt (src.x / TileSize), Mathf.RoundToInt (src.z / TileSize));
        Debug.Log(start);

		APoint finish = new APoint (Mathf.RoundToInt (dest.x / TileSize), Mathf.RoundToInt (dest.z / TileSize));
        Debug.Log(finish);

        if (start.Equal(finish))
        {

            List<Vector3> l = new List<Vector3>();
            l.Add(dest);
            return l;
        }

        if (start.x < 0 || start.y < 0 || finish.x < 0 || finish.y < 0 || start.x >= m_width || start.y >= m_height || finish.x >= m_width || finish.y >= m_height)
		{
			List <Vector3> l = new List<Vector3> ();
			l.Add (dest);
			return l;
		}

		int  [,] parents = new int[m_width,m_height];

		for (int i = 0; i < m_width; i++) for (int j = 0; j < m_height; j++)
			parents[i,j] = -1;

		//		SortedDictionary <int, APoint> active = new SortedList<int, APoint>(Comparer<int>.Default);
		//		active.Add(start.cost, start);

		PriorityQueue <APoint> active = new PriorityQueue <APoint>();
		active.Enqueue(start);

		int [,] directions =  new int[4,2] {{1,0}, {0,1}, {-1,0}, {0,-1}};

		while (active.Count() != 0)
		{
			APoint point = active.Dequeue();

			//APoint point = active.
			//APoint point = active.Values[0];
			//active.
			if (point.Equal(finish)) break;

			for (int i = 0; i < 4; i++)
			{
				APoint p = new APoint();
				p.x = point.x - directions [i,0];
				p.y = point.y - directions [i,1];
				p.cost = 0;

				if (p.x < 0 || p.y < 0 || p.x >= m_width || p.y >= m_height)
					continue;

				if (parents[p.x, p.y] < 0)
				{
					if (m_data [p.x, p.y] == Tile.Floor)
						p.cost = (p.x - finish.x) * (p.x - finish.x) +
						(p.y - finish.y) * (p.y - finish.y);
					else
						p.cost = 1000;
					active.Enqueue(p);

					parents [p.x, p.y] = i;
				}

			}
		}

		APoint pnt = finish;
		while (!(pnt.Equal(start)))
		{
			//m_data[pnt.x, pnt.y] = Tile.FloorTest;

			path.Add(new Vector3(pnt.x * TileSize, 0.0f, pnt.y * TileSize));

            if (pnt.x < 0 || pnt.y < 0 || pnt.x >= m_width || pnt.y >= m_height) break;

            pnt.x += directions[
                parents[pnt.x, pnt.y]
                ,0];

            if (parents[pnt.x, pnt.y] < 0 || parents[pnt.x, pnt.y] >= directions.Length)
            {
                Debug.Log("wrong parents[pnt.x, pnt.y] =" + parents[pnt.x, pnt.y].ToString());
                Debug.Log("start ="+ start.x.ToString() + " " + start.y.ToString());
                Debug.Log("pnt =" + start.x.ToString() + " " + start.y.ToString());



                break;
            }

			pnt.y += directions[
                parents[pnt.x, pnt.y]
                ,1];//ну ёпта
		}

		path.Reverse ();

		return path;		
	}

	public APoint GetRandomFloorTile()
	{
		List <APoint> floors = new List<APoint>();
		
		for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
			if (m_data[x,y] == Tile.Floor)
		{
			APoint p = new APoint();
			p.x = x;
			p.y = y;
			floors.Add(p);
		}
		
		
		APoint randomTile = floors[Random.Range(0, floors.Count-1)];
		
		return randomTile;
	}

	public Vector3 GetRandomFloorTileVector()
	{

        APoint randomTile = GetRandomFloorTile();


        Vector3 v = new Vector3 (randomTile.x * TileSize, 0.0f, randomTile.y * TileSize);

		return v;
	}


    public void StartMap(int size, int roomsCount, int enemiesNumber, List<GameObject> enemiesPrefabs = null)
    {
        tileMap = new Dictionary<APoint, TileObject>();

        InitLevel(size, size, roomsCount);

        APoint point = GetRandomFloorTile();

        GameObject.Find("Hero").transform.position = new Vector3(point.x * TileSize, 0.0f, point.y * TileSize);
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Hero h = (Hero)player.GetComponent<Hero>();

        for (int i = 0; i < enemiesNumber; ++i)
        {
            APoint p = GetRandomFloorTile();

            if ((enemiesPrefabs != null) && (enemiesPrefabs.Count > 0))
            {
                Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)], new Vector3(p.x * TileSize, 0.0f, p.y * TileSize)
                       , Quaternion.identity);
            }
            else
            {
            Instantiate(Resources.Load("Prefabs/Mob"), new Vector3(p.x * TileSize, 0.0f, p.y * TileSize)
                        , Quaternion.identity);
            }
        }


        point = GetRandomFloorTile();

		/*
		if (!GameObject.Find ("ladder")) {
		}
			
        GameObject.Find("ladder").transform.position = new Vector3(point.x * TileSize, 0.1f, point.y * TileSize);
        
		*/
    }

	public void StartMap(int size, int roomsCount)
	{
		tileMap = new Dictionary<APoint, TileObject>();

		InitLevel(size, size, roomsCount);

		APoint point = GetRandomFloorTile();

		GameObject.Find("Hero").transform.position = new Vector3(point.x * TileSize, 0.0f, point.y * TileSize);
	}

    void Start () {
	
        /*
		tileMap = new Dictionary<APoint, TileObject>();

		InitLevel(30,30,10);

		APoint point = GetRandomFloorTile();

		GameObject.Find("Hero").transform.position = new Vector3 (point.x * TileSize, 0.0f, point.y * TileSize);
		//GameObject player = GameObject.FindGameObjectWithTag("Player");
		//Hero h = (Hero)player.GetComponent<Hero>();

		//добавляем злодеев
		for (int i = 0; i < 20; ++i)
		{
			APoint p = GetRandomFloorTile();
			Instantiate(Resources.Load("Prefabs/Mob"),  new Vector3 (p.x * TileSize, 0.0f, p.y * TileSize)
			            , Quaternion.identity);
		}

		/*
		List <APoint> floors = new List<APoint>();

		for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
			if (m_data[x,y] == Tile.Floor)
		{
			APoint p = new APoint();
			p.x = x;
			p.y = y;
			floors.Add(p);
		}


	
		APoint randomTile = floors[Random.Range(0, floors.Count-1)];

		h.SetCoordinates(randomTile.x, randomTile.y,0);
		*/
        
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
