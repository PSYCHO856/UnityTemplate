﻿using System;
using UnityEngine;

namespace Watermelon
{
    [Serializable]
    public class IAPItem
    {
        [SerializeField] private string id;
        [SerializeField] private ProductKeyType productKeyType;
        [SerializeField] private ProductType productType;

        public string ID
        {
            get => id;
            set => id = value;
        }

        public ProductType ProductType
        {
            get => productType;
            set => productType = value;
        }

        public ProductKeyType ProductKeyType
        {
            get => productKeyType;
            set => productKeyType = value;
        }

#if MODULE_IAP
        private UnityEngine.Purchasing.Product product;
        public UnityEngine.Purchasing.Product Product { get => product; }
#endif

        public IAPItem()
        {
        }

        public IAPItem(string id, ProductKeyType productKeyType, ProductType productType)
        {
            this.id = id;
            this.productKeyType = productKeyType;
            this.productType = productType;
        }

#if MODULE_IAP
        public void SetProduct(UnityEngine.Purchasing.Product product)
        {
            this.product = product;
        }
#endif
    }
}

// -----------------
// IAP Manager v 0.4
// -----------------