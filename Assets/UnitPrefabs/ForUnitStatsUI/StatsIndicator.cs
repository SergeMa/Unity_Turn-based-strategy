using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsIndicator : MonoBehaviour
{
    public GameObject Unit;
    public int height = 1;
    public Vector3 InterfaceScale = new Vector3(0.125f, 0.125f, 0.08f);
    private Vector3 Camera;
    private GameObject Cam;

    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        Camera = new Vector3(Cam.transform.position.x, (-1) * Cam.transform.position.y, Cam.transform.position.z);
    }

    void Update()
    {
        if (Unit != null)
        {
            this.transform.position = new Vector3(Unit.transform.position.x, Unit.transform.position.y + height, Unit.transform.position.z);
            this.transform.LookAt(Camera);
            this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().SetText(Unit.GetComponent<Unit>().health.ToString());
            this.transform.GetChild(1).GetChild(0).GetComponent<TextMeshPro>().SetText(Unit.GetComponent<Unit>().Damage.ToString());
            this.transform.GetChild(2).GetChild(0).GetComponent<TextMeshPro>().SetText(Unit.GetComponent<Unit>().MaxMovement.ToString());

            if (Unit != Unit.GetComponent<Unit>().map.selectedUnit && Unit.tag == Unit.GetComponent<Unit>().map.selectedUnit.tag)
            {
                Vector3 LowerPosition = new Vector3(Unit.transform.position.x, Unit.transform.position.y - height, Unit.transform.position.z);
                transform.position = Vector3.Lerp(Unit.transform.position, LowerPosition, 2f);
            }

            else if (Unit.tag != Unit.GetComponent<Unit>().map.selectedUnit.tag && Unit == Unit.GetComponent<Unit>().map.selectedUnit)
            {
                Vector3 UpperPosition = new Vector3(Unit.transform.position.x, Unit.transform.position.y + height, Unit.transform.position.z);
                transform.position = Vector3.Lerp(Unit.transform.position, UpperPosition, 2f);
            }

            if (Unit.GetComponent<Unit>().health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
