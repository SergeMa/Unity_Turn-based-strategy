using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeArmour : MonoBehaviour
{
    public TileMap map;
    public int ReturnedDamage = 2;

    private int CurrentHealth = 10;


    void Update()
    {
        if (map == null)
        {
            map = this.GetComponent<Unit>().map;
            CurrentHealth = this.GetComponent<Unit>().health;
        }

        if (this.GetComponent<Unit>().health > 0 && CurrentHealth != this.GetComponent<Unit>().health)
        {
            Debug.Log("CounterAttack");
            map.AllUnitList[map.AllUnitList.Count - 1].GetComponent<Unit>().health -= ReturnedDamage;
            CurrentHealth = this.GetComponent<Unit>().health;
        }
    }
}
