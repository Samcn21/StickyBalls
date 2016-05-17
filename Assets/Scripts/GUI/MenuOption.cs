using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Text optionText;
    public void Select(bool isSelected, Sprite newSprite)
    {
        if (isSelected)
        {
            buttonImage.sprite = newSprite;

        }
        else
        {
            buttonImage.sprite = newSprite;
        }
    }
}
