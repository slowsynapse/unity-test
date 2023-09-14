using Commands;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils.injection;

namespace View
{
    [RequireComponent(typeof(Text))]
    public class TurnTimer : InjectableBehaviour
    {
        [Inject] private PlayerAction _playerAction;
        [Inject] private TurnReset _turnReset;
        
        private Text _text;
        
        private float _value;
        private bool _stopped;

        private void Start()
        {
            _text = GetComponent<Text>();
            _playerAction.Add(OnPlayerAction);
            _turnReset.Add(OnTurnReset);
        }
        

        // Update is called once per frame
        void Update()
        {
            if (_stopped)
                return;
            _value -= Time.deltaTime;

            if (_value < 0)
                return;

            var m = (int) _value / 60;
            var s = ((int) _value % 60).ToString().PadLeft(2, '0');
            _text.text =  $"{m}:{s}";
        }

        void Stop()
        {
            _stopped = true;
            _text.text = "READY";
        }
        
        void OnDestroy(){
            _playerAction.Remove(OnPlayerAction);
            _turnReset.Remove(OnTurnReset);
        }

        private void OnTurnReset(int value)
        {
            _value = value;
            _stopped = false;
        }

        private void OnPlayerAction(PlayerActionPayload p)
        {
            Stop();
        }
    }
}