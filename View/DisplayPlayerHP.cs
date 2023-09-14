using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils.injection;

namespace View
{
    public sealed class DisplayPlayerHP : InjectableBehaviour
    {
        [Inject] private PlayerStateModel _model;

        [SerializeField] private bool player;
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
            _model.Updated.Add(Redraw);
            Redraw();
        }

        private void Redraw()
        {
            _text.text = (player ? _model.Player : _model.Opponent)?.Health.ToString();
        }

        private void OnDestroy()
        {
            _model.Updated.Remove(Redraw);
        }
    }
}