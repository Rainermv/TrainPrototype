using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public Entity Engine;
    
    private SpriteRenderer _egineSpriteRenderer;
    private List<WagonComponent> _wagons = new List<WagonComponent>();

    public Transform marker;
    public int TrainSize => _wagons.Count;

    // Start is called before the first frame update
    void Awake()
    {
        _egineSpriteRenderer = Engine.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WagonComponent AddWagonOnRear(WagonComponent wagonPrefab)
    {
        
        // GameObject
        var wagon = Instantiate(wagonPrefab, transform);
        wagon.transform.position =  CalculateRearPosition(wagon.SpriteRenderer.bounds, 
        _egineSpriteRenderer.bounds);
        //wagon.Bounds = wagon.GetComponent<Collider2D>().bounds;

        _wagons.Add(wagon);

        return wagon;
    }

    private Vector3 CalculateRearPosition(Bounds wagonBounds, Bounds engineBounds)
    {
        
        var position = new Vector2(engineBounds.min.x, engineBounds.min.y); //   [][][].[engine]
        float offset = 0.1f;

        //[][].[][engine]
        foreach (var wagon in _wagons)
        {
            position.x -= wagon.Collider2D.bounds.size.x + offset;
        }

        position.x -= wagonBounds.extents.x;
        position.y += wagonBounds.extents.y - wagonBounds.center.y;

        Instantiate(marker).position = position;

        return position;

    }
}
