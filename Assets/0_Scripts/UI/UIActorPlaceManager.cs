using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UIActorPlaceManager : MonoBehaviour
    {
        private List<UIActorPlaceButton> _buttons;
        private Actor _placingActor;

        private void Awake()
        {
            _buttons = GetComponentsInChildren<UIActorPlaceButton>().ToList();
            foreach (var button in _buttons)
            {
                button.Initialize(this);
            }
        }

        public void OnClick_ActorButton(UIActorPlaceButton button)
        {
            foreach (var button0 in _buttons)
            {
                button0.SetPressed(false);
            }

            button.SetPressed(true);
            StartPlacingActor(button.ActorType);
        }

        public void StartPlacingActor(ActorType actorType)
        {
            StopPlacingActor();

            _placingActor = InGameManager.ActorManager.CreatePlacingActor(actorType, Team.Player, 1);
            StartCoroutine(nameof(PlaceActorCo));
        }

        public void PlaceActor(Tile tile)
        {
            _placingActor.View.SpriteRenderer.sortingOrder = Constant.ActorSortingOrder;

            if (tile.Coord.x >= Constant.Instance.MapSize.x / 2)
            {
                if (!GameSetting.Instance.TestEnemyPlace)
                {
                    StopPlacingActor();
                    return;
                }

                _placingActor.SetTeam(Team.CPU);
                _placingActor.SetDirection(-1);
            }

            InGameManager.ActorManager.SpawnActor(_placingActor, tile);

            _placingActor = null;
            StopPlacingActor();
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
                        StopPlacingActor();
                    }

                    yield break;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    StopPlacingActor();
                    yield break;
                }

                isFirstFrame = false;
                yield return null;
            }
        }

        public void StopPlacingActor()
        {
            foreach (var button in _buttons)
            {
                button.SetPressed(false);
            }

            StopCoroutine(nameof(PlaceActorCo));
            if (_placingActor != null)
            {
                Destroy(_placingActor.gameObject);
            }
        }
    }
}
