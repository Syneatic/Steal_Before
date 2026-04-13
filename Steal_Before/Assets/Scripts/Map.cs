using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Public classes
    public int width;
    public int height;
    public GameObject tile;
    // Private classes
    GameObject [,] mapArray;

    // Start is called before the first frame update
    void Start()
    {
        mapArray = new GameObject [width, height];

        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                if(i % 2 == 0 && j % 2 == 0)
                {
                    mapArray[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
                }
                else
                {
                    mapArray[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
                    mapArray[i, j].GetComponent<SpriteRenderer>().color = Color.black;
                }

                if (i % 2 == 1 && j % 2 == 1)
                {
                    mapArray[i, j] = Instantiate(tile, new Vector3(i, j, 0), Quaternion.identity);
                }
            }
        }

        Debug.Log("mapArray has a totoal of " + mapArray.Length + " stored data");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
