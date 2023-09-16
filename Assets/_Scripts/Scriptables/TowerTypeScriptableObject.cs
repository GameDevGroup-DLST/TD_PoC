using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Type", menuName = "ScriptableObjects/GridObjects/Tower")]
public class TowerTypeScriptableObject : ScriptableObject
{
    public static Dir GetNextDir(Dir dir) => (Dir)(((int)(dir) + 1) % Enum.GetValues(typeof(Dir)).Length);
    public static Dir GetPrevDir(Dir dir) {
        int prev = (int)(dir) - 1;
        if(prev < 0) {
            prev = Enum.GetValues(typeof(Dir)).Length - 1;
        }
        return (Dir)(prev % Enum.GetValues(typeof(Dir)).Length);
    }

    public enum Dir {
        North, East, South, West
    }

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;
    public SpellScriptableObject spellToCast; // THIS NEEDS TO BE REPLACED WITH A TOWER SPELL SO
    public float maxHealth = 100f;

    public int GetRotationAngle(Dir dir) {
        switch(dir) {
            default:
            case Dir.South: return 0;
            case Dir.West:  return 90;
            case Dir.North: return 180;
            case Dir.East:  return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir) {
        switch(dir) {
            default:
            case Dir.South: return new Vector2Int(0, 0);
            case Dir.West:  return new Vector2Int(0, width);
            case Dir.North: return new Vector2Int(width, height);
            case Dir.East:  return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir) {
            default:
            case Dir.South:
            case Dir.North:
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++){
                        gridPositionList.Add(offset + new Vector2Int(x,y));
                    }
                }
                break;
            case Dir.East:
            case Dir.West:
                for (int x = 0; x < height; x++) {
                    for (int y = 0; y < width; y++){
                        gridPositionList.Add(offset + new Vector2Int(x,y));
                    }
                }
            break;
        }
        return gridPositionList;
    }
}
