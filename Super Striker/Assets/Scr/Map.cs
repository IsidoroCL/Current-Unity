using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	int width = 25;
	int height = 12;

	public float xOffset = 1.23f;
	public float yOffset = 1.42f;

	//Matriz para almacenar las casillas del campo. X e Y marca el índice en el que se encuentra
	public Hex[,] campo;

	// Use this for initialization
	void Awake () {
		campo = new Hex[width, height];
	
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				float yPos = y * yOffset;

				// Are we on an odd row?
				if( x % 2 == 1 ) {
					yPos += yOffset/2f;
				}

				GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(x * xOffset, yPos , 0 ), Quaternion.identity  );

				// Name the gameobject something sensible.
				hex_go.name = "Hex_" + x + "_" + y;

				// Make sure the hex is aware of its place on the map
				hex_go.GetComponent<Hex>().x = x;
				hex_go.GetComponent<Hex>().y = y;

				// For a cleaner hierachy, parent this hex to the map
				hex_go.transform.SetParent(this.transform);

				// TODO: Quill needs to explain different optimization later...
				hex_go.isStatic = true;

				//Introducimos la casilla dentro de la matriz en su posicion
				campo[x,y] = hex_go.GetComponent<Hex>();

			}
		}

	}

	public Hex CasillaCentral()
    {
		int centroX = Mathf.FloorToInt(width / 2);
		int centroY = Mathf.FloorToInt(height / 2);
		return campo[centroX, centroY];
    }

}
