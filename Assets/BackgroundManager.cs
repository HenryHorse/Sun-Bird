using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] clouds;
    public GameObject[] stars;
    public GameObject earth;
    public float cloudInterval = 2f;
    public float movementSpeed = 1f;
    public float despawnDistance = 2f;
    public Color initialSkyColor = Color.blue;
    public Color endSkyColor = Color.black;
    public float skyChangeDuration;
    public GameObject sky;

    private Camera mainCamera;
    private GameManager gameManager;
    private float skyChangeTimer = 0f;
    private Color currentColor;
    private SpriteRenderer skySprite;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        gameManager = GetComponent<GameManager>();
        currentColor = initialSkyColor;
        skySprite = sky.GetComponent<SpriteRenderer>();
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
            GameObject randomBackgroundObject;

            if (GameManager.Instance.CurrentWaveIndex == 0)
            {
                randomBackgroundObject = clouds[Random.Range(0, clouds.Length)];
            }
            else if (GameManager.Instance.CurrentWaveIndex == 1) 
            {
                randomBackgroundObject = stars[Random.Range(0, clouds.Length)];
            }
            else 
            {
                randomBackgroundObject = stars[Random.Range(0, clouds.Length)];
            }

            Instantiate(randomBackgroundObject, spawnPoint, Quaternion.identity);

            yield return new WaitForSeconds(cloudInterval);

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentWaveIndex == 1) 
        {
            movementSpeed = 5f;
        }
        foreach (GameObject backgroundObject in GameObject.FindGameObjectsWithTag("BackgroundObject"))
        {
            backgroundObject.transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);

            if (backgroundObject.transform.position.y < mainCamera.ViewportToWorldPoint(Vector3.zero).y - despawnDistance)
            {
                Destroy(backgroundObject);
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
