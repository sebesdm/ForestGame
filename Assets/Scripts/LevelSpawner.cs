using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public List<GameObject> rockPrefabs;
    public List<GameObject> liveTreePrefabs;
    public List<GameObject> deadTreePrefabs;
    public List<GameObject> brushPrefabs;
    public List<GameObject> grassPrefabs;
    public List<GameObject> animalPrefabs;
    public List<GameObject> mushroomPrefabs;
    public float spawnMultiplier;
    public float grassRatio;
    public float rockRatio;
    public float liveTreeRatio;
    public float deadTreeRatio;
    public float brushRatio;
    public float animalRatio;
    public float mushroomRatio;


    private TerrainCollider floorCollider;

    // Start is called before the first frame update
    void Start()
    {
        int grassCount = (int)(grassRatio * spawnMultiplier);
        int rockCount = (int)(rockRatio * spawnMultiplier);
        int liveTreeCount = (int)(liveTreeRatio * spawnMultiplier);
        int deadTreeCount = (int)(deadTreeRatio * spawnMultiplier);
        int brushCount = (int)(brushRatio * spawnMultiplier);
        int animalCount = (int)(animalRatio * spawnMultiplier);
        int mushroomCount = (int)(mushroomRatio * spawnMultiplier);

        floorCollider = GameObject.Find("Terrain").GetComponent<TerrainCollider>();

        for (int i = 0; i < grassCount; i++)
        {
            int nextObjectIndex = Random.Range(0, grassPrefabs.Count);

            GameObject go = Instantiate(grassPrefabs[nextObjectIndex], GetRandomPositionInLevel(), GetRandomRotation());
            go.transform.localScale = GetRandomSize(go.transform.localScale);
        }
        for (int i = 0; i < rockCount; i++)
        {
            int nextObjectIndex = Random.Range(0, rockPrefabs.Count);
            GameObject go = Instantiate(rockPrefabs[nextObjectIndex], GetRandomPositionInLevel(), GetRandomRotation());
            go.transform.localScale = GetRandomSize(go.transform.localScale);
        }
        for (int i = 0; i < liveTreeCount; i++)
        {
            int nextObjectIndex = Random.Range(0, liveTreePrefabs.Count);
            GameObject go = Instantiate(liveTreePrefabs[nextObjectIndex], GetRandomPositionInLevel(), GetRandomRotation());
            go.transform.localScale = GetRandomSize(go.transform.localScale);
        }
        for (int i = 0; i < deadTreeCount; i++)
        {
            int nextObjectIndex = Random.Range(0, deadTreePrefabs.Count);
            GameObject go = Instantiate(deadTreePrefabs[nextObjectIndex], GetRandomPositionInLevel(), GetRandomRotation());
            go.transform.localScale = GetRandomSize(go.transform.localScale);
        }
        for (int i = 0; i < brushCount; i++)
        {
            int nextObjectIndex = Random.Range(0, brushPrefabs.Count);
            GameObject go = Instantiate(brushPrefabs[nextObjectIndex], GetRandomPositionInLevel(), GetRandomRotation());
            go.transform.localScale = GetRandomSize(go.transform.localScale);
        }
        for (int i = 0; i < animalCount; i++)
        {
            int nextObjectIndex = Random.Range(0, animalPrefabs.Count);
            GameObject go = Instantiate(animalPrefabs[nextObjectIndex], GetRandomPositionInLevel(), GetRandomRotation());
            go.transform.localScale = GetRandomSize(go.transform.localScale);
        }
        for (int i = 0; i < mushroomCount; i++)
        {
            SpawnNewMushroom();
        }
    }

    private Vector3 GetRandomPositionInLevel()
    {
        RaycastHit hitInfo;

        float randX;
        float randZ;
        float y;

        do
        {
            var maxX = floorCollider.bounds.max.x;
            var minX = floorCollider.bounds.min.x;
            var maxZ = floorCollider.bounds.max.z;
            var minZ = floorCollider.bounds.min.z;

            randX = Random.Range(minX, maxX);
            randZ = Random.Range(minZ, maxZ);

            Physics.Raycast(new Vector3(randX, 1000f, randZ), Vector3.down, out hitInfo, 500000f);
            y = hitInfo.point.y - .1f;
        } while (!hitInfo.collider.name.Contains("Terrain"));

        return new Vector3(randX, y, randZ);
    }

    public Quaternion GetRandomRotation()
    {
        grassPrefabs[0].transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        return grassPrefabs[0].transform.rotation;
    }

    public Vector3 GetRandomSize(Vector3 baseSize)
    {
        float sizeScale = GetRandomSizeScale();
        return new Vector3(baseSize.x * sizeScale, baseSize.y * sizeScale, baseSize.z * sizeScale);
    }

    public float GetRandomSizeScale()
    {
        return Random.Range(.75f, 1.25f);
    }

    public void SpawnNewMushroom()
    {
        int nextObjectIndex = Random.Range(0, mushroomPrefabs.Count);
        Vector3 spawnPosition = GetRandomPositionInLevel();
        spawnPosition.y += .6f;
        Instantiate(mushroomPrefabs[nextObjectIndex], spawnPosition, mushroomPrefabs[nextObjectIndex].transform.rotation);
    }
}
