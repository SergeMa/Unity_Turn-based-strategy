using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : MonoBehaviour
{

    public TileMap map;
    public float OverlapSphereRadius = 4f;
    public GameObject Unit;
    private bool IllusionSummoned;
    private GameObject UnitInterface;

    public LayerMask Ground;

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, OverlapSphereRadius);
    }

    void Update()
    {
        if (map == null)
        {
            map = this.GetComponent<Unit>().map;
            UnitInterface = map.UnitInterface;
        }

        if (map.selectedUnit.GetComponent<Unit>().tileX == map.AbilityAssist.x && map.selectedUnit.GetComponent<Unit>().tileZ == map.AbilityAssist.y && map.selectedUnit == this.gameObject && this.GetComponent<Unit>().health > 0)
        {
            Ability();
            this.GetComponent<Unit>().ChangeUnit();
        }
        if(Unit != null && map.selectedUnit == Unit && IllusionSummoned || this.GetComponent<Unit>().health == 0)
        {
            Unit.GetComponent<Unit>().health = 0;
            IllusionSummoned = false;
        }
        else if(Unit != null && Unit.GetComponent<Unit>().health <= 0)
        {
            IllusionSummoned = false;
        }
    }

    private void Ability()
    {
        Collider[] PossibleSpawnLocations = Physics.OverlapSphere(this.transform.position, OverlapSphereRadius, Ground);

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1f, Ground);
        List<Collider> PossibleSpawnLocationsList = new List<Collider>(PossibleSpawnLocations);
        GameObject Interface = Instantiate(UnitInterface, Vector3.zero, Quaternion.identity);

        List<Collider> LocationsToRemove = new List<Collider>();
        foreach (Collider col in PossibleSpawnLocationsList)
        {
            if (map.tileTypes[map.tiles[(int)col.transform.position.x, (int)col.transform.position.z]].isWalkable == false)
            {
                LocationsToRemove.Add(col);
            }
        }
        foreach (Collider col in LocationsToRemove)
        {
            PossibleSpawnLocationsList.Remove(col);
        }

        int SpawnIndex = Random.Range(0, PossibleSpawnLocations.Length - 1);
        GameObject SpawnPoint = PossibleSpawnLocations[SpawnIndex].gameObject;
        PossibleSpawnLocationsList.RemoveAt(SpawnIndex);

        int Change = Random.Range(1, 3);
        
        if (Change == 2) {
            Vector3 SpawnTarget = new Vector3(SpawnPoint.transform.position.x, 0f, SpawnPoint.transform.position.z);
            Unit = Instantiate(this.gameObject, SpawnTarget, this.transform.rotation);

            Unit.GetComponent<Unit>().map = this.GetComponent<Unit>().map;
            Unit.GetComponent<Unit>().health = this.GetComponent<Unit>().health;
            Unit.GetComponent<Unit>().Damage = this.GetComponent<Unit>().Damage;
            Unit.GetComponent<Unit>().tileX = (int)SpawnTarget.x;
            Unit.GetComponent<Unit>().tileZ = (int)SpawnTarget.z;

            SpawnTarget = SpawnTarget + new Vector3(0, 3, 0);
            Interface.GetComponent<StatsIndicator>().Unit = Unit;
        }
        else
        {
            Vector3 SpawnTarget = this.transform.position;
            Unit = Instantiate(this.gameObject, SpawnTarget, this.transform.rotation);
            
            Interface.GetComponent<StatsIndicator>().Unit = Unit;

            Unit.GetComponent<Unit>().map = this.GetComponent<Unit>().map;
            Unit.GetComponent<Unit>().health = this.GetComponent<Unit>().health;
            Unit.GetComponent<Unit>().Damage = this.GetComponent<Unit>().Damage;
            Unit.GetComponent<Unit>().tileX = this.GetComponent<Unit>().tileX;
            Unit.GetComponent<Unit>().tileZ = this.GetComponent<Unit>().tileZ;

            this.GetComponent<Unit>().tileX = (int)SpawnPoint.transform.position.x;
            this.GetComponent<Unit>().tileZ = (int)SpawnPoint.transform.position.z;
            

            Debug.Log("Positions changed"); 
        }
        
        map.AllUnitList.Add(Unit);
        Debug.Log("Illusion summoned!");
        IllusionSummoned = true;
    }
}
