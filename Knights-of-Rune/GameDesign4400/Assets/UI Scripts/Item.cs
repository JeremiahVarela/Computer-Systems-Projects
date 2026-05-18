using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject {
    // Only gameplay
    public TileBase tile;
    public ItemType type;          // <— use this for slot filtering
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    // Only UI
    public bool stackable = true;  // <— use this for stacking
    // (OPTIONAL) add a max stack:
    public int maxStack = 99;

    // Both
    public Sprite image;
}

public enum ItemType { BuildingBlock, Tool }
public enum ActionType { Dig, Mine }
