using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapShadowGen : MonoBehaviour
{
    [SerializeField] private GameObject _castCube;
    [SerializeField] private string _wallName;

    private void Start()
    {
        var tilemap = GetComponent<Tilemap>();
        var bounds = tilemap.cellBounds;

        for (int y = bounds.yMin; y < bounds.yMax + 1; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax + 1; x++)
            {
                var tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                if (tile == null)
                    continue;

                if (tile.name == _wallName || (tilemap.GetTile(new Vector3Int(x, y + 1, 0)) != null && tilemap.GetTile(new Vector3Int(x, y + 2, 0)) != null))
                {
                    var cube = Instantiate(_castCube,
                        new Vector3(tilemap.transform.position.x + tilemap.cellSize.x * x + 0.5F, tilemap.transform.position.y + tilemap.cellSize.y * y + 0.5F),
                        Quaternion.identity);

                    cube.transform.localScale = new Vector3(tilemap.cellSize.x, tilemap.cellSize.y, cube.transform.localScale.z);
                }    
            }
        }
    }
}
