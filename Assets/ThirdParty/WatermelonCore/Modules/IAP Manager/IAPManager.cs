﻿using UnityEngine;

#if MODULE_IAP
using UnityEngine.Purchasing;
#endif

namespace Watermelon
{
    [Define("MODULE_IAP")]
    public class IAPManager
    {
        private static IAPManager instance;

        private readonly bool isInititalized = false;

#if MODULE_IAP
        private IAPStoreListener storeListener;
#endif

        public static OnPurchaseCompleteCallback OnPurchaseComplete;
        public static OnPurchaseFaildedCallback OnPurchaseFailded;

        public void Init(GameObject initObject, IAPSettings settings)
        {
            if (isInititalized)
            {
                Debug.Log("[IAP Manager]: Module is already initialized!");
                return;
            }

            instance = this;

#if MODULE_IAP
            storeListener = initObject.AddComponent<IAPStoreListener>();
            storeListener.Init(this, settings.storeItems);
#else
            Debug.Log("[IAP Manager]: Define is disabled!");
#endif
        }

        public static void RestorePurchases()
        {
#if MODULE_IAP
            instance.storeListener.Restore();
#endif
        }

        public static void BuyProduct(ProductKeyType productKeyType)
        {
#if MODULE_IAP
            instance.storeListener.OnPurchaseClicked(productKeyType);
#endif
        }

#if MODULE_IAP
        public class IAPStoreListener : MonoBehaviour, IStoreListener
        {
            private Dictionary<ProductKeyType, IAPItem> productsTypeToProductLink =
 new Dictionary<ProductKeyType, IAPItem>();
            private Dictionary<string, IAPItem> productsKeyToProductLink = new Dictionary<string, IAPItem>();

            private IStoreController controller;
            private IExtensionProvider extensions;

            private IAPManager manager;

            public void Init(IAPManager manager, IAPItem[] items)
            {
                this.manager = manager;

                // Init products
                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                for (int i = 0; i < items.Length; i++)
                {
                    builder.AddProduct(items[i].ID, (UnityEngine.Purchasing.ProductType)items[i].ProductType);

                    productsTypeToProductLink.Add(items[i].ProductKeyType, items[i]);
                    productsKeyToProductLink.Add(items[i].ID, items[i]);
                }

                UnityPurchasing.Initialize(this, builder);
            }

            /// <summary>
            /// Called when Unity IAP is ready to make purchases.
            /// </summary>
            public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
            {
                manager.isInititalized = true;

                this.controller = controller;
                this.extensions = extensions;

                foreach (var item in controller.products.all)
                {
                    if (item.availableToPurchase)
                    {
                        if(!string.IsNullOrEmpty(item.definition.id))
                        {
                            if (productsKeyToProductLink.ContainsKey(item.definition.id))
                            {
                                productsKeyToProductLink[item.definition.id].SetProduct(item);
                            }
                        }
                    }
                }

                Debug.Log("[IAPManager]: Module is initialized!");
            }

            /// <summary>
            /// Called when Unity IAP encounters an unrecoverable initialization error.
            ///
            /// Note that this will not be called if Internet is unavailable; Unity IAP
            /// will attempt initialization until it becomes available.
            /// </summary>
            public void OnInitializeFailed(InitializationFailureReason error)
            {
                Debug.Log("[IAPManager]: Module initialization is failed!");
            }

            /// <summary>
            /// Called when a purchase completes.
            ///
            /// May be called at any time after OnInitialized().
            /// </summary>
            public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
            {
                Debug.Log("[IAPManager]: Purchasing - " + e.purchasedProduct.definition.id + " is completed!");

                if (productsKeyToProductLink.ContainsKey(e.purchasedProduct.definition.id))
                {
                    IAPItem tempStoreItem = productsKeyToProductLink[e.purchasedProduct.definition.id];
                    if (tempStoreItem != null)
                    {
                        if (OnPurchaseComplete != null)
                            OnPurchaseComplete.Invoke(tempStoreItem.ProductKeyType);
                    }
                }
                else
                {
                    Debug.Log("[IAPManager]: Product - " + e.purchasedProduct.definition.id + " can't be found!");
                }

                return PurchaseProcessingResult.Complete;
            }

            /// <summary>
            /// Called when a purchase fails.
            /// </summary>
            public void OnPurchaseFailed(Product product, UnityEngine.Purchasing.PurchaseFailureReason failureReason)
            {
                Debug.Log("[IAPManager]: Purchasing - " + product.definition.id + " is failed!");
                Debug.Log("[IAPManager]: Fail reason - " + failureReason.ToString());

                if (productsKeyToProductLink.ContainsKey(product.definition.id))
                {
                    IAPItem tempShopItem = productsKeyToProductLink[product.definition.id];
                    if (tempShopItem != null)
                    {
                        if (OnPurchaseFailded != null)
                            OnPurchaseFailded.Invoke(tempShopItem.ProductKeyType, (Watermelon.PurchaseFailureReason)failureReason);
                    }
                }
                else
                {
                    Debug.Log("[IAPManager]: Product - " + product.definition.id + " can't be found!");
                }
            }

            public void OnPurchaseClicked(ProductKeyType productKeyType)
            {
                if (!manager.isInititalized)
                    return;

                controller.InitiatePurchase(productsTypeToProductLink[productKeyType].ID);
            }

            public void Restore()
            {
                if (!manager.isInititalized)
                    return;

                extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
                {
                    if (result)
                    {
                        // This does not mean anything was restored,
                        // merely that the restoration process succeeded.
                    }
                    else
                    {
                        // Restoration failed.
                    }
                });
            }
        }
#endif

        public delegate void OnPurchaseCompleteCallback(ProductKeyType productKeyType);

        public delegate void OnPurchaseFaildedCallback(ProductKeyType productKeyType,
            PurchaseFailureReason failureReason);
    }

    public enum PurchaseFailureReason
    {
        PurchasingUnavailable = 0,
        ExistingPurchasePending = 1,
        ProductUnavailable = 2,
        SignatureInvalid = 3,
        UserCancelled = 4,
        PaymentDeclined = 5,
        DuplicateTransaction = 6,
        Unknown = 7
    }

    public enum ProductType
    {
        Consumable = 0,
        NonConsumable = 1,
        Subscription = 2
    }
}

// -----------------
// IAP Manager v 0.4
// -----------------

// Changelog
// v 0.4
// • IAPStoreListener inheriting from MonoBehaviour
// v 0.3
// • Editor style update
// v 0.2
// • IAPManager structure changed
// • Enums from UnityEditor.Purchasing has duplicated to prevent serialization problems
// v 0.1
// • Added basic version