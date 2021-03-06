﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YenePaySdk
{
    public class CheckoutHelper
    {
        private const string checkoutBaseUrlProd = "http://localhost/checkout/Home/Process";
        private const string checkoutBaseUrlSandbox = "http://localhost/checkout/Home/Process";

        public static string GetCheckoutUrl(CheckoutOptions options, CheckoutItem item)
        {
            try
            {
                var dict = options.GetAsKeyValue();
                item.GetAsKeyValue(dict);
                var checkoutUrl = string.Format(string.Concat(checkoutBaseUrlProd, "?{0}"), dict.ConvertToUriParamString());
                if (options.UseSandbox)
                    checkoutUrl = string.Format(string.Concat(checkoutBaseUrlSandbox, "?{0}"), dict.ConvertToUriParamString());
                return checkoutUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCheckoutUrl(CheckoutOptions options, List<CheckoutItem> items)
        {
            try
            {
                var dict = options.GetAsKeyValue();
                for (int i = 0; i < items.Count; i++)
                {
                    var itemDict = items[i].GetAsKeyValue(null);
                    foreach (var keyValue in itemDict)
                    {
                        dict.Add(string.Format("Items[{0}].{1}", i, keyValue.Key), keyValue.Value);
                    }
                }
                var checkoutUrl = string.Format(string.Concat(checkoutBaseUrlProd, "?{0}"), dict.ConvertToUriParamString());
                if (options.UseSandbox)
                    checkoutUrl = string.Format(string.Concat(checkoutBaseUrlSandbox, "?{0}"), dict.ConvertToUriParamString());
                return checkoutUrl;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public static class HelperExtentions
    {
        public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Dictionary<TKey, TValue> clone = new Dictionary<TKey, TValue>();
            foreach (var keyValue in dict)
            {
                clone.Add(keyValue.Key, keyValue.Value);
            }
            return clone;
        }

        public static void CleanQueryParams(this IDictionary<string, string> dict)
        {
            if (dict.Count > 0)
            {
                var invalidKeys = dict.Keys.Where(k => k.Contains("?")).ToArray();
                foreach (var invalidKey in invalidKeys)
                {
                    string validKey = invalidKey.Replace("?", string.Empty);
                    string value = dict[invalidKey];
                    dict.Remove(invalidKey);
                    dict.Add(validKey, value);
                }
            }
        }

        public static string ConvertToUriParamString(this IDictionary<string, string> dict, bool cloneFirst = false, bool cleanFirst = false)
        {
            IDictionary<string, string> resultDict = cloneFirst ? dict.Clone() : dict;
            if (cleanFirst)
            {
                resultDict.CleanQueryParams();
            }
            List<string> paramList = new List<string>();
            foreach (var keyValue in resultDict)
            {
                paramList.Add(string.Format("{0}={1}", keyValue.Key, keyValue.Value));
            }
            if (paramList.Count > 0)
            {
                return string.Join("&", paramList.ToArray());
            }
            return string.Empty;
        }
    }
}

