using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;
using System;

public class MarketManager : MonoBehaviour, IStoreListener
{
    [Serializable]
    public struct IAProduct
    {
        public string ID;
        public Button button;
        public ProductType type;
    }

    public static MarketManager instance;
    IStoreController controller;

    [SerializeField] private List<IAProduct> Products;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        IAPStart();
    }

    void IAPStart() 
    {
        var module = StandardPurchasingModule.Instance();
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        //Specifying Prices
        foreach (IAProduct p in Products)
        {
            string id = p.ID;

            p.button.onClick.AddListener(() => IAPButton(id));
            
            

            builder.AddProduct(id, p.type);
        }

        UnityPurchasing.Initialize(this, builder);
    }
    public void IAPButton(string id)
    {
        Product proc = controller.products.WithID(id);

        if (proc != null && proc.availableToPurchase)
        {
            Debug.Log("Buying");
            controller.InitiatePurchase(proc);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("Purchased: " + e.purchasedProduct.definition.id);
        switch (e.purchasedProduct.definition.id)
        {
            case "":
                {
                    break;
                }
        }
        return PurchaseProcessingResult.Pending;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log(error.ToString());
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Error while buying" + product.ToString());
    }
}
