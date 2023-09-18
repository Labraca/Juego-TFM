using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;
using System;

public class MenuManager : MonoBehaviour
{
    //private IEnumerator coroutine;
    private Vector2 lastItemPosition = Vector2.zero;
    private bool OnHotspotEventPassed = false;
    void OnEnable() {
        EventManager.OnMenuElementClick += ElementClick;
        EventManager.OnMenuTurnOn += inventoryInteractionShift;
    }


    void OnDisable() { 
        EventManager.OnMenuTurnOn -= inventoryInteractionShift;
        EventManager.OnMenuElementClick -= ElementClick;
    }

    private void inventoryInteractionShift(Menu _menu, bool isInstant)
    {
        if (_menu.title.Equals("Interaction") && !AC.PlayerMenus.GetMenuWithName("Inventory").IsOn())
        {
            _menu.uiPositionType = UIPositionType.OnHotspot;
            return;
        }
        if (!_menu.title.Equals("Interaction"))
            return;
        Debug.Log(_menu.title);
        _menu.uiPositionType = UIPositionType.Manual;

        //if (!OnHotspotEventPassed)
          //  return;
        RectTransform menuRect = _menu.rectTransform;
        
        Vector2 newInteractionPos = new Vector2(ACScreen.width*0.5f,ACScreen.height-(lastItemPosition.y+menuRect.rect.height));
        _menu.SetCentre(newInteractionPos, true);
        //_menu.Centre();
      
        _menu.Recalculate();
        //OnHotspotEventPassed = false;
        //Rect menuRect = _menu.GetRect();
        //coroutine = TransportMenu((newValue => _menu = newValue),_menu, new Vector2(menuRect.x, menuRect.y - (menuRect.y / 2)));
        //StartCoroutine(coroutine);
    }

    private void ElementClick(Menu _menu, MenuElement _element, int _slot, int buttonPressed)
    {
        if (!(_element is MenuInventoryBox))
            return;

        lastItemPosition = _menu.GetSlotCentre(_element, _slot);
        OnHotspotEventPassed = true;
    }

    IEnumerator TransportMenu(Action<Menu> menuToBeChagned,AC.Menu _menu,  Vector2 vector2)
    {
        Debug.Log("Funcionando");
        while (_menu.IsOn())
        {
            AC.Menu newMenu = _menu;
            newMenu.rectTransform.position = vector2;
            menuToBeChagned(newMenu);

            yield return null;
        }
    }
}
