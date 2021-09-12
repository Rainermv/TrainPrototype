using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{

    public float MAX_SPAWN_DISTANCE = 3;

    public EmojiButtonsController EmojiButtonsController;

    public EnemyPlayeController enemyPlayerController;

    public Castle PlayerCastle;
    public Castle EnemyCastle;

    public Entity[] emojiPrefabs;

    public Collider2D[] Lanes;

    private Entity _selectedPrefab;

    // Start is called before the first frame update
    void Start()
    {

        EmojiButtonsController.Initialize((selectedPrefab) => {
            _selectedPrefab = selectedPrefab;
        });

        foreach (var emoji in emojiPrefabs) {
            EmojiButtonsController.AddButton(emoji);
        }

        enemyPlayerController.Initialize(this, EnemyCastle, 1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) {

            if (_selectedPrefab == null) {
                return;
            }

            var mousePosition = Input.mousePosition;

            var clickedWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //clickedWorldPosition.z = 0;

            var clickedOn = Physics2D.OverlapPoint(clickedWorldPosition);
                        
            foreach (var lane in Lanes) {

                if (clickedOn == lane) {

                    InstantiateOnLane(_selectedPrefab, PlayerCastle, lane, Tags.PLAYER);

                }
            }

        }

    }

    public void InstantiateOnLane(Entity instantiatePrefab, Castle baseCastle, Collider2D lane, string tag) {

        var position = new Vector3(baseCastle.transform.position.x, lane.transform.position.y);

        var emoji = Instantiate(instantiatePrefab, position, Quaternion.identity);

        emoji.Initialize(tag);
    }
}
