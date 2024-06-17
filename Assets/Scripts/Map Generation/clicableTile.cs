using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicableTile : MonoBehaviour
{
    public int tileX;
    public int tileZ;
    public TileMap map;
    public int UnitID;

    void OnMouseUp()
    {
        if (map.GameInitiated)
        {
            map.MoveUnitTo(tileX, tileZ);
            map.AbilityAssist = new Vector2(tileX, tileZ);
        }
        else
        {
            map.SummonPos = this.transform.position;
        }
    }

    /*void OnMouseOver()
    {
        map.MoveUnitTo(tileX, tileZ);
    }*/
}
