using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    [System.Serializable]
    public class StoreItem
    {
        public string itemName;
        public GameObject itemObject; // Referencia al objeto en el Player
        public int price;
        public Button toggleButton;
        public TextMeshProUGUI priceText;
    }

    public StoreItem[] items; // Lista de ítems en la tienda

    private void Start()
    {
        LoadPurchases();
    }

    public void ToggleItem(int index)
    {
        string itemKey = items[index].itemName;

        // Si no está comprado, intentamos comprarlo
        if (PlayerPrefs.GetInt(itemKey, 0) == 0)
        {
            int totalCarrots = UIManager.GetInstance().GetTotalCarrots();

            if (totalCarrots >= items[index].price)
            {
                // Restar zanahorias y actualizar el UIManager
                UIManager.GetInstance().SpendCarrots(items[index].price);

                // Marcar el objeto como comprado
                PlayerPrefs.SetInt(itemKey, 1);
                PlayerPrefs.SetInt(itemKey + "_active", 1); // Guardar que está activo
                PlayerPrefs.Save();

                // Activar el objeto
                items[index].itemObject.SetActive(true);

                // Cambiar el texto del botón
                items[index].priceText.text = "On/Off";
            }
            else
            {
                Debug.Log("No tienes suficientes zanahorias.");
            }
        }
        else
        {
            // Si ya fue comprado, simplemente alternamos su estado
            bool isActive = !items[index].itemObject.activeSelf;
            items[index].itemObject.SetActive(isActive);

            // Guardar el estado en PlayerPrefs
            PlayerPrefs.SetInt(itemKey + "_active", isActive ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private void LoadPurchases()
    {
        for (int i = 0; i < items.Length; i++)
        {
            string itemKey = items[i].itemName;

            if (PlayerPrefs.GetInt(itemKey, 0) == 1) // Si fue comprado
            {
                bool isActive = PlayerPrefs.GetInt(itemKey + "_active", 1) == 1;
                items[i].itemObject.SetActive(isActive);
                items[i].priceText.text = "On/Off"; // Cambiar el texto del botón
            }
            else
            {
                items[i].priceText.text = items[i].price + " "; // Mostrar precio si no está comprado
            }
        }
    }
}
