using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[DisallowMultipleComponent]
public class EnemyBorn : MonoBehaviour
{
    public float standbyTime;
    //敌人起点
    public Vector2Int origin;
    //敌人终点
    public Vector2Int destination;
    [HideInInspector]
    public List<EnemyGroup> groups = new List<EnemyGroup>();

    int Sequence;

    [Serializable]
    public class EnemyGroup
    {
        public int id;
        public int num;
        public float refreshTime;

    }
    /*-----------------------敌人自动刷新-----------------------------*/
    private void Awake()
    {
        Sequence = 0;
        StartCoroutine(Refresh());
    }
    IEnumerator Refresh()
    {
        Sequence = 0;
        yield return new WaitForSeconds(standbyTime);
        while (true)
        {
            if (groups.Count >= Sequence + 1)
            {
                int Num = groups[Sequence].num;
                if (groups[Sequence].id == 0)
                {
                    yield return new WaitForSeconds(groups[Sequence].refreshTime);
                }
                else
                    while (Num > 0)
                    {
                        RefreshEnemyByEG(groups[Sequence]);
                        yield return new WaitForSeconds(groups[Sequence].refreshTime);
                        Num--;
                    }
                Sequence++;
            }
            else
                yield break;
        }
    }
    void RefreshEnemyByEG(EnemyGroup group)
    {
        if (group.num == 0)
            return;
        else
        {
            GameObject prefab = PrefabManager.instance.GetEnemyPrefabById(group.id);
            if (prefab != null)
            {
                Instantiate(prefab, this.transform);
            }
        }
    }
    /*----------------------------------------------------------------*/
    /// <summary>
    /// 仅供编辑器使用
    /// </summary>
    public void Add()
    {
        groups.Add(new EnemyGroup());
    }

}

