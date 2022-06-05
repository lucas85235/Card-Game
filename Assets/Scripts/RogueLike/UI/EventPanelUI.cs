using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private Button _startButton;

    [Header("Chest UI")]
    [SerializeField] private GameObject _chestEventPanel;
    [SerializeField] private Button _chestStartButton;

    [Header("To Test")]
    [SerializeField] private bool enemyBreak = false;
    [SerializeField] private bool itemBreak = false;
    [SerializeField] private bool shopBreak = false;
    [SerializeField] private bool bossBreak = false;
    [SerializeField] private bool workshopBreak = false;
    [SerializeField] private bool eventBreak = false;

    private RoguePathPoints savePoint;

    public void SetEventPanel(RoguePathPoints point)
    {
        savePoint = point;

        _startButton.onClick.RemoveAllListeners();
        _chestStartButton.onClick.RemoveAllListeners();

        switch (point.Type)
        {
            case RoguePathPoints.PointType.Enemy:

                if (enemyBreak)
                {
                    EnableNextPoints(point);
                    return;
                }
                
                SceneLoader.Instance.LoadScene("GameRogueLike");
                break;

            case RoguePathPoints.PointType.Item:

                if (itemBreak)
                {
                    EnableNextPoints(point);
                    return;
                }

                _chestEventPanel.SetActive(true);
                _chestStartButton.onClick.AddListener(() =>
                    EnableNextPoints(point));
                break;

            case RoguePathPoints.PointType.Shop:

                if (shopBreak)
                {
                    EnableNextPoints(point);
                    return;
                }

                _eventPanel.SetActive(true);
                _startButton.onClick.AddListener(() =>
                    EnableNextPoints(point));
                break;

            case RoguePathPoints.PointType.Boss:

                if (bossBreak)
                {
                    EnableNextPoints(point);
                    return;
                }
                SceneLoader.Instance.LoadScene("GameRogueLikeBoss");
                break;

            case RoguePathPoints.PointType.Workshop:

                if (workshopBreak)
                {
                    EnableNextPoints(point);
                    return;
                }
                break;

            case RoguePathPoints.PointType.Event:

                if (eventBreak)
                {
                    EnableNextPoints(point);
                    return;
                }
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
