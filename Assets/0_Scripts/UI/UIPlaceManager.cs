using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIPlaceManager : MonoBehaviour
    {
        [Serializable]
        public class KeyCodes
        {
            public List<KeyCode> Codes;
        }

        [SerializeField] private GameObject _food;
        [SerializeField] private List<KeyCodes> _keyCodes;

        private List<UIPlaceButton> _buttons;
        private Actor _placingActor;
        private UIPlaceButton _currentButton;

        private void Awake()
        {
            _buttons = GetComponentsInChildren<UIPlaceButton>().ToList();
            foreach (var button in _buttons)
            {
                button.Initialize(this, _keyCodes[button.transform.GetSiblingIndex()].Codes);
            }
        }

        public void OnClick_ActorButton(UIPlaceButtonActor actorButton)
        {
            StopPlacing();
            _currentButton = actorButton;
            StartPlacingActor(actorButton.ActorType);
            actorButton.SetPressed(true);
        }

        public void OnClick_FoodButton(UIPlaceButtonFood foodButton)
        {
            StopPlacing();
            _currentButton = foodButton;
            StartCoroutine(nameof(StartFeeding));
            foodButton.SetPressed(true);
        }

        public void StartPlacingActor(ActorType actorType)
        {
            _placingActor = InGameManager.ActorManager.CreatePlacingActor(actorType, Team.Player, 1);
            _placingActor.View.AdjustSortingOrders(Constant.PlacingOrder);
            StartCoroutine(nameof(PlaceActorCo));
        }

        public void PlaceActor(Tile tile)
        {
            if (IsEnemyArea(tile))
            {
                if (!GameSetting.Instance.TestEnemyPlace)
                {
                    StopPlacing();
                    return;
                }

                _placingActor.SetTeam(Team.CPU);
                _placingActor.SetDirection(-1);
            }

            _placingActor.View.AdjustSortingOrders(-Constant.PlacingOrder);
            AmazingAIScript.Instance.OnPlayerSpawned(tile, _placingActor);
            InGameManager.Instance.Gold -= _currentButton.GetCost();
            InGameManager.ActorManager.SpawnActor(_placingActor, tile);

            _placingActor = null;
            StopPlacing();
        }

        private bool IsEnemyArea(Tile tile)
        {
            if (tile == null)
            {
                return false;
            }

            return tile.Coord.x >= Constant.Instance.MapSize.x / 2;
        }

        private IEnumerator PlaceActorCo()
        {
            bool isFirstFrame = true;
            while (true)
            {
                var worldPosition = InGameUIManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;
                _placingActor.transform.position = worldPosition;

                var hoveredTile = InputManager.Instance.CurrentHoveredTile;
                if (hoveredTile != null)
                {
                    _placingActor.transform.position = hoveredTile.transform.position;
                }

                if (!isFirstFrame && (Input.GetMouseButtonUp(0)))
                {
                    if (hoveredTile != null)
                    {
                        PlaceActor(InputManager.Instance.CurrentHoveredTile);
                    }
                    else
                    {
                        StopPlacing();
                    }

                    yield break;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    StopPlacing();
                    yield break;
                }

                isFirstFrame = false;
                yield return null;
            }
        }

        public void StopPlacing()
        {
            foreach (var button in _buttons)
            {
                button.SetPressed(false);
            }

            StopCoroutine(nameof(PlaceActorCo));
            StopCoroutine(nameof(FeedingCo));
            if (_placingActor != null)
            {
                Destroy(_placingActor.gameObject);
            }

            _food.SetActive(false);

            _currentButton = null;
        }

        public void StartFeeding()
        {
            _food.SetActive(true);
            StartCoroutine(nameof(FeedingCo));
        }

        private IEnumerator FeedingCo()
        {
            while (true)
            {
                var worldPosition = InGameUIManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;
                _food.transform.position = worldPosition;

                if (Input.GetMouseButtonDown(0))
                {
                    if (!IsEnemyArea(InputManager.Instance.CurrentHoveredTile))
                    {
                        var hit = Physics2D.Raycast(worldPosition, Vector2.up, 0.01f, 1 << LayerMask.NameToLayer("Actor"));
                        if (hit.collider != null)
                        {
                            var actor = hit.collider.GetComponent<Actor>();
                            if (actor.Team == Team.Player && actor.CanLevelUp)
                            {
                                int cost = actor.Data.Grade * (int) Mathf.Pow(2, actor.Level - 1);
                                if (InGameManager.Instance.Gold >= cost)
                                {
                                    InGameManager.Instance.Gold -= cost;
                                    actor.LevelUp();
                                }
                            }
                        }
                    }

                    StopPlacing();
                    yield break;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    StopPlacing();
                    yield break;
                }

                yield return null;
            }
        }
    }
}
