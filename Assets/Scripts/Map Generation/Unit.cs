using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public int tileX;
    public int tileZ;
    public TileMap map;
    public float speed = 5f;
    [SerializeField]
    public int MaxMovement = 5;
    public int AttackRange = 1;

    int tileType = 0;
    [SerializeField]
    public int health = 1;
    [SerializeField]
    public int Damage = 1;
    int currentNode = 0;
    bool tileTypeGotten = false;

    public  List<Node> currentPath = null;
    public GameObject Enemy;

    void Update()
    {
        if(health <= 0)
        {
            map.tiles[tileX, tileZ] = tileType;
            map.AllUnitList.Remove(this.transform.gameObject);
            transform.position = Vector3.Lerp(transform.position, new Vector3(tileX, -7, tileZ), 0.5f * Time.deltaTime);
            this.tag = "Untagged";
        }
        else if (map.selectedUnit.GetComponent<Unit>() != this)
        {
            if (tileTypeGotten == false)
            {
                tileType = map.tiles[tileX, tileZ];
                tileTypeGotten = true;
            }
            map.tiles[tileX, tileZ] = 2;
        }

        if (map.selectedUnit.GetComponent<Unit>() == this)
        {
            if (tileType != 100)
            {
                map.tiles[tileX, tileZ] = tileType;
            }
            if (currentPath != null)
            {
                while (currentNode < currentPath.Count - 1)
                {
                    Vector3 start = map.TileToWorld(currentPath[currentNode].x, currentPath[currentNode].z);
                    Vector3 end = map.TileToWorld(currentPath[currentNode + 1].x, currentPath[currentNode + 1].z);

                    currentNode += 1;
                    if (currentPath == null)
                    {
                        break;
                    }

                }
            }
        }
        if (Vector3.Distance(transform.position, map.TileToWorld(tileX, tileZ)) < 0.1f)
        {
            MoveToNext();
        }
        transform.position = Vector3.Lerp(transform.position, map.TileToWorld(tileX, tileZ), speed * Time.deltaTime);
        StartCoroutine(Battle());
    }

    void MoveToNext()
    {
        if(currentPath == null || currentPath.Count <= 0)
        {
            if (Enemy != null)
            {
                Vector3 LookTarget = new Vector3(Enemy.GetComponent<Unit>().tileX, this.transform.GetChild(0).transform.position.y, Enemy.GetComponent<Unit>().tileZ);
                this.transform.GetChild(0).transform.LookAt(LookTarget);
                Enemy.GetComponent<Unit>().health -= this.Damage;
                Enemy = null;
                ChangeUnit();
            }
            return;
        }


        if (currentPath != null)
        {
            currentPath.RemoveAt(0);

            if (currentPath.Count >= 1)
            {
                LookToNext();
            }

            if (currentPath.Count == 1)
            {

                currentNode = 0;
                currentPath = null;

                if (Enemy == null)
                {
                    ChangeUnit();
                }
            }
        }
    }

    public void ChangeUnit()
    {
        GameObject Unit = map.AllUnitList[0];
        map.AllUnitList.RemoveAt(0);
        tileTypeGotten = false;
        map.AllUnitList.Add(Unit);
        map.selectedUnit = map.AllUnitList[0];
    }

    void LookToNext()
    {
        Vector3 past = new Vector3(tileX, this.transform.GetChild(0).transform.position.y, tileZ);
        tileX = currentPath[0].x;
        tileZ = currentPath[0].z;
        Vector3 target = new Vector3(tileX, this.transform.GetChild(0).transform.position.y, tileZ);
        this.transform.GetChild(0).transform.LookAt(target);
    }

    IEnumerator Battle()
    {
        if (Enemy != null && currentPath == null || Enemy !=null && Vector3.Distance(this.transform.position, Enemy.transform.position) <= AttackRange)
        {
            //Debug.Log(Vector3.Distance(this.transform.position, Enemy.transform.position));
            if (Enemy != null)
            {
                if (currentPath != null)
                {
                    currentPath = new List<Node>();
                    Vector3 LookTarget = new Vector3(Enemy.transform.transform.GetChild(0).position.x, this.transform.GetChild(0).transform.position.y, Enemy.transform.transform.GetChild(0).position.z);
                    this.transform.GetChild(0).transform.LookAt(LookTarget);

                    yield return new WaitForSeconds(0.5f);
                    if (Enemy != null)
                    {
                        Vector3 EnemyTarget = new Vector3(tileX, Enemy.transform.position.y, tileZ);
                        Enemy.transform.LookAt(EnemyTarget);

                        yield return new WaitForSeconds(0.5f);
                    }
                    if (Enemy != null)
                    {
                        Enemy.GetComponent<Unit>().health -= this.Damage;
                        Enemy = null;
                    }
                }
                else
                {
                    Vector3 LookTarget = new Vector3(Enemy.transform.transform.GetChild(0).position.x, this.transform.GetChild(0).transform.position.y, Enemy.transform.transform.GetChild(0).position.z);
                    this.transform.GetChild(0).transform.LookAt(LookTarget);

                    yield return new WaitForSeconds(0.5f);
                    if (Enemy != null)
                    {
                        Vector3 EnemyTarget = new Vector3(tileX, Enemy.transform.position.y, tileZ);
                        Enemy.transform.LookAt(EnemyTarget);

                        yield return new WaitForSeconds(0.5f);
                    }
                    if (Enemy != null)
                    {
                        Enemy.GetComponent<Unit>().health -= this.Damage;
                        Enemy = null;
                    }
                }

            }

        }
        map.EndGame(this.transform.gameObject);
    }

}
