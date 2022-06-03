using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _chestEventPanel;
    [SerializeField] private Button _chestStartButton;
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private Button _startButton;

    private RoguePathPoints savePoint;

    public void SetEventPanel(RoguePathPoints point)
    {
        savePoint = point;
        _startButton.onClick.RemoveAllListeners();

        switch (point.Type)
        {
            case RoguePathPoints.PointType.Enemy:
                // EnableNextPoints(point);
                SceneLoader.Instance.LoadScene("GameRogueLike");
                break;

            case RoguePathPoints.PointType.Item:
                _chestEventPanel.SetActive(true);
                _chestStartButton.onClick.AddListener(() =>
                    EnableNextPoints(point));
                break;

            case RoguePathPoints.PointType.Shop:
                _eventPanel.SetActive(true);
                _startButton.onClick.AddListener(() =>
                    EnableNextPoints(point));
                break;

            case RoguePathPoints.PointType.Boss:
                SceneLoader.Instance.LoadScene("GameRogueLikeBoss");
                break;

            case RoguePathPoints.PointType.Workshop:
                break;

            case RoguePathPoints.PointType.Event:
                break;
        }

    }

    private void EnableNextPoints(RoguePathPoints point)
    {
        _eventPanel.SetActive(false);
        _chestEventPanel.SetActive(false);

        foreach (var item in point.Nexts)
        {
            item.button.interactable = true;
        }
    }
}
