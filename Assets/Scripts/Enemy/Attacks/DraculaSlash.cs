using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraculaSlash : MonoBehaviour
{
    public Color WarningColor;
    public Color SlashColor;

    public float WarningTime;
    public float DecayTime;

    public float SlashDamage;

    public SpriteRenderer Sprite { get; private set; }
    public BoxCollider2D Collider { get; private set; }

    public int PlayerLayerMask { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Sprite = GetComponent<SpriteRenderer>();
        Collider = GetComponent<BoxCollider2D>();
        PlayerLayerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(PerformSlash());
    }

    private IEnumerator PerformSlash()
    {
        var startColor = new Color(WarningColor.r, WarningColor.g, WarningColor.b, 0f);
        var endColor = new Color(WarningColor.r, WarningColor.g, WarningColor.b, 0.5f);

        var startTime = Time.time;
        while (Time.time < startTime + WarningTime)
        {
            var t = 1 - (startTime + WarningTime - Time.time) / WarningTime;
            Sprite.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        if (Collider.IsTouchingLayers(PlayerLayerMask))
        {
            PlayerHealth.Instance.TakeDamage(SlashDamage);
        }

        startColor = new Color(SlashColor.r, SlashColor.g, SlashColor.b, 1f);
        endColor = new Color(SlashColor.r, SlashColor.g, SlashColor.b, 0f);

        startTime = Time.time;
        while (Time.time < startTime + DecayTime)
        {
            var t = 1 - (startTime + DecayTime - Time.time) / DecayTime;
            Sprite.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
