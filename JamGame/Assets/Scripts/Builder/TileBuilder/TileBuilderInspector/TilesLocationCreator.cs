﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public partial class TileBuilderInspector
{
    private readonly int squareSideLength = 30;
    public partial void ShowLocationBuildingButtons(TileBuilder tileBuilder)
    {
        _ = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create random building"))
        {
            int x = 0;
            int y = 0;
            tileBuilder.DeleteAllTiles();
            for (int i = 0; i < squareSideLength * squareSideLength; i++)
            {
                float value = Random.value * 100;
                if (value < 50)
                {
                    tileBuilder.CreateTileAndBind(tileBuilder.FreespacePrefab, new(x, y), 0);
                }
                else if (value is > 50 and < 65)
                {
                    tileBuilder.CreateTileAndBind(tileBuilder.StairsPrefab, new(x, y), 0);
                }
                else if (value is > 65 and < 80)
                {
                    tileBuilder.CreateTileAndBind(tileBuilder.WindowPrefab, new(x, y), 0);
                }
                else if (value > 80)
                {
                    tileBuilder.CreateTileAndBind(tileBuilder.OutdoorPrefab, new(x, y), 0);
                }
                y++;
                if (y >= squareSideLength)
                {
                    y = 0;
                    x++;
                }
            }
        }
        if (GUILayout.Button("Create normal building"))
        {
            tileBuilder.CreateNormalBuilding();
        }
        EditorGUILayout.EndHorizontal();
        _ = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add choosed Tile"))
        {
            _ = tileBuilder.Execute(new AddTileToSceneCommand(tileBuilder.ChoosedTile));
        }
        if (GUILayout.Button("Clear Scene"))
        {
            tileBuilder.DeleteAllTiles();
        }
        EditorGUILayout.EndHorizontal();
        _ = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add 4 Tiles"))
        {
            tileBuilder.DeleteAllTiles();
            tileBuilder.CreateTileAndBind(tileBuilder.OutdoorPrefab, new(0, 0), 0);
            tileBuilder.CreateTileAndBind(tileBuilder.OutdoorPrefab, new(0, 1), 0);
            tileBuilder.CreateTileAndBind(tileBuilder.WorkingPlaceFree, new(1, 0), 0);
            tileBuilder.CreateTileAndBind(tileBuilder.WorkingPlace, new(1, 1), 0);
        }
        if (GUILayout.Button("Update All"))
        {
            tileBuilder.UpdateAllTiles();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif