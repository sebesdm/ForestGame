using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryEnforcer : MonoBehaviour
{
    public GameObject boundObject;

    void Start()
    {
        
    }

    void Update()
    {
        EnforceBoundariesFloor(gameObject, boundObject);
    }

    public static void EnforceBoundariesFloor(GameObject collidableObject, GameObject player)
    {
        BoxCollider floorCollider = collidableObject.GetComponent<BoxCollider>();
        Vector3 floorExtents = floorCollider.bounds.extents;
        Vector3 floorPosition = collidableObject.transform.position;
        Vector3 floorBordersMax = floorPosition + floorExtents;
        Vector3 floorBordersMin = floorPosition - floorExtents;

        BoxCollider playerCollider = player.GetComponent<BoxCollider>();
        Vector3 playerExtents = playerCollider.bounds.extents;
        Vector3 playerPosition = player.transform.position;
        Vector3 playerBordersMax = playerPosition + playerExtents;
        Vector3 playerBordersMin = playerPosition - playerExtents;

        if (playerBordersMax.x > floorBordersMax.x)
        {
            player.transform.position = new Vector3(floorBordersMax.x - playerExtents.x, player.transform.position.y, player.transform.position.z);
        }
        else if (playerBordersMin.x < floorBordersMin.x)
        {
            player.transform.position = new Vector3(floorBordersMin.x + playerExtents.x, player.transform.position.y, player.transform.position.z);
        }

        if (playerBordersMax.z > floorBordersMax.z)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, floorBordersMax.z - playerExtents.z);
        }
        else if (playerBordersMin.z < floorBordersMin.z)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, floorBordersMin.z + playerExtents.z);
        }
    }
}
