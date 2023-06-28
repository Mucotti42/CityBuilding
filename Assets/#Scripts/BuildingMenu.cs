using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] private BuildingType type;
    private BuildingModel model;

    private bool canPurchase;
    
    #region UIReferances

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject black;

    //Income
    [SerializeField] private TextMeshProUGUI genGold;
    [SerializeField] private GameObject genSeperator;
    [SerializeField] private TextMeshProUGUI genGem;
    
    //Cost
    [SerializeField] private TextMeshProUGUI costGold;
    [SerializeField] private GameObject costSeperator;
    [SerializeField] private TextMeshProUGUI costGem;

    #endregion
    #region Events

    private void OnEnable()
    {
        EventManager.instance.OnCurrencyUpdated += UpdateAffordability;
    }

    private void OnDisable()
    {
        EventManager.instance.OnCurrencyUpdated -= UpdateAffordability;
    }

    #endregion
    
    private void Awake()
    {
        LoadResources();
        Initialize();
        TriggerEvent();
    }

    private void LoadResources()
    {
        model = Resources.Load<BuildingModel>("Buildings/"+type.ToString());
        
        if (model == null)
        {
            Debug.LogError("Failed to load the object from Resources folder.");
        }
    }

    private void Initialize()
    {
        nameText.text = model.name;
        
        //Income
        genGold.text = "+" + model.goldGen.ToString();
        genGem.text = "+" + model.gemGen.ToString();
        if (model.goldGen < 1 || model.gemGen < 1)
        {
            genGold.transform.parent.gameObject.SetActive(model.goldGen > 0);
            genGem.transform.parent.gameObject.SetActive(model.gemGen > 0);
            genSeperator.SetActive(false);
        }
        
        //Cost
        costGold.text = model.goldCost.ToString();
        costGem.text = model.gemCost.ToString();
        if (model.goldCost < 1 || model.gemCost < 1)
        {
            costGold.transform.parent.gameObject.SetActive(model.goldCost > 0);
            costGem.transform.parent.gameObject.SetActive(model.gemCost > 0);
            costSeperator.SetActive(false);
        }

    }
    private void TriggerEvent()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry onPointerDown = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.BeginDrag
        };
        
        
        onPointerDown.callback.AddListener(OnPointerDown);
        eventTrigger.triggers.Add(onPointerDown);
    }

    private void OnPointerDown(BaseEventData eventData)
    {
        if (canPurchase)
        {
            EventManager.instance.StartDragging(model);
        }
    }

    private void UpdateAffordability()
    {
        canPurchase = MenuView.instance.gold > model.goldCost && MenuView.instance.gem > model.gemCost;
        black.SetActive(!canPurchase);
    }
}
