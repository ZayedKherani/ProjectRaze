using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> prefabTileset;
    Dictionary<int, GameObject> tile_groups;

    public GameObject prefab_forest;
    public GameObject prefab_hills;
    public GameObject prefab_mountains;
    public GameObject prefab_plains;
    public GameObject prefab_sand;
    public GameObject prefab_water;

    int map_width = 160;
    int map_height = 90;

    //List<List<int>> noise_grid = new List<List<int>>();
    //List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    List<List<(int, GameObject)>> noise_tile_grid = new List<List<(int, GameObject)>>();

    // recommend 4 to 20
    float magnification = 7.0f;

    int x_offset = 0; // <- +>
    int y_offset = 0; // v- +^

    GameObject CreateTile(int tile_id, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/

        GameObject tile_prefab = prefabTileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];

        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        return tile;
    }

    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/

        tile_groups = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> prefab_pair in prefabTileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/

        prefabTileset = new Dictionary<int, GameObject>();
        prefabTileset.Add(0, prefab_plains);
        prefabTileset.Add(1, prefab_forest);
        prefabTileset.Add(2, prefab_hills);
        prefabTileset.Add(3, prefab_mountains);
        prefabTileset.Add(4, prefab_sand);
        prefabTileset.Add(5, prefab_water);
    }

    int GetIdUsingPerlin(int x, int y)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/

        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification
        );
        float clamp_perlin = Mathf.Clamp01(raw_perlin); // Thanks: youtu.be/qNZ-0-7WuS8&lc=UgyoLWkYZxyp1nNc4f94AaABAg
        float scaled_perlin = clamp_perlin * prefabTileset.Count;

        // Replaced 4 with tileset.Count to make adding tiles easier
        if (scaled_perlin == prefabTileset.Count)
        {
            scaled_perlin = (prefabTileset.Count - 1);
        }

        return Mathf.FloorToInt(scaled_perlin);
    }

    void GenerateMap()
    {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/

        for (int x = 0; x < map_width; x++)
        {
            noise_tile_grid.Add(new List<(int, GameObject)>());

            for (int y = 0; y < map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                GameObject tile = CreateTile(tile_id, x, y);

                noise_tile_grid[x].Add((tile_id, tile));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
