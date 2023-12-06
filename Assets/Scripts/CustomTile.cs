using UnityEngine;

[CreateAssetMenu(menuName = "CustomTile/Tile", fileName = "NewCustomTile")]

public class CustomTile : ScriptableObject
{
    public bool isSeedTile;
    public bool isWaterTile;
    public bool isPlantingTile;
    public bool isColliderTile;
}


