using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    [SerializeField] Image optionImage;
    [SerializeField] Text optionText;
    public void Select(bool isSelected, Sprite newSprite)
    {
        if (isSelected)
        {
            optionImage.sprite = newSprite;
        }
        else
        {
            optionImage.sprite = newSprite;
        }
    }
}
