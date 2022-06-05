using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanelUI : MonoBehaviour
{
    [Header("Chest")]
    [SerializeField] private GameObject _chestEventPanel;
    [SerializeField] private Button _chestStartButton;

    [Header("Shop")]
    [SerializeField] private GameObject _consumablesShopEventPanel;
    [SerializeField] private Button _consumablesShopButton;
    [SerializeField] private GameObject _upgradesShopEventPanel;
    [SerializeField] private Button _upgradesShopButton;

    [Header("Shop")]
    [SerializeField] private GameObject _workshopEventPanel;
    [SerializeField] private Button _workshopStartButton;

    [Header("Shop")]
    [SerializeField] private List<EventPanels> _eventPanels;

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

        _chestStartButton.onClick.RemoveAllListeners();
        _consumablesShopButton.onClick.RemoveAllListeners();
        _upgradesShopButton.onClick.RemoveAllListeners();
        _workshopStartButton.onClick.RemoveAllListeners();
        foreach (var item in _eventPanels)
            item._eventStartButton.onClick.RemoveAllListeners();

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

                if (Random.Range(0, 2) == 0)
                {
                    _consumablesShopEventPanel.SetActive(true);
                    _consumablesShopButton.onClick.AddListener(() =>
                        EnableNextPoints(point));
                }
                else
                {
                    _upgradesShopEventPanel.SetActive(true);
                    _upgradesShopButton.onClick.AddListener(() =>
                        EnableNextPoints(point));
                }

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

                _workshopEventPanel.SetActive(true);
                _workshopStartButton.onClick.AddListener(() =>
                    EnableNextPoints(point));
                break;

            case RoguePathPoints.PointType.Event:

                if (eventBreak)
                {
                    EnableNextPoints(point);
                    return;
                }

                var r = Random.Range(0, _eventPanels.Count);
                _eventPanels[r]._eventPanel.SetActive(true);
                _eventPanels[r]._eventStartButton.onClick.AddListener(() =>
                    EnableNextPoints(point));
                break;
        }

    }

    private void EnableNextPoints(RoguePathPoints point)
    {
        _chestEventPanel.SetActive(false);
        _consumablesShopEventPanel.SetActive(false);
        _upgradesShopEventPanel.SetActive(false);
        _workshopEventPanel.SetActive(false);

        foreach (var item in _eventPanels)
            item._eventPanel.SetActive(false);

        foreach (var item in point.Nexts)
        {
            item.button.interactable = true;
        }
    }

    [System.Serializable]
    public struct EventPanels
    {
        public GameObject _eventPanel;
        public Button _eventStartButton;
    }
}
