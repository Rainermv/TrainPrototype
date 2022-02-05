using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("References")] public Canvas Canvas;
    public Camera Camera;
    public BuyWagonButton WagonButtonPrefab;
    public DragAndDropItem ItemPrefab;
    public DragAndDropCell CellPrefab;

    [Header("Station UI")] public GameObject StationUI;
    public Transform SlotsTransform;
    public Transform BuildMenuTransform;    
    public TextMeshProUGUI BigTitleText;
    public Text TitleText;
    public Text GoldText;
    public Text NextLevelButtonText;
    public Button NextLevelButton;

    [Header("Level UI")] public GameObject LevelUI;

    private int _tier;

    private Action _onNextLevelButtonClicked;
    public Action<WagonComponent> OnBuyWagonButtonClicked;
    private List<BuyWagonButton> _buyWagonButtons = new List<BuyWagonButton>();


    public void Initialize(GameData gameData, List<WagonComponent> wagonLibrary,
        Action onNextLevelButtonClicked)
    {
        AddBuildButtons(wagonLibrary);

        UpdateUI(gameData);
        _onNextLevelButtonClicked = onNextLevelButtonClicked;


    }

    private void AddBuildButtons(List<WagonComponent> wagonLibrary)
    {
        foreach (var wagonComponent in wagonLibrary)
        {
            var buyWagonButton = Instantiate(WagonButtonPrefab, BuildMenuTransform);
            
            buyWagonButton.ImageComponent.sprite = wagonComponent.SpriteRenderer.sprite;
            buyWagonButton.NameComponent.text = wagonComponent.WagonName;
            buyWagonButton.MoneyValueComponent.text = $"{wagonComponent.WagonPrice} Gold";
            buyWagonButton.WagonComponent = wagonComponent;

            _buyWagonButtons.Add(buyWagonButton);

            buyWagonButton.ButtonComponent.onClick.AddListener(() =>
            {
                OnBuyWagonButtonClicked(buyWagonButton.WagonComponent);
            });

        }
    }

    public void UpdateUI(GameData gameData)
    {
        SetUiChildrenStatus(false);
        switch (gameData.GameState)
        {
            case GameState.Level:
                UpdateLevelUI(gameData);
                LevelUI.SetActive(true);
                break;
            case GameState.Station:
                UpdateStationUI(gameData);
                StationUI.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }



    private void SetUiChildrenStatus(bool isActive)
    {
        StationUI.gameObject.SetActive(isActive);
        LevelUI.gameObject.SetActive(isActive);
    }

    private void UpdateLevelUI(GameData gameData)
    {

    }

    private void UpdateStationUI(GameData gameData)
    {
        TitleText.text = $"Welcome to Station {gameData.Level:00}";
        BigTitleText.text = $"Station {gameData.Level:00}";

        //_tier = gameData.Tier;
        NextLevelButtonText.text = $"Press to start level {gameData.Level + 1}";
        GoldText.text = $"Gold: {gameData.Gold}";

        NextLevelButton.onClick.RemoveAllListeners();
        NextLevelButton.onClick.AddListener(() => { _onNextLevelButtonClicked(); });

        foreach (var buyWagonButton in _buyWagonButtons)
        {
            buyWagonButton.SetEnabled(gameData.Gold - buyWagonButton.WagonComponent.WagonPrice >= 0);
        }
    }

    public void AddDraggableSlot(Vector3 worldPosition, WagonComponent wagonComponent, string id)
    {
        
        var slot = Instantiate(CellPrefab, SlotsTransform, true);
        slot.name = $"Slot {id}";
        var slotRect = slot.GetComponent<RectTransform>();
        
        var canvasRect = Canvas.GetComponent<RectTransform>();

        RectWorldToViewportPosition(worldPosition, slotRect, canvasRect);

        var item = Instantiate(ItemPrefab, slot.transform);
        item.name = $"Item {id}";

        var itemImage = item.GetComponent<Image>();
        var wagonSpriteRenderer = wagonComponent.GetComponentInChildren<SpriteRenderer>();
        itemImage.sprite = wagonSpriteRenderer.sprite;

        var wagonColor = wagonSpriteRenderer.color;
        var colorIdle = new Color(1f, 1f, 1f, 0);
        var colorDragging = new Color(1f, 1f, 1f, 0.8f);
        itemImage.color = colorIdle;

        var itemRect = item.GetComponent<RectTransform>();
        itemRect.anchoredPosition = Vector2.zero;
        itemRect.localScale = Vector3.one;

        var itemRef = item.GetComponent<DragItemReference>();
        itemRef.WagonComponentReference = wagonComponent;
        itemRef.ItemImageReference = itemImage;
        itemRef.OnBeginDragAction = (eventData, dragItemReference) =>
        {
            wagonSpriteRenderer.color = colorDragging;
            //DragAndDropItem.icon.GetComponent<Image>().color = colorDragging;

            //dragItemReference.ItemImageReference.color = colorDragging;
        };
        itemRef.OnEndDragAction = (eventData, dragItemReference) =>
        {
            wagonSpriteRenderer.color = wagonColor;
            //dragItemReference.ItemImageReference.color = colorIdle;
        };

        //rect.position = Vector3.one;

        //RectWorldToViewportPosition(worldPosition, item.GetComponent<RectTransform>(), canvasRect);

        //slot.AddItem(item);
        slot.cellType = DragAndDropCell.CellType.Swap;
        //slot.AddItem(item);

    }

    private void RectWorldToViewportPosition(Vector3 worldPosition, RectTransform rect, RectTransform canvasRect)
    {
        //first you need the RectTransform component of your canvas

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        var viewportPosition = Camera.WorldToViewportPoint(worldPosition);
        var worldObjectScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        rect.anchoredPosition = worldObjectScreenPosition;
        rect.localScale = Vector3.one;
    }


    void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        var sourceCell = desc.sourceCell.GetItem();
        var destinationCell = desc.destinationCell.GetItem();

        var sourceReference = sourceCell.GetComponent<DragItemReference>();
        var destinationReference = destinationCell.GetComponent<DragItemReference>();

        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.item.name + " from " + sourceCell.name + " to " + destinationCell.name);

                //var sourcePosition = sourceReference.transform.localPosition;
                sourceReference.DestinationPosition = destinationReference.WagonComponentReference.transform.localPosition;

                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " from " + sourceCell.name + " to " + destinationCell.name);

                    // SWAP!!!
                    sourceReference.WagonComponentReference.transform.localPosition = sourceReference.DestinationPosition;
                    //destinationReference.transform.localPosition = sourcePosition;
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceCell.name + " to " + destinationCell.name);
                }
                break;
            case DragAndDropCell.TriggerType.ItemAdded:                         // New item is added from application
                Debug.Log("Item " + desc.item.name + " added into " + destinationCell.name);
                break;
            case DragAndDropCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                Debug.Log("Item " + desc.item.name + " will be destroyed from " + sourceCell.name);
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }
    }
}
