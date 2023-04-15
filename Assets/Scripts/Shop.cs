using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private GameObject _player;
    private Player _playerScript;
    public GameObject shopPanel;
    private int _itemPrice;

    public int currentItem;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _playerScript = _player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) < 2)
        {
            UIManager.Instance.OpenShop(_playerScript.diamondCount.ToString());
            shopPanel.SetActive(true);
        }
        else
        {
            shopPanel.SetActive(false);
        }
    }
    
    public void SelectItem(int index)
    {
        // 0: Flame
        // 1: Boot
        // 2: Key
        switch (index)
        {
            case 0: UIManager.Instance.UpdateSelection(0);
                currentItem = index;
                _itemPrice = 400;
                break;
            case 1: UIManager.Instance.UpdateSelection(1);
                currentItem = index;
                _itemPrice = 200;
                break;
            case 2: UIManager.Instance.UpdateSelection(2);
                currentItem = index;
                _itemPrice = 100;
                break;
        }
    }

    public void BuyItem()
    {
        if (_playerScript.diamondCount >= _itemPrice)
        {
            if (currentItem == 2)
            {
                GameManager.Instance.hasKey = true;
            }
            //reward item
            _playerScript.diamondCount -= _itemPrice;
        }
        else
        {
            Debug.Log("You do not have enough diamonds.");
        }
    }
}
