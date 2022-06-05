using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RogueChest : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    private Sprite[] sprites;

    private void Awake() 
    {
        sprites = Resources.LoadAll<Sprite>("RogueLikeIcons");
    }

    private void OnEnable()
    {
        var rand = Random.Range(0, sprites.Length);

        image.sprite = sprites[rand];
        text.text = "+1";
        
        switch (sprites[rand].name)
        {
            case "attack": RogueItems.Instance.Attack += 1;
                break;
            case "defense": RogueItems.Instance.Defense += 1;
                break;
            case "intelligence": RogueItems.Instance.Intelligence += 1;
                break;
            case "velocity": RogueItems.Instance.Velocity += 1;
                break;
            case "repairs": RogueItems.Instance.Repairs += 1;
                break;
            case "stickers": RogueItems.Instance.Stickers += 1;
                break;
            case "smokes": RogueItems.Instance.Smokes += 1;
                break;
        }
    }
}
