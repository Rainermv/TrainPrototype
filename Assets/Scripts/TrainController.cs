using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public Entity Engine;
    
    private Bounds _engineBounds;
    private List<WagonComponent> _wagons = new List<WagonComponent>();

    public Transform marker;

    // Start is called before the first frame update
    void Start()
    {
        _engineBounds = Engine.GetComponentInChildren<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WagonComponent AddWagonOnRear(WagonComponent wagonPrefab)
    {
        
        // GameObject
        var wagon = Instantiate(wagonPrefab, transform, true);
        wagon.transform.position =  CalculateRearPosition(wagon.SpriteRenderer.bounds);
        //wagon.Bounds = wagon.GetComponent<Collider2D>().bounds;

        _wagons.Add(wagon);

        return wagon;
    }

    private Vector3 CalculateRearPosition(Bounds wagonBounds)
    {
        
        var position = new Vector2(_engineBounds.min.x, _engineBounds.min.y); //   [][][].[engine]
        float offset = 0.1f;

        //[][].[][engine]
        foreach (var wagon in _wagons)
        {
            position.x -= wagon.Collider2D.bounds.size.x + offset;
        }

        position.x -= wagonBounds.extents.x;
        position.y += wagonBounds.extents.y - wagonBounds.center.y;

        return position;

    }
}
