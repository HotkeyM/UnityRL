using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorRendererScript : MonoBehaviour {

	public List<Vector3> newVertices = new List<Vector3>();

	public List<int> newTriangles = new List<int>();

	public List<Vector2> newUV = new List<Vector2>();

	public int numTiles = 1;

	private Mesh mesh;

	private MeshCollider collider;

	private int squareCount = 0;

	public void GenerateMesh(Map.Tile [,] m_data, Map.Tile type, float height = 0.0f, float scale = 1.0f)
	{
		mesh = GetComponent<MeshFilter> ().mesh;
		collider = GetComponent<MeshCollider>();


		squareCount = 0;

		int m_width = m_data.GetLength(0);
		int m_height = m_data.GetLength(1);

		for (int x = 0; x < m_width; x++) for (int y = 0; y < m_height; y++)
		{
			if (m_data[x,y] == type)
			{
				newVertices.Add( new Vector3 (x*scale - 0.5f*scale  ,  height , y*scale - 0.5f*scale ));
				newVertices.Add( new Vector3 (x*scale + 0.5f*scale , height , y*scale - 0.5f*scale));
				newVertices.Add( new Vector3 (x*scale + 0.5f*scale , height, y*scale + 0.5f*scale));
				newVertices.Add( new Vector3 (x*scale - 0.5f*scale , height, y*scale+ 0.5f*scale));

				newTriangles.Add(squareCount*4);
				newTriangles.Add((squareCount*4)+3);
				newTriangles.Add((squareCount*4)+1);
				newTriangles.Add((squareCount*4)+1);
				newTriangles.Add((squareCount*4)+3);
				newTriangles.Add((squareCount*4)+2);

				int tileIndex = 0;
                    //tileIndex = Mathf.RoundToInt(Mathf.Sqrt(Random.Range(0, numTiles * numTiles)));
                    //tileIndex = Mathf.FloorToInt(Mathf.Sqrt(Random.Range(0.0f, numTiles * numTiles * 1.0f)));

                    tileIndex = Mathf.RoundToInt(Mathf.Sqrt(Random.Range(0, numTiles * numTiles)*1.0f));

				int squareSize = Mathf.FloorToInt(Mathf.Sqrt(numTiles));


				Debug.Log("square size = " + squareSize.ToString());
				Debug.Log("tileIndex = " + squareSize.ToString());


                    switch (tileIndex)
                    {
                            case (0):
                            newUV.Add(new Vector2(.0f, .0f));
                            newUV.Add(new Vector2(.5f, .0f));
                            newUV.Add(new Vector2(.5f, 0.5f));
                            newUV.Add(new Vector2(.0f, 0.5f));

                            break;
                            case (1):
                            newUV.Add(new Vector2(.5f, .0f));
                            newUV.Add(new Vector2(1.0f, .0f));
                            newUV.Add(new Vector2(1.0f, 0.5f));
                            newUV.Add(new Vector2(.5f, 0.5f));

                            break;
                            case (2):
                            newUV.Add(new Vector2(.0f, .5f));
                            newUV.Add(new Vector2(.5f, .5f));
                            newUV.Add(new Vector2(.5f, 1.0f));
                            newUV.Add(new Vector2(.0f, 1.0f));

                            break;
                            case (3):
                            newUV.Add(new Vector2(.0f, .5f));
                            newUV.Add(new Vector2(.5f, .5f));
                            newUV.Add(new Vector2(.5f, 1.0f));
                            newUV.Add(new Vector2(.0f, 1.0f));

                            break;
                            default:
                            newUV.Add(new Vector2(.5f, .5f));
                            newUV.Add(new Vector2(1.0f, .5f));
                            newUV.Add(new Vector2(1.0f, 1.0f));
                            newUV.Add(new Vector2(.5f, 1.0f));
                            break;

                        }


                    //newUV.Add(new Vector2((tileIndex % squareSize)/squareSize, (tileIndex/squareSize)/squareSize));
                    //newUV.Add(new Vector2(((tileIndex + 1) % squareSize)/squareSize, (tileIndex/squareSize)/squareSize));
                    //newUV.Add(new Vector2(((tileIndex + 1) % squareSize)/ squareSize, (tileIndex / squareSize + 1)/squareSize));
                    //    newUV.Add(new Vector2((tileIndex % squareSize)/squareSize, (tileIndex/squareSize + 1)/squareSize));

                    //newUV.Add(new Vector2 (0.0f, 0.0f));
                    //newUV.Add(new Vector2 (1.0f, 0.0f));
                    //newUV.Add(new Vector2 (1.0f, 1.0f));
                    //newUV.Add(new Vector2 (0.0f, 1.0f));



                    /*
                    newUV.Add(new Vector2(.0f, .0f));
                    newUV.Add(new Vector2(.5f, .0f));
                    newUV.Add(new Vector2(.5f, 0.5f));
                    newUV.Add(new Vector2(.0f, 0.5f));
                    */
                    /*
                    newUV.Add(new Vector2(.5f, .0f));
                    newUV.Add(new Vector2(1.0f, .0f));
                    newUV.Add(new Vector2(1.0f, 0.5f));
                    newUV.Add(new Vector2(.5f, 0.5f));
                    */
                    /*
                    newUV.Add(new Vector2(.0f, .5f));
                    newUV.Add(new Vector2(.5f, .5f));
                    newUV.Add(new Vector2(.5f, 1.0f));
                    newUV.Add(new Vector2(.0f, 1.0f));
                    */
                    /*
                    newUV.Add(new Vector2(.5f, .5f));
                    newUV.Add(new Vector2(1.0f, .5f));
                    newUV.Add(new Vector2(1.0f, 1.0f));
                    newUV.Add(new Vector2(.5f, 1.0f));
                    */

                    squareCount++;
			}
		}

		mesh.Clear ();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();

		Mesh phMesh = mesh;

		phMesh.RecalculateBounds();
		collider.sharedMesh = phMesh;

		squareCount=0;
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
