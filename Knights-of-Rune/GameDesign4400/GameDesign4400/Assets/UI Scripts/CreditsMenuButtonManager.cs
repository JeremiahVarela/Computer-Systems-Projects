using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MainMenuManager.CreditsButtons _buttonType;
    public void ButtonClicked()
    {
        MainMenuManager._.CreditsButtonClicked(_buttonType);
    }
}
