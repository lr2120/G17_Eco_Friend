using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTileInteraction : MonoBehaviour
{
    public Tilemap gameTilemap; // Assign this in the Unity Editor

    public TileBase ecoTileset_GreenShrub; // Assign in Unity Editor
    public TileBase ecoTileset_Planted;    // Assign in Unity Editor
    public TileBase ecoTileset_Watered;    // Assign in Unity Editor
    public TileBase ecoTileset_Seed;       // Assign in Unity Editor


    private PlayerHealthAndScore playerScoreScript; // Reference to the PlayerHealthAndScore script

    private bool hasSeed = false;
    private bool hasWater = false;
    private int score = 0;
    private int totalSeeds = 0;

    void Start()
    {
        playerScoreScript = GetComponent<PlayerHealthAndScore>();
        if (playerScoreScript == null)
        {
            Debug.LogError("PlayerHealthAndScore script not found on the player.");
        }
        CountInitialSeeds();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Assuming interaction is on Space key press
        {
            Vector3Int cellPosition = gameTilemap.WorldToCell(transform.position);
            TileBase currentTile = gameTilemap.GetTile(cellPosition);

            // Collect Seed
            if (currentTile.name == "EcoTileset_Seed" && !hasSeed)
            {
                hasSeed = true;
                ChangeTile(cellPosition, ecoTileset_GreenShrub); // Change to shrub (collider tile)
            }
            // Collect Water
            else if (currentTile.name == "EcoTileset_Water" && !hasWater)
            {
                hasWater = true;
                ChangeTile(cellPosition, ecoTileset_GreenShrub); // Change to shrub (collider tile)
            }
            // Plant Seed
            else if (currentTile.name == "EcoTileset_NotPlanted" && hasSeed)
            {
                hasSeed = false;
                ChangeTile(cellPosition, ecoTileset_Planted); // Change to planted tile
            }
            // Water Plant
            else if (currentTile.name == "EcoTileset_Planted" && hasWater)
            {
                hasWater = false;
                ChangeTile(cellPosition, ecoTileset_Watered); // Change to watered tile
                UpdateScore(); // Update the score
            }
        }
    }

    private void CountInitialSeeds()
    {
        BoundsInt bounds = gameTilemap.cellBounds;
        TileBase tile;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tile = gameTilemap.GetTile(pos);
                if (tile != null && tile == ecoTileset_Seed)
                {
                    totalSeeds++;
                }
            }
        }

        Debug.Log("Total Seeds: " + totalSeeds);
    }


    void ChangeTile(Vector3Int position, TileBase newTile)
    {
        gameTilemap.SetTile(position, newTile);
    }

    void UpdateScore()
    {
        score++;
        if (playerScoreScript != null)
        {
            playerScoreScript.AddPoints(1); // Assuming 1 point per action, adjust as needed
        }

        if (score >= totalSeeds)
        {
            // Logic for when all seeds are collected and planted
            Debug.Log("All seeds collected and planted!");
        }
    }
}
