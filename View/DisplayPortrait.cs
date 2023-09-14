using System.Collections;
using System.Collections.Generic;
using Commands;
using Model;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils.injection;

namespace View
{
    public class DisplayPortrait : InjectableBehaviour
    {
        [Inject] private PlayerStateModel _model;
        [SerializeField] private bool isPlayer;

        private Image _image;


        void Start()
        {
            _image = GetComponent<Image>();
            _model.Updated.Add(OnModelUpdated);
        }

        private void OnModelUpdated()
        {
            _model.Updated.Remove(OnModelUpdated);
            StartCoroutine(DownloadImage((isPlayer ? _model.Player : _model.Opponent).User.PortraitSrc));
        }


        private IEnumerator DownloadImage(string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                var tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                var rect = new Rect(0, 0, tex.width, tex.height);
                var sprite = Sprite.Create(tex, rect, new Vector2(rect.width / 2, rect.height / 2));
                _image.overrideSprite = sprite;
                _image.color = Color.white;
            }
        }

        void OnDestroy()
        {
            _model.Updated.Remove(OnModelUpdated);
        }
    }
}