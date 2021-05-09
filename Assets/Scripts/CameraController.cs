using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        terrainCollider = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
        camera = GetComponent<Camera>();

        zoomDistance = -20f;
        defaultOffset = new Vector3(0, 3, zoomDistance);
        offset = defaultOffset;



        Vector3 lookAtPosition = player.transform.position + lookAtOffset;
        Vector3 newCameraPos = lookAtPosition + offset;
        transform.position = newCameraPos;
        transform.LookAt(lookAtPosition);
        MovePlayer();
    }

    private TerrainCollider terrainCollider;
    private Camera camera;

    private float minZoomDistance = -5f;
    private float maxZoomDistance = -50f;
    private float zoomFactor = 3f;

    void LateUpdate()
    {
        float zoomDelta = Input.mouseScrollDelta.y;
        if(zoomDistance + zoomDelta < minZoomDistance && zoomDistance + zoomDelta > maxZoomDistance)
        {
            float zoomAmount = zoomDelta * zoomFactor;
            float newZoom = zoomDistance + zoomAmount;
            zoomDistance = Mathf.Min(Mathf.Max(newZoom, maxZoomDistance), minZoomDistance);
        }


        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            RevolveX();
            RevolveY();
        }

        if (Input.GetMouseButton(1))
        {
            MovePlayer();
        }

        float lerpSpeed = .1f;
        float rayStartFactor = 2;

        Ray lowerLeft = camera.ViewportPointToRay(new Vector3(0, 0, 0));
        lowerLeft.origin = lowerLeft.origin + new Vector3(-1, -1, 1) * rayStartFactor;

        Ray lowerRight = camera.ViewportPointToRay(new Vector3(1, 0, 0));
        lowerRight.origin = lowerRight.origin + new Vector3(-1, 1, 1) * rayStartFactor;

        Ray lowerCenter = camera.ViewportPointToRay(new Vector3(.5f, 0, 0));
        lowerCenter.origin = lowerCenter.origin + new Vector3(-1, 1, 1) * rayStartFactor;


        terrainCollider.Raycast(lowerLeft, out RaycastHit hitInfoLowerLeft, 1000f);
        terrainCollider.Raycast(lowerRight, out RaycastHit hitInfoLowerRight, 1000f);
        terrainCollider.Raycast(lowerCenter, out RaycastHit hitInfoLowerCenter, 1000f);
        if ((!hitInfoLowerLeft.collider?.name.Contains("Terrain") ?? true) || 
            (!hitInfoLowerRight.collider?.name.Contains("Terrain") ?? true) || 
            (!hitInfoLowerCenter.collider?.name.Contains("Terrain") ?? true))
        {
            offset += (Vector3.up) * 4f;
            lerpSpeed = .95f;
        }

        offset = Vector3.Lerp(offset, transform.forward * zoomDistance, lerpSpeed);

        Vector3 lookAtPosition = player.transform.position + lookAtOffset;
        Vector3 newCameraPos = lookAtPosition + offset;

        transform.position = Vector3.Slerp(transform.position, newCameraPos, lerpSpeed);
        transform.LookAt(lookAtPosition);
    }

    private void RevolveX()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
        offset = camTurnAngle * offset;
    }

    private void RevolveY()
    {
        float inputAmt = -Input.GetAxis("Mouse Y");

        if (transform.forward.y <= -.95f)
        {
            inputAmt = -.15f;
        }
        if (transform.forward.y >= 0)
        {
            inputAmt = .15f;
        }

        Quaternion camTurnAngle = Quaternion.AngleAxis(inputAmt * RotationSpeed, transform.right);
        offset = camTurnAngle * offset;
    }

    private void MovePlayer()
    {
        Vector3 directionToFace = transform.forward;
        directionToFace.y = 0;
        playerController.RotateToFace(directionToFace);
    }

    private Vector3 offset;
    private Vector3 defaultOffset;
    private Vector3 lookAtOffset = new Vector3(0, 3f, 0);

    private GameObject player;
    private PlayerController playerController;

    private float RotationSpeed = 7f;
    private float zoomDistance = -10f;
}
