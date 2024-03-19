using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    public GameObject[] starPrefabs;
    public GameObject earth;
    public float cloudInterval = 2f;
    public float cloudMovementSpeed = 5f;
    public float starMovementSpeed = 1f;
    public float despawnDistance = 2f;
    public Color initialSkyColor = Color.blue;
    public Color endSkyColor = Color.black;
    public float skyChangeDuration;
    public GameObject sky;

    private Camera mainCamera;
    private float skyChangeTimer = 0f;
    private Color currentColor;
    private SpriteRenderer skySprite;

    private List<GameObject> clouds;
    private List<GameObject> stars;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        currentColor = initialSkyColor;
        skySprite = sky.GetComponent<SpriteRenderer>();

        clouds = new();
        stars = new();

        StartCoroutine(moveEarth());
    }

    IEnumerator moveEarth()
    {
        while (earth != null) {
            earth.transform.Translate(Vector3.down * 1f * Time.deltaTime);
            if (earth.transform.position.y < mainCamera.ViewportToWorldPoint(Vector3.zero).y - 10f)
            {
                Destroy(earth);
            }
            yield return null;
        }
        StartCoroutine(CreateBackgroundObjects());
    }

    IEnumerator CreateBackgroundObjects()
    {
        while (true)
        {
            float randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector2(0, 1)).x, 
                                        mainCamera.ViewportToWorldPoint(new Vector2(1, 1)).x);
            Vector3 spawnPoint = new Vector3(randomX, mainCamera.transform.position.y + 6, 1);

            if (GameManager.Instance.CurrentWaveIndex == 0)
            {
                SpawnCloud(spawnPoint);
            }
            else if (GameManager.Instance.CurrentWaveIndex == 1) 
            {
                SpawnStar(spawnPoint);
            }
            else 
            {
                SpawnStar(spawnPoint);
            }

            yield return new WaitForSeconds(cloudInterval);

        }
    }

    private GameObject SpawnCloud(Vector3 position)
    {
        var randomCloud = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        var cloudObj = Instantiate(randomCloud, position, Quaternion.identity);
        cloudObj.transform.SetParent(transform, true);
        clouds.Add(cloudObj);
        return cloudObj;
    }

    private GameObject SpawnStar(Vector3 position)
    {
        var randomStar = starPrefabs[Random.Range(0, starPrefabs.Length)];
        var starObj = Instantiate(randomStar, position, Quaternion.identity);
        starObj.transform.SetParent(transform, true);
        stars.Add(starObj);
        return starObj;
    }

    // Update is called once per frame
    void Update()
    {
        clouds.RemoveAll(obj => obj.IsDestroyed());
        foreach (var cloud in clouds)
        {
            cloud.transform.Translate(cloudMovementSpeed * Time.deltaTime * Vector3.down);

            if (cloud.transform.position.y < mainCamera.ViewportToWorldPoint(Vector3.zero).y - despawnDistance)
            {
                Destroy(cloud);
            }
        }

        stars.RemoveAll(obj => obj.IsDestroyed());
        foreach (var star in stars)
        {
            star.transform.Translate(starMovementSpeed * Time.deltaTime * Vector3.down);

            if (star.transform.position.y < mainCamera.ViewportToWorldPoint(Vector3.zero).y - despawnDistance)
            {
                Destroy(star);
            }
        }

        if (skyChangeTimer < skyChangeDuration)
        {
            currentColor = Color.Lerp(initialSkyColor, endSkyColor, skyChangeTimer / skyChangeDuration);
            skySprite.color = currentColor;

            skyChangeTimer += Time.deltaTime;
        }   
    }
}
