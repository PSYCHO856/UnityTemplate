#if ESSENTIAL_KIT
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

namespace MobileKit
{
    public static class PurchaseManager
    {
        private const string TAG = nameof(PurchaseManager);
        public static bool IsPurchaseAvailable => isStoreInit && BillingServices.CanMakePayments();

        private static bool isStoreInit;
        
        public static void Init()
        {
            if (BillingServices.IsAvailable())
            {
                MLog.Log(TAG, nameof(Init) + "BillingAvailable? " + BillingServices.IsAvailable());
                // register for events
                BillingServices.OnInitializeStoreComplete   += OnInitializeStoreComplete;
                BillingServices.OnTransactionStateChange    += OnTransactionStateChange;
                BillingServices.OnRestorePurchasesComplete  += OnRestorePurchasesComplete;
                BillingServices.InitializeStore();
            }
        }

        private static void Destroy()
        {
              if (BillingServices.IsAvailable())
              {
                   BillingServices.OnInitializeStoreComplete   -= OnInitializeStoreComplete;
                   BillingServices.OnTransactionStateChange    -= OnTransactionStateChange;
                   BillingServices.OnRestorePurchasesComplete  -= OnRestorePurchasesComplete;
              }
        }

        private static IBillingProduct[] products;
        private static void OnInitializeStoreComplete(BillingServicesInitializeStoreResult result, Error error)
        {
            MLog.Log(TAG, nameof(OnInitializeStoreComplete) + " " + BillingServices.CanMakePayments());
            
            if (error == null)
            {
                products = result.Products;
                isStoreInit = true;
                BillingServices.RestorePurchases();
                MLog.Log(TAG, "store initialized successfully.");
                MLog.Log(TAG, "Total products fetched: " + products.Length);
                MLog.Log(TAG, "Below are the available products:");
                for (int iter = 0; iter < products.Length; iter++)
                {
                    var product = products[iter];
                    MLog.Log(TAG, $"[{iter}]: {product}");
                }
            }
            else
            {
                MLog.Log(TAG, "Store initialization failed with error. Error: " + error);
            }
            
            var  invalidIds = result.InvalidProductIds;
            MLog.Log(TAG, "Total invalid products: " + invalidIds.Length);
            if (invalidIds.Length > 0)
            {
                MLog.Log(TAG, "Here are the invalid product ids:");
                for (int iter = 0; iter < invalidIds.Length; iter++)
                {
                    MLog.Log(TAG, $"[{iter}]: {invalidIds[iter]}");
                }
            }
        }

        
        private static void OnTransactionStateChange(BillingServicesTransactionStateChangeResult result)
        {
            MLog.Log(TAG, nameof(OnTransactionStateChange));
            var transactions = result.Transactions;
            foreach (var transaction in transactions)
            {
                switch (transaction.TransactionState)
                {
                    case BillingTransactionState.Purchased:
                        MLog.Log(TAG, $"Buy product with id:{transaction.Payment.ProductId} finished successfully.");
                        purchaseCallback?.Invoke(transaction.Payment.ProductId, true);
                        break;

                    case BillingTransactionState.Failed:
                        MLog.Log(TAG, $"Buy product with id:{transaction.Payment.ProductId} failed with error. Error: {transaction.Error}");
                        purchaseCallback?.Invoke(transaction.Payment.ProductId, false);
                        break;
                }

                if (transaction.TransactionState != BillingTransactionState.Purchasing)
                {
                    UIManager.HideNetworkWaiting();
                }
            }
            purchaseCallback = null;
        }

        private static void OnRestorePurchasesComplete(BillingServicesRestorePurchasesResult result, Error error)
        {
            MLog.Log(TAG, nameof(OnRestorePurchasesComplete));
            if (error == null)
            {
                var transactions = result.Transactions;
                MLog.Log(TAG, "Request to restore purchases finished successfully. Total restored products: " + transactions.Length);
                foreach (var transaction in transactions)
                {
                    MLog.Log(TAG, $"{transaction.Payment.ProductId}");
                    if (transaction.Payment.ProductId == AdsManager.RemoveAdsProductId)
                    {
                        AdsManager.IsAdFree = true;
                    }
                }
            }
            else
            {
                MLog.Log(TAG, "Request to restore purchases failed with error. Error: " +  error);
            }
        }


        private static Action<string, bool> purchaseCallback;
        
        public static void BuyPurchase(string productId, Action<string, bool> callback)
        {
            MLog.Log(TAG, nameof(BuyPurchase) + " " + productId);
            if (!IsPurchaseAvailable)
            {
                callback?.Invoke(productId, false);
                MLog.Log(TAG, nameof(IsPurchaseAvailable) + "false");
            }
            var product = GetProduct(productId);
            if (product != null)
            {
                if (BillingServices.IsProductPurchased(product))
                {
                    callback?.Invoke(productId, true);
                    MLog.Log(TAG, "IsProductPurchased");
                }
                else
                {
                    purchaseCallback = callback;
                    UIManager.ShowNetworkWaiting();
                    BillingServices.BuyProduct(product);
                    AnalyticsManager.OnCustomEvent("iap_click_" + productId);
                }
            }
            else
            {
                callback?.Invoke(productId, false);
                MLog.Log(TAG, " error. product is null");
            }
        }

        public static void RestorePurchases()
        {
            if (!IsPurchaseAvailable) return;
            BillingServices.RestorePurchases();    
        }
        
        public static IBillingProduct GetProduct(string productId)
        {
            if (!IsPurchaseAvailable) return null;
            foreach (var product in products)
            {
                if (product.Id == productId)
                {
                    return product;
                }
            }
            return null;
        }
    }
}
#endif