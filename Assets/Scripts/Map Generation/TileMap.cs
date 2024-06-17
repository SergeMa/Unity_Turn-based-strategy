using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class TileMap : MonoBehaviour
{
    public GameObject selectedUnit = null;
    public bool GameInitiated = false;

    public TileType[] tileTypes;
    public int[,] tiles;
    Node[,] graph;

    public int mapX = 10;
    public int mapZ = 10;
    public bool FourWay = true;
    public List<GameObject> AllUnitList = null;
    //public List<GameObject> AllPossibleUnits;
    public GameObject EndScreenUI;


    public int PlayerBugdet = 5;
    private Transform Pointer;
    private List<Node> currentPath = null;
    private int Cost = -1;

    public GameObject PickedUnit;
    private GameObject AttackUI;
    private GameObject SelectedUI;
    public Vector3 SummonPos;
    public GameObject UnitInterface;

    public Vector2 AbilityAssist;
    GameObject EnemyUnit = null;
    //public bool GameStarted = false;

    void Start()
    {
        Pointer = this.transform.GetChild(0);
        AbilityAssist = new Vector2(100, 100);

        SelectUnit();
        //BuyPhase();
        GenerationData();
        ShowPossibleMoves();
        GenerateMap();
    }


    void Update()
    {
        Pointer.transform.position = new Vector3(selectedUnit.transform.position.x, 1f, selectedUnit.transform.position.z);
        if (selectedUnit != AllUnitList[0])
        {
            selectedUnit = AllUnitList[0];
        }
        if (GameInitiated == false)
        {
            SelectUnit();
            GameInitiated = true;
        }
    }

    void SelectUnit()
    {
        AllUnitList = new List<GameObject>();
        GameObject[] Team1Units;
        GameObject[] Team2Units;
        if (GameInitiated)
        {
            Team1Units = GameObject.FindGameObjectsWithTag("Team1");
            Team2Units = GameObject.FindGameObjectsWithTag("Team2");


            while (Team1Units.Length != 0 && Team2Units.Length != 0)
            {
                if(Team1Units.Length != 0)
                {
                    int Index = Random.Range(0, Team1Units.Length - 1);
                    AllUnitList.Add(Team1Units[Index]);
                    List<GameObject> Units1 = new List<GameObject>(Team1Units);
                    Units1.RemoveAt(Index);
                    Team1Units = Units1.ToArray();
                }
                if (Team2Units.Length != 0)
                {
                    int Index = Random.Range(0, Team1Units.Length - 1);
                    AllUnitList.Add(Team2Units[Index]);
                    List<GameObject> Units2 = new List<GameObject>(Team2Units);
                    Units2.RemoveAt(Index);
                    Team2Units = Units2.ToArray();
                }
            }
            foreach (GameObject Unit in AllUnitList)
            {
                Unit UnitData = Unit.GetComponent<Unit>();
                UnitData.tileX = (int)Unit.transform.position.x;
                UnitData.tileZ = (int)Unit.transform.position.z;
                UnitData.map = this;

                Instantiate(UnitInterface, new Vector3(0, 0, 0), Quaternion.identity);
                UnitInterface.GetComponent<StatsIndicator>().Unit = Unit;
            }
            selectedUnit = AllUnitList[0];
        }
    }

    /*private void BuyPhase()
    {
        for (int i = 0; i < AllPossibleUnits.Count - 1; i++)
        {
            Instantiate(AllPossibleUnits[i], new Vector3(-3, 0, i), Quaternion.identity);
        }
        int BuyingTeam = 0;
        int[] BudgetOfTeam = null;

        BudgetOfTeam.Append(PlayerBugdet);
        BudgetOfTeam.Append(PlayerBugdet);
        while (BudgetOfTeam[0] <= 0 && BudgetOfTeam[1] <= 0)
        {
            if(PickedUnit != null && SummonPos != null && SummonPos != new Vector3(100,100,100))
            {
                Instantiate(PickedUnit, SummonPos, Quaternion.identity);
                SummonPos = new Vector3(100,100,100);
                BudgetOfTeam[BuyingTeam] = BudgetOfTeam[BuyingTeam] - 1;
                if (BuyingTeam == 0)
                {
                    BuyingTeam = 1;
                }
                else
                {
                    BuyingTeam = 0;
                }
            }
            GameInitiated = true;
            break;
        }
    }*/

    public void EndGame(GameObject Unit)
    {
        string tag = Unit.tag;
        List<GameObject> Units = new List<GameObject>();
        foreach (GameObject unit in AllUnitList)
        {
            if (tag == unit.tag)
            {
                Units.Add(unit);
            }
        }
        if (Units.Count == AllUnitList.Count)
        {
            Debug.Log(tag + " wins!");
            EndScreenUI.SetActive(true);
            if (tag == "Team1")
            {
                EndScreenUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Red team wins!!!";
                EndScreenUI.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                EndScreenUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Blue team wins!!!";
                EndScreenUI.transform.GetChild(0).gameObject.SetActive(false);
            }
            foreach (GameObject unit in AllUnitList)
            {
                Destroy(unit.GetComponent<Unit>());
            }
            Pointer.gameObject.SetActive(false);
        }
    }

    float CostToEnterTile(int sourceX, int sourceZ, int targetX,int targetZ)
    {
        TileType tt = tileTypes[tiles[targetX, targetZ]];

        float cost = tt.movementCost;

        /*if(sourceX != targetX && sourceZ!= targetZ)
        {
            cost += 0.00001f;
        }*/
        if(tileTypes[tiles[targetX, targetZ]].isWalkable == false)
        {
            cost = Mathf.Infinity;
        }
        return cost;
    }

    void GenerationData()
    {
        tiles = new int[mapX, mapZ];
        for (int x = 0; x < mapX; x++)
        {
            for (int z = 0; z < mapZ; z++)
            {
                tiles[x, z] = Random.Range(0, 3);
            }
        }
        foreach (GameObject Unit in AllUnitList)
        {
            tiles[Unit.GetComponent<Unit>().tileX, Unit.GetComponent<Unit>().tileZ] = 0;
        }
    }
    

    
    void GenerateMap()
    {
        for (int x = 0; x < mapX; x++)
        {
            for (int z = 0; z < mapZ; z++)
            {
                TileType tt = tileTypes[tiles[x, z]];
                GameObject go = (GameObject)Instantiate(tt.prefab, new Vector3(x, 0, z), Quaternion.identity);

                clicableTile ct = go.GetComponent<clicableTile>();
                ct.tileX = x;
                ct.tileZ = z;
                ct.map = this;
            }
        }
    }

    public Vector3 TileToWorld(int x, int z)
    {
        return new Vector3(x, 0, z);
    }

    public void ShowPossibleMoves()
    {
        graph = new Node[mapX, mapZ];
        for (int x = 0; x < mapX; x++)
        {
            for (int z = 0; z < mapZ; z++)
            {
                graph[x, z] = new Node();
                graph[x, z].x = x;
                graph[x, z].z = z;
            }
        }
        for (int x = 0; x < mapX; x++)
        {
            for (int z = 0; z < mapZ; z++)
            {
                if (FourWay == true)
                {
                    if (x > 0)
                    {
                        graph[x, z].neighbours.Add(graph[x - 1, z]);
                    }
                    if (x < mapX - 1)
                    {
                        graph[x, z].neighbours.Add(graph[x + 1, z]);
                    }
                    if (z > 0)
                    {
                        graph[x, z].neighbours.Add(graph[x, z - 1]);
                    }
                    if (z < mapZ - 1)
                    {
                        graph[x, z].neighbours.Add(graph[x, z + 1]);
                    }
                }
                if (FourWay == false)
                {
                    if (x > 0)
                    {
                        graph[x, z].neighbours.Add(graph[x - 1, z]);
                        if (z > 0)
                        {
                            graph[x, z].neighbours.Add(graph[x-1, z - 1]);
                        }
                        if (z < mapZ - 1)
                        {
                            graph[x, z].neighbours.Add(graph[x-1, z + 1]);
                        }
                    }
                    if (x < mapX - 1)
                    {
                        graph[x, z].neighbours.Add(graph[x + 1, z]);
                        if (z > 0)
                        {
                            graph[x, z].neighbours.Add(graph[x+1, z - 1]);
                        }
                        if (z < mapZ - 1)
                        {
                            graph[x, z].neighbours.Add(graph[x+1, z + 1]);
                        }
                    }
                    if (z > 0)
                    {
                        graph[x, z].neighbours.Add(graph[x, z - 1]);
                    }
                    if (z < mapZ - 1)
                    {
                        graph[x, z].neighbours.Add(graph[x, z + 1]);
                    }
                }
            }

        }
    }

    public void MoveUnitTo(int x, int z)
    {
        if(tileTypes[tiles[x, z]].isWalkable == false && tileTaken(x,z) == false)
        {
            return;
        }
        if (tileTypes[tiles[x, z]].isWalkable == false && tileTaken(x,z) == true)
        {
            selectedUnit.GetComponent<Unit>().Enemy = EnemyUnit;
            EnemyUnit = null;
            InitiatePathfinding(x, z);
            selectedUnit.GetComponent<Unit>().currentPath.RemoveAt(selectedUnit.GetComponent<Unit>().currentPath.Count - 1);
        }
        else
        {
            InitiatePathfinding(x, z);
        }
    }

    private void InitiatePathfinding(int x, int z)
    {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        List<Node> unvisited = new List<Node>();
        Node target = graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileZ];
        Node source = graph[x, z];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0)
        {
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }

            }
            if (u == target)
            {
                break;
            }
            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + CostToEnterTile(u.x, u.z, v.x, v.z);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[target] == null)
        {
            return;
        }
        currentPath = new List<Node>();
        Node curr = target;
        Cost = 0;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
            for (int tile = 1; tile < currentPath.Count-3; tile++)
            {
                Cost += (int)CostToEnterTile(currentPath[tile].x, currentPath[tile].z, currentPath[tile + 1].x, currentPath[tile + 1].z);
                //Debug.Log(Cost);
            }
            if(selectedUnit.GetComponent<Unit>().Enemy != null)
            {
                Cost -= 1;
            }
            if (Cost > selectedUnit.GetComponent<Unit>().MaxMovement)
            {
                if(selectedUnit.GetComponent<Unit>().Enemy != null)
                {
                    selectedUnit.GetComponent<Unit>().Enemy = null;
                }
                //Debug.Log(Cost + " move than Unit Cost by " + (Cost - selectedUnit.GetComponent<Unit>().MaxMovement) + " points");
                break;
            }
            else
            {
                Cost = Cost + 1;
                selectedUnit.GetComponent<Unit>().currentPath = currentPath;
            }
        }
    }

    bool tileTaken(int x, int z)
    {
        bool isTrue = false;
        foreach (GameObject Unit in AllUnitList)
        {
            if (Unit.GetComponent<Unit>().tileX == x && Unit.GetComponent<Unit>().tileZ == z && selectedUnit.tag != Unit.tag)
            {
                isTrue = true;
                EnemyUnit = Unit;
                break;
            }
        }
        return isTrue;
    }
}
