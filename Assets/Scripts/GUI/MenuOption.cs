using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    [SerializeField] Image pipeImage;
    [SerializeField] Text optionText;
    public void Select(bool isSelected, Sprite newSprite)
    {
        if (isSelected)
        {
            pipeImage.sprite = newSprite;

        }
        else
        {
            pipeImage.sprite = newSprite;
        }
    }
}
