using System;
using System.Collections.Generic;

namespace MobileKit
{
    public interface IAppHandler
    {
        void Init();
        
        string GetAppName();
        string GetChannelName();
        
        void RequestStoreReview();

        void ShowUserProtocol();
        void ShowPrivacyPolicy();

        void PurchaseInApp( string skuId, Action<string> succCallback, Action<string> failCallback );
        string GetPriceById( string skuId );
        void RestoredPurchase( Action<string[]> succCallback, Action<string> failCallback );
    }
}
