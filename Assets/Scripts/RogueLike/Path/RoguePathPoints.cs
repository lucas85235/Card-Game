using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoguePathPoints : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PointType type = PointType.Enemy;
    [SerializeField] private List<RoguePathPoints> nexts;

    [HideInInspector] public Button button;
    private Image image;

    public PointType Type
    {
        get => type;
        set
        {
            type = value;
            SetImage();
        }
    }

    public List<RoguePathPoints> Nexts
    {
        get => nexts;
        set => nexts = value;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        button.onClick.AddListener(SetClick);
        Type = type;
    }

    private void SetClick()
    {
        FindObjectOfType<RoguePath>().NextPosition(this);
    }

    private void SetImage()
    {
        var sprites = Resources.LoadAll<Sprite>("Map");

        foreach (var sprite in sprites as Sprite[])
        {
            if (sprite.name == Type.ToString())
                image.sprite = sprite;
        }
    }

    public enum PointType
    {
        Enemy,
        Item,
        Event,
        Shop,
        Workshop,
        Boss,
    }
}
