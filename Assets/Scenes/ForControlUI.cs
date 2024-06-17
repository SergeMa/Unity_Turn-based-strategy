using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForControlUI : MonoBehaviour
{
    public Vector3 StartSize = new Vector3(0.125f,0.125f,0.125f/2f);
    public Camera camera;

    public void ShowOtherStats()
    {
        GameObject[] UnitInterfaces = GameObject.FindGameObjectsWithTag("UI");

        if (this.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text == "Show secondary stats")
        {
            foreach (GameObject UI in UnitInterfaces)
            {
                UI.transform.GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(1).gameObject.SetActive(false);
                UI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(2).gameObject.SetActive(true);
                UI.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(3).gameObject.SetActive(true);
            }
            this.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "Show primary stats";
        }
        else
        {
            foreach (GameObject UI in UnitInterfaces)
            {
                UI.transform.GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(1).gameObject.SetActive(true);
                UI.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(2).gameObject.SetActive(false);
                UI.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(3).gameObject.SetActive(false);
            }
            this.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "Show secondary stats";
        }
    }

    public void ChangeUISize()
    {
        GameObject[] UnitInterfaces = GameObject.FindGameObjectsWithTag("UI");
        if (this.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text == "Disable unit stats")
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Enable unit stats";
            foreach (GameObject UI in UnitInterfaces)
            {
                UI.transform.GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(1).gameObject.SetActive(false);
                UI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(2).gameObject.SetActive(false);
                UI.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(3).gameObject.SetActive(false);
                UI.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            this.transform.parent.GetChild(1).GetChild(0).gameObject.SetActive(true);
            this.transform.parent.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Disable unit stats";
            foreach (GameObject UI in UnitInterfaces)
            {
                UI.transform.GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(1).gameObject.SetActive(true);
                UI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(2).gameObject.SetActive(true);
                UI.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(3).gameObject.SetActive(true);
                UI.GetComponent<MeshRenderer>().enabled = true;

            }
            ShowOtherStats();
            ShowOtherStats();
        }
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray,out hit))
        {
            GameObject HitObj = hit.transform.gameObject;
            if(HitObj.tag == "AbilityInfo")
            {
                this.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "0";
            }
            else
            {
                this.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "1";
            }
        }
    }
}