using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheSky : MonoBehaviour
{
    public TileMap map;
    public int AbilityDamage = 1;
    public GameObject LightningEffect;

    void Update()
    {
        if(map == null)
        {
            map = this.GetComponent<Unit>().map;
        }
        if (map.selectedUnit.GetComponent<Unit>().tileX == map.AbilityAssist.x && map.selectedUnit.GetComponent<Unit>().tileZ == map.AbilityAssist.y && map.selectedUnit == this.gameObject && this.GetComponent<Unit>().health > 0)
        {
            Instantiate(LightningEffect, this.transform.position + new Vector3(0.5f,0,0.25f), Quaternion.Euler(new Vector3(180,0,0)));
            Instantiate(LightningEffect, this.transform.position + new Vector3(0, 0, 0.25f), Quaternion.Euler(new Vector3(180, 0, 0)));
            Instantiate(LightningEffect, this.transform.position + new Vector3(-0.25f, 0, 0), Quaternion.Euler(new Vector3(180, 0, 0)));
            Instantiate(LightningEffect, this.transform.position + new Vector3(0, 0, -0.25f), Quaternion.Euler(new Vector3(180, 0, 0)));
            
            StartCoroutine(Ability());
            StartCoroutine(ChangeUnit());
            this.GetComponent<Unit>().ChangeUnit();
        }

        IEnumerator Ability()
        {
            yield return new WaitForSeconds(1f);
            if (this.tag == "Team1")
            {
                GameObject[] Team1Units = GameObject.FindGameObjectsWithTag("Team2");
                foreach (GameObject unit in Team1Units)
                {
                    unit.GetComponent<Unit>().health -= AbilityDamage;
                    Instantiate(LightningEffect, unit.transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                }
            }
            if (this.tag == "Team2")
            {
                GameObject[] Team1Units = GameObject.FindGameObjectsWithTag("Team1");
                foreach (GameObject unit in Team1Units)
                {
                    unit.GetComponent<Unit>().health -= AbilityDamage;
                    Instantiate(LightningEffect, unit.transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                }
            }
            Debug.Log("Has dealt " + AbilityDamage + " damage");
            AbilityDamage += 1;

            yield return new WaitForSeconds(3f);

        }

        IEnumerator ChangeUnit()
        {
            yield return new WaitForSeconds(2f);
        }
    }
}
