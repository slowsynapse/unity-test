using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace view.behaviours.UI
{
    public class MoveUpAndFade : MonoBehaviour
    {
        public float Duration = 1;
        public Vector3 Offset = Vector3.up * 50; //px!

        private Text _text;
        private Vector3 _pos;
        private float _timePassed;

        public void Start()
        {
            _text = GetComponent<Text>();
            _pos = transform.position;
        }

        public void SetValue(string value)
        {
            var intValue = int.Parse(value);
            if (intValue > 10)
            {
                _text.color = Color.red;
            }
            else if (intValue > 5)
            {
                _text.text = "-" + value;
                _text.color = new Color(1, 0.5f, 0f, 1f);
            }
            else if (intValue > 0)
            {
                _text.text = "-" + value;
                _text.color = Color.yellow;
            }

            else
            {
                _text.color = Color.white;
                _text.text = "MISS!";
            }


            transform.position = _pos;
            _text.enabled = true;
            _timePassed = 0;
        }

        void Update()
        {
            if (_text == null || !_text.enabled)
                return;

            if (_timePassed > Duration)
            {
                _text.enabled = false;
                return;
            }

            transform.position = _pos + Offset * (float)Math.Sqrt(_timePassed / Duration);

            var color = _text.color;
            _text.color = new Color(color.r, color.g, color.b, 1 - (_timePassed / Duration));

            _timePassed += Time.deltaTime;
        }
    }
}