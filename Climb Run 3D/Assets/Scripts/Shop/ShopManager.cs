using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    public List<Item> items;
    public List<Item> equipItem;
    public Renderer playerRenderer;

    private float _emissionColorIntensityForCube = .002f;
    private bool _firstGame;
    private bool _allItemPurchased = false;

    public bool AllItemPurchased { get => _allItemPurchased; set => _allItemPurchased = value; }

    private void Start()
    {
        _firstGame = PlayerPrefs.GetInt("firstgame") == 0;
        if (_firstGame)
        {
            PlayerPrefs.SetInt("firstgame", 1);
            PlayerPrefs.SetInt("item" + items[0].name, 2);
        }
        CheckItems();

    }

    public void CheckItems()
    {
        foreach (Item item in items)
        {
            item.CheckItem();
            if (item.itemId == 2)
            {
                equipItem.Remove(equipItem[0]);
                equipItem.Add(item);
                playerRenderer.material.color = item.color;
                playerRenderer.material.SetColor("_EmissionColor", item.color * _emissionColorIntensityForCube);

            }
        }
    }

    public void EquipButton(int index)
    {
        PlayerPrefs.SetInt("item" + equipItem[0].name, 1);
        equipItem[0].GetComponent<Outline>().enabled = false;
        equipItem[0].CheckItem();
        equipItem.Remove(equipItem[0]);
        PlayerPrefs.SetInt("item" + items[index].name, 2);
        items[index].GetComponent<Outline>().enabled = true;
        playerRenderer.material.color = items[index].color;
        playerRenderer.material.SetColor("_EmissionColor", items[index].color * _emissionColorIntensityForCube);
        items[index].CheckItem();
        equipItem.Add(items[index]);
    }

    public bool UnlockRandomItem()
    {
        int unPurchased = 0;
        foreach (var item in items)
        {
            if (item.itemId == 0)
            {
                unPurchased++;
            }
        }
        if (unPurchased != 0)
        {
            AllItemPurchased = false;
        }
        else
        {
            AllItemPurchased = true;
        }
        return AllItemPurchased;
    }

    public void UnlockItem()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.uiClickClip);
        PlayerPrefs.SetInt("Total", (PlayerPrefs.GetInt("Total") - 1000));
        
    again:
        int random = Random.Range(0, 9);
        if (items[random].itemId == 0)
        {
            PlayerPrefs.SetInt("item" + items[random].name, 1);
            CheckItems();
            UIManager.Instance.ShopUIUpdate();
        }
        else
        {
            goto again;

        }
    }
}
