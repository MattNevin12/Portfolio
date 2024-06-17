using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupFarmTiles : MonoBehaviour
{
    public Transform tileSpawnStartPos;
    public GameObject tile;

    public int lengthCount;
    public int depthCount;

    private void Start()
    {
        SpawnFarmTiles();
    }

    public void SpawnFarmTiles()
    {
        for (int i = 0; i < depthCount; i++)
        {
            for (int j = 0; j < lengthCount; j++)
            {
                // Don't put interactable tiles near the entrance/exit
                if (i < 3 && j > (lengthCount - 4))
                {

                }
                else
                {
                    Vector3 pos = new Vector3(tileSpawnStartPos.position.x + (j * 2), tileSpawnStartPos.position.y, tileSpawnStartPos.position.z + (i * 2));
                    GameObject newTile = Instantiate(tile, pos, transform.rotation);
                    newTile.transform.parent = transform.parent;
                    newTile.name = i + "." + j;

                    AssignTileID(newTile, i, j);
                }

            }
        }
    }

    private void AssignTileID(GameObject tile, int i, int j)
    {
        TileData td = tile.GetComponent<TileData>();
        td.ID = (i * 100) + j;
    }
}