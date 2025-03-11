using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    [System.Serializable]
    public class StoreItem
    {
        public string itemName;
        public GameObject itemObject;
        public int price;
        public Button toggleButton;
        public TextMeshProUGUI priceText;
    }
    //Hola
    public StoreItem[] items;

    private void Start()
    {
        LoadPurchases();
    }

    public void ToggleItem(int index)
    {
        string itemKey = items[index].itemName;

        if (PlayerPrefs.GetInt(itemKey, 0) == 0)
        {
            int totalCarrots = UIManager.GetInstance().GetTotalCarrots();

            if (totalCarrots >= items[index].price)
            {
                UIManager.GetInstance().SpendCarrots(items[index].price);

                PlayerPrefs.SetInt(itemKey, 1);
                PlayerPrefs.SetInt(itemKey + "_active", 1);
                PlayerPrefs.Save();

                items[index].itemObject.SetActive(true);

                items[index].priceText.text = "On/Off";
            }
            else
            {
                Debug.Log("No tienes suficientes zanahorias.");
            }
        }
        else
        {

            bool isActive = !items[index].itemObject.activeSelf;
            items[index].itemObject.SetActive(isActive);

            PlayerPrefs.SetInt(itemKey + "_active", isActive ? 1 : 0);
            PlayerPrefs.Save();

        }
    }

    private void LoadPurchases()
    {
        for (int i = 0; i < items.Length; i++)
        {
            string itemKey = items[i].itemName;

            if (PlayerPrefs.GetInt(itemKey, 0) == 1)
            {
                bool isActive = PlayerPrefs.GetInt(itemKey + "_active", 1) == 1;
                items[i].itemObject.SetActive(isActive);
                items[i].priceText.text = "bought";
            }
            else
            {
                items[i].priceText.text = " $ " + items[i].price;
            }
        }
    }
}
