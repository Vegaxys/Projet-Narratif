using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using PlayFab;
using PlayFab.ClientModels;

public class UIShopManager : MonoBehaviour{

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemContainer;

    private void Start() {
        GetSkinData();
    }
    void GetSkinData() {

        GetCatalogItemsRequest request = new GetCatalogItemsRequest();
        request.CatalogVersion = "Skins";

        PlayFabClientAPI.GetCatalogItems(request, LogSuccess => {
            StartCoroutine(PopulateShop(LogSuccess));
        }, LogFailure => {
        });
    }
    private IEnumerator PopulateShop(GetCatalogItemsResult result) {

        yield return new WaitUntil(() => result.Catalog.Count != 0);

        foreach (var item in result.Catalog) {
            GameObject skin = Instantiate(itemPrefab, itemContainer);
            uint cost = 0;
            item.VirtualCurrencyPrices.TryGetValue("VP", out cost);
            skin.GetComponentInChildren<TextMeshProUGUI>().text = cost.ToString();

            string image = item.ItemImageUrl;
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(image)) {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError) {
                    Debug.Log(uwr.error);
                } else {
                    // Get downloaded asset bundle
                    Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                    skin.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }
        }
    }
}
