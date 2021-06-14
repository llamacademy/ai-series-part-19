using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProgressiveLevelEnemySpawner))]
public class ProgressiveLevelEnemySpawnerEditor : Editor
{
    public void OnSceneGUI()
    {
        GUIStyle Style = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            fontSize = 14
        };
        ProgressiveLevelEnemySpawner spawner = (ProgressiveLevelEnemySpawner)target;
        if (spawner == null)
        {
            return;
        }

        foreach (KeyValuePair<Vector3, int> keyValuePair in spawner.SpawnedTilesToEnemiesMap)
        {
            if (keyValuePair.Value > 0)
            {
                Handles.color = Color.green;
            }
            else
            {
                Handles.color = Color.red;
            }
            Handles.DrawWireCube(keyValuePair.Key * ProgressiveLevelEnemySpawner.TileSize, new Vector3(ProgressiveLevelEnemySpawner.TileSize, 2, ProgressiveLevelEnemySpawner.TileSize));
            Handles.Label(keyValuePair.Key * ProgressiveLevelEnemySpawner.TileSize, $"Position:\r\n{keyValuePair.Key}\r\nEnemies: {keyValuePair.Value}", Style);
        }
    }
}
