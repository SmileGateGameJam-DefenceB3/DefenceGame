using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
            StartPlacingActor(button.ActorPrefab);
        }

        public void StartPlacingActor(Actor actorPrefab)
        {
            StopPlacingActor();

            _placingActor = Instantiate(actorPrefab);
            _placingActor.Initialize(1, Team.Player);
            StartCoroutine(nameof(PlaceActorCo));
        }

        public void PlaceActor(Tile tile)
        {
            _placingActor.View.SpriteRenderer.sortingOrder = Constant.ActorSortingOrder;
            InGameManager.ActorManager.SpawnActor(_placingActor, Team.Player, tile);
            
            _placingActor = null;
            StopPlacingActor();
        }
        
        private IEnumerator PlaceActorCo()
        {
            _placingActor.View.SpriteRenderer.sortingOrder = Constant.PlacingActorSortingOrder;

            bool isFirstFrame = true;
            while (true)
            {
                var worldPosition = InGameUIManager.Instance.UICamera.ScreenToWorldPoint(Input.mousePosition);
                _placingActor.transform.position = worldPosition;

                var hoveredTile = InputManager.Instance.CurrentHoveredTile;
                if (hoveredTile != null)
                {
                    _placingActor.transform.position = hoveredTile.transform.position;
                }

                if (!isFirstFrame && Input.GetMouseButtonUp(0))
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
