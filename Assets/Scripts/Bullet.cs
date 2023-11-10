using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction;

    public float livingTime = 3f;
    public Color initialColor = Color.white;
    public Color finalColor;

    private SpriteRenderer _renderer;
    private float _startingTime;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Save initial time
        _startingTime = Time.time;

        // Destroy the bullet ofter some time
        Destroy(this.gameObject, livingTime );
    }

    // Update is called once per frame
    void Update()
    {
        //Move Object
        Vector2 movement = direction.normalized * speed * Time.deltaTime;
        //transform.position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);
        transform.Translate(movement);

        //Change bullet´s color over time
        float _timeSinceStarted = Time.time - _startingTime;
        float _percentageCompleted = _timeSinceStarted / livingTime;

        _renderer.color = Color.Lerp(initialColor, finalColor, _percentageCompleted);
    }
}
