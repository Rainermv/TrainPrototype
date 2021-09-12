using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiButtonsController : MonoBehaviour
{
    public Button ButtonTemplate;
    private Action<Entity> _onClickButtonAction;

    private List<EmojiButton> _buttons;

    internal void Initialize(Action<Entity> onClickButtonAction) {

        _buttons = new List<EmojiButton>();
        _onClickButtonAction = onClickButtonAction;

    }

    public void AddButton(Entity entityPrefab) {

        var sprite = entityPrefab.GetComponent<SpriteRenderer>();

        var button = Instantiate(ButtonTemplate);
        button.GetComponent<RectTransform>().SetParent(this.transform);
        button.transform.localScale = new Vector3(1, 1, 1);
        
        _buttons.Add(button.GetComponent<EmojiButton>());

        button.GetComponent<EmojiButton>().Initialize(sprite.sprite, (prefab) => {

            ClearButtons();
            _onClickButtonAction(prefab);

        }, entityPrefab);

    }

    

    private void ClearButtons() {
        foreach (var button in _buttons) {
            button.SetSelected(false);
        }
    }
}
