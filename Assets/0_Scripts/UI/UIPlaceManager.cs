using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIPlaceManager : MonoBehaviour
    {
        private List<UIPlaceButton> _buttons;
        private Actor _placingActor;

        private void Awake()
        {
            _buttons = GetComponentsInChildren<UIPlaceButton>().ToList();
            foreach (var button in _buttons)
            {
                button.Initialize(this);
            }
        }

        public void OnClick_ActorButton(UIPlaceButtonActor actorButton)
        {
            StopPlacing();
            StartPlacingActor(actorButton.ActorType);
            actorButton.SetPressed(true);
        }

        public void OnClick_FoodButton(UIPlaceButtonFood foodButton)
        {
            StopPlacing();
            StartCoroutine(nameof(StartFeeding));
            foodButton.SetPressed(true);
        }

        public void StartPlacingActor(ActorType actorType)
        {
            _placingActor = InGameManager.ActorManager.CreatePlacingActor(actorType, Team.Player, 1);
            StartCoroutine(nameof(PlaceActorCo));
        }

        public void PlaceActor(Tile tile)
        {
            _placingActor.View.SpriteRenderer.sortingOrder = Constant.ActorSortingOrder;

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

            InGameManager.ActorManager.SpawnActor(_placingActor, tile);

            _placingActor = null;
            StopPlacing();
        }

        private bool IsEnemyArea(Tile tile)
        {
            return tile.Coord.x >= Constant.Instance.MapSize.x / 2;
        }

        private IEnumerator PlaceActorCo()
        {
            _placingActor.View.SpriteRenderer.sortingOrder = Constant.PlacingActorSortingOrder;

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
            StopCoroutine(nameof(StartFeeding));
            if (_placingActor != null)
            {
                Destroy(_placingActor.gameObject);
            }
        }

        public void StartFeeding()
        {
            StartCoroutine(nameof(FeedingCo));
        }

        private IEnumerator FeedingCo()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!IsEnemyArea(InputManager.Instance.CurrentHoveredTile))
                    {
                        var worldPosition = InGameUIManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
                        worldPosition.z = 0;
                        var hit = Physics2D.Raycast(worldPosition, Vector2.up, 0.01f, 1 << LayerMask.NameToLayer("Actor"));
                        if (hit.collider != null)
                        {
                            var actor = hit.collider.GetComponent<Actor>();
                            if (actor.Team == Team.Player)
                            {
                                actor.LevelUp();
                            }
                        }
                    }

                    StopPlacing();
                    yield break;
                }

                yield return null;
            }
        }
    }
}
