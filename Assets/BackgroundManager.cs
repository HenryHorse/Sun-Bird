using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] clouds;
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
        StartCoroutine(CreateClouds());
    }

    IEnumerator CreateClouds()
    {
        while (true)
        {
            float randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector2(0, 1)).x, 
                                        mainCamera.ViewportToWorldPoint(new Vector2(1, 1)).x);
            Vector3 spawnPoint = new Vector3(randomX, mainCamera.transform.position.y + 6, 1);

            GameObject randomCloud = clouds[Random.Range(0, clouds.Length)];
            Instantiate(randomCloud, spawnPoint, Quaternion.identity);

            yield return new WaitForSeconds(cloudInterval);

        }
    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject cloud in GameObject.FindGameObjectsWithTag("Cloud"))
        {
            cloud.transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);

            if (cloud.transform.position.y < mainCamera.ViewportToWorldPoint(Vector3.zero).y - despawnDistance)
            {
                Destroy(cloud);
            }
        }

        if (skyChangeTimer < skyChangeDuration)
        {
            currentColor = Color.Lerp(currentColor, endSkyColor, skyChangeTimer / skyChangeDuration);
            skySprite.color = currentColor;

            skyChangeTimer += Time.deltaTime;
        }   
    }
}
