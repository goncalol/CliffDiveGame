using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class GooglePlayController : MonoBehaviour, IStoreListener
{
    private BannerView bannerView;
    private int noAds;
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private string coins1000 = "1000_coins";
    private string coins500 = "500_coins";
    private string coins2000 = "2000_coins";
    private string noAdsId = "no_ads";

    public GameObject bottomGroup;
    public ShopController shop;
    public TextMeshProUGUI Buy1;
    public TextMeshProUGUI Buy2;
    public TextMeshProUGUI Buy3;

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }

        noAds = PlayerPrefs.GetInt("NoAds", 0);

        if (noAds == 0)
        {
            bannerView = new BannerView("ca-app-pub-8455806674655279/2613052177", AdSize.Banner, AdPosition.Bottom);//"ca-app-pub-3940256099942544/6300978111".
            AdRequest request = new AdRequest.Builder().Build();
            bannerView.LoadAd(request);
        }
        else
            bottomGroup.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 900, 0);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn(false);
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(coins1000, ProductType.Consumable);
        builder.AddProduct(coins2000, ProductType.Consumable);
        builder.AddProduct(coins500, ProductType.Consumable);
        builder.AddProduct(noAdsId, ProductType.NonConsumable);
       
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    void SignIn(bool showLeaderboard)
    {
        Social.localUser.Authenticate(success =>
        {
            if (success && showLeaderboard) Social.ShowLeaderboardUI();
            else if (showLeaderboard && !success)
            {
                //message alert to the user to say need to be authenticated in google play!!!
                Debug.Log(success);
            }
        });
    }

    public void RemoveBanner()
    {
        if (noAds == 0)
        {
            BuyProductID(noAdsId);            
        }
    }

    public void OnCoinPriceClick(int type)
    {
        if (type == 1)
            BuyProductID(coins500);
        else if (type == 2)
            BuyProductID(coins1000);
        else if(type == 3)
            BuyProductID(coins2000);
    }

    public void AddScoreToLeaderBoard(string id, long score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, id, success =>
            {
                Debug.Log(success);
            });
        }
    }

    public void ShowLeaderboardUI()
    {
        if (Social.localUser.authenticated)
        {
            //actualiza o score do player
            Social.ReportScore(PlayerPrefs.GetInt("MaxScore", 0), GPGSIds.leaderboard_cliff_dive_leaderboard, success =>
            {
                if (success) Social.ShowLeaderboardUI();
            });
        }
        else
        {
            SignIn(true);
        }
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }   

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", i.definition.storeSpecificId, p));
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (string.Equals(args.purchasedProduct.definition.id, coins500, StringComparison.Ordinal))
        {
            shop.IncreaseCoins(500);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coins1000, StringComparison.Ordinal))
        {
            shop.IncreaseCoins(1000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coins2000, StringComparison.Ordinal))
        {
            shop.IncreaseCoins(2000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, noAdsId, StringComparison.Ordinal))
        {
            if (bannerView != null) bannerView.Destroy();

            bottomGroup.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 900, 0);
            PlayerPrefs.SetInt("NoAds", 1);
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        foreach (var product in controller.products.all)
        {
            if (string.Equals(product.definition.id, coins500, StringComparison.Ordinal))
            {
                Buy1.SetText(product.metadata.localizedPriceString +" "+ product.metadata.isoCurrencyCode);
            }
            else if (String.Equals(product.definition.id, coins1000, StringComparison.Ordinal))
            {
                Buy2.SetText(product.metadata.localizedPriceString + " " + product.metadata.isoCurrencyCode);
            }
            else if (String.Equals(product.definition.id, coins2000, StringComparison.Ordinal))
            {
                Buy3.SetText(product.metadata.localizedPriceString + " " + product.metadata.isoCurrencyCode);
            } 
        }
    }
}
