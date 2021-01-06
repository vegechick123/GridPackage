using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GText : GTower
{
    public Transform decoration;
    public string text { get { return GetComponent<TextMesh>().text; } }
    public override void BePickUp(Player player)
    {
        base.BePickUp(player);
        player.PickUp(ProjectileType.Text,_Color.orgin);
        player.conveyObject.GetComponent<TextMesh>().text = text;
        LevelNumber levelNumber = GetComponent<LevelNumber>();
        if (levelNumber)
        {
            LevelNumber target = player.conveyObject.GetComponent<LevelNumber>();
            target.levelNumber= levelNumber.levelNumber;
            target.targetLocation = levelNumber.targetLocation;
        }
        if (decoration!=null)
            decoration.parent = transform.parent;
        Destroy(gameObject);
    }
}
