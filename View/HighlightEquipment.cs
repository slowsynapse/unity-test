using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils.injection;

namespace View
{
    public class HighlightEquipment : InjectableBehaviour
    {
        [Inject] private PlayerStateModel _model;

        [SerializeField] private bool player;
        [SerializeField] private Image[] slots;

        [SerializeField] private bool useAlpha;


        private void Start()
        {
            _model.Updated.Add(OnUpdated);
        }

        private void OnUpdated()
        {
            var state = (player ? _model.Player : _model.Opponent).EquipmentSlots;

            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;
                
                var slotEnabled = state.ContainsKey(i) && state[i] != null;
                if (useAlpha)
                    slots[i].color = new Color(1, 1, 1, slotEnabled ? 1 : 0.05f);
                else
                    slots[i].gameObject.SetActive(slotEnabled);
            }
        }

        private void OnDestroy()
        {
            _model.Updated.Remove(OnUpdated);
        }
    }
}