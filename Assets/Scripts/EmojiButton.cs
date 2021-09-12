using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiButton : MonoBehaviour
{

    public Color SelectedColor;
    public Color UnSelectedColor;

    public Image ButtonImage;

    public Entity EntityPrefab;

    internal void Initialize(Sprite sprite, Action<Entity> onClickButtonAction, Entity entityPrefab) {

        EntityPrefab = entityPrefab;
        ButtonImage.sprite = sprite;

        GetComponent<Button>().onClick.AddListener(() => {

            onClickButtonAction(entityPrefab);
            SetSelected(true);
        });

        SetSelected(false);
        
    }

    internal void SetSelected(bool selected) {
        
        if (selected) {
            ButtonImage.color = SelectedColor;
            return;
        }

        ButtonImage.color = UnSelectedColor;
    }
}
