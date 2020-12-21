using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Tilemaps;
using UnityEditor;

[CreateAssetMenu(fileName = "GActorBrush", menuName = "2D Extras/Brushes/GActor Brush", order = 359)]
[CustomGridBrush(false, true, false, "GActor Brush")]
public class GActorBrush : PrefabRandomBrush
{
    [SerializeField] protected float[] probability;
    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31 || brushTarget == null)
        {
            return;
        }

        var objectsInCell = GetObjectsInCell(grid, brushTarget.transform, position);
        var existPrefabObjectInCell = objectsInCell.Any(objectInCell =>
        {
            return m_Prefabs.Any(prefab => PrefabUtility.GetCorrespondingObjectFromSource(objectInCell) == prefab);
        });

        if (!existPrefabObjectInCell)
        {
           
            int index=-1;
            if(probability.Length==0)
                index= Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset) * m_Prefabs.Length), 0, m_Prefabs.Length - 1);
            else if(probability.Length==m_Prefabs.Length)
            {
                float rand = GetPerlinValue(position, m_PerlinScale, k_PerlinOffset);
                float sum = 0;
                foreach (float x in probability)
                {
                    sum += x;
                }
                rand *= sum;
                sum = 0;
                for(int i=0;i<probability.Length;i++)
                {
                    sum += probability[i];
                    if(rand<sum)
                    {
                        index = i;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("概率数量与预制体数量不匹配");
            }
            var prefab = m_Prefabs[index];
            var instance = base.InstantiatePrefabInCell(grid, brushTarget, position, prefab);
            GActor actor = instance.GetComponent<GActor>();
            if (actor == null)
            {
                Debug.LogError("非GActor");
            }
            else
            {
                actor.location = new Vector2Int(position.x, position.y);
            }
        }

    }
}
