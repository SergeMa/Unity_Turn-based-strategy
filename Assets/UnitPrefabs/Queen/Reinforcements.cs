using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforcements : MonoBehaviour
{
    public TileMap map;
    public float OverlapSphereRadius = 4f;
    public int SummonedUnitsCount = 2;
    public GameObject SummonedModel;
    public int SummonedHealth = 3;
    public int SummonedDamage = 3;
    public int SummonedMovement = 5;
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
        }
        
        else if (this.GetComponent<Unit>().tileX == map.AbilityAssist.x && this.GetComponent<Unit>().tileZ == map.AbilityAssist.y && map.selectedUnit == this.gameObject && this.GetComponent<Unit>().health > 0 && map.GameInitiated)
        {
            Ability();
            this.GetComponent<Unit>().ChangeUnit();
        }
    }

    private void Ability()
    {
        Collider[] PossibleSpawnLocations = Physics.OverlapSphere(this.transform.position, OverlapSphereRadius, Ground);

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1f, Ground);
        List<Collider> PossibleSpawnLocationsList = new List<Collider>(PossibleSpawnLocations);

        List<Collider> LocationsToRemove  = new List<Collider>();
        foreach (Collider col in PossibleSpawnLocationsList)
        {
            if (map.tileTypes[map.tiles[(int)col.transform.position.x, (int)col.transform.position.z]].isWalkable == false)
            {
                LocationsToRemove.Add(col);
            }
        }
        foreach(Collider col in LocationsToRemove)
        {
            PossibleSpawnLocationsList.Remove(col);
        }

        for (int i = 0; i != SummonedUnitsCount; i++)
        {
            int SpawnIndex = Random.Range(0, PossibleSpawnLocationsList.Count - 1);
            GameObject SpawnPoint = PossibleSpawnLocationsList[SpawnIndex].gameObject;
            PossibleSpawnLocationsList.RemoveAt(SpawnIndex);

            Vector3 SpawnTarget = new Vector3(SpawnPoint.transform.position.x, 0f, SpawnPoint.transform.position.z);
            GameObject Unit = Instantiate(SummonedModel, SpawnTarget, Quaternion.identity);

            Unit.GetComponent<Unit>().map = this.GetComponent<Unit>().map;
            Unit.GetComponent<Unit>().health = SummonedHealth;
            Unit.GetComponent<Unit>().Damage = SummonedDamage;
            Unit.GetComponent<Unit>().MaxMovement = SummonedMovement;
            Unit.GetComponent<Unit>().tileX = (int)SpawnTarget.x;
            Unit.GetComponent<Unit>().tileZ = (int)SpawnTarget.z;
            SpawnTarget = SpawnTarget + new Vector3(0, 3, 0);

            Unit.transform.GetChild(0).GetComponent<StatsIndicator>().Unit = Unit;

            map.AllUnitList.Add(Unit);
        }
        Debug.Log(SummonedUnitsCount + " units summoned!");

    }
}
