using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private GameObject focusObs;
    [SerializeField] private Camera playerCamera;
    public int towerCost;

    void Update()
    {
        if (focusObs != null)
        {
            // Toren volgt de muis
            Ray camray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camray, out RaycastHit hitInfo, 100000f))
            {
                Vector3 previewPos = hitInfo.point;
                previewPos.y += 0.5f; // Zorg dat toren boven grond zweeft
                focusObs.transform.position = previewPos;
                focusObs.GetComponent<BoxCollider>().enabled = false;
            }

            // ✅ Linkermuisklik = Plaatsen op Platform
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Platform") && hit.normal == Vector3.up)
                    {
                        Vector3 placePos = hit.collider.transform.position;
                        placePos.y += 0.5f;
                        focusObs.transform.position = placePos;

                        focusObs.GetComponent<BoxCollider>().enabled = true;
                      
                        focusObs = null;
                    }
                    else
                    {
                        // Geen geldig oppervlak
                        Destroy(focusObs);
                        focusObs = null;
                    }
                }
            }

            // ❌ Rechtermuisklik = Annuleren
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(focusObs);
                focusObs = null;
            }
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        focusObs = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }
}
