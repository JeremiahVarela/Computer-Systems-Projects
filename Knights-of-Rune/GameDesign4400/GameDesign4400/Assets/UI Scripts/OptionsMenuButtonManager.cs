using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MainMenuManager.OptionsButtons _buttonType;
    public void ButtonClicked()
    {
        MainMenuManager._.OptionsButtonClicked(_buttonType);
    }
}
