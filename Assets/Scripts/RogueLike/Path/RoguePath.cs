using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoguePath : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _player;

    [Header("Points")]
    [SerializeField] private List<RoguePathPoints> _points;

    [Space]
    public UnityEvent<RoguePathPoints> OnUpdatePoint;

    private string savePointKey = "SAVE_POINT";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(savePointKey))
            PlayerPrefs.SetString(savePointKey, _points[0].name);

        if (PlayerPrefs.HasKey(savePointKey))
        {
            var pointName = PlayerPrefs.GetString(savePointKey);

            foreach (var point in _points)
            {
                if (point.name == pointName)
                {
                    SetPosition(point, true, false);
                    break;
                }
            }
        }

        OnUpdatePoint.AddListener(SavePath);
    }

    public void SavePath(RoguePathPoints point)
    {
        PlayerPrefs.SetString(savePointKey, point.name);
        PlayerPrefs.Save();
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(savePointKey);
    }

    public void NextPosition(RoguePathPoints path)
    {
        SetPosition(path);
    }

    private void SetPosition(RoguePathPoints path, bool enableNexts = false, bool callEvent = true)
    {
        _player.SetParent(path.transform);
        _player.localPosition = Vector3.zero;

        foreach (var point in _points)
        {
            point.button.interactable = false;
        }

        if (enableNexts)
        {
            foreach (var item in path.Nexts)
            {
                item.button.interactable = true;
            }
        }

        if (callEvent) OnUpdatePoint?.Invoke(path);
    }
}
