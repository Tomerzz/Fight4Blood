using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCharacterSelection : MonoBehaviour
{
    public List<GameObject> characters;
    public List<Button> buttons;

    Button lastButton;
    bool isChanged = true;

    void Start()
    {
        foreach(GameObject gm in characters)
        {
            gm.SetActive(false);
        }

        isChanged = true;
    }

    void Update()
    {
        if (isChanged)
        {
            ChangeSelection();
        }

        if (lastButton.gameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject != lastButton.gameObject)
            {
                isChanged = true;
            }
        }
    }

    public void ChangeSelection()
    {
        foreach (GameObject gm in characters)
        {
            gm.SetActive(false);
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == buttons[i].gameObject)
            {
                characters[i].SetActive(true);
                lastButton = buttons[i];
            }
        }

        isChanged = false;
    }
}