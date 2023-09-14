using UnityEngine;
using Utils.injection;

namespace Utils
{
    public class SetupInjections : MonoBehaviour
    {
        private void Awake()
        {
            Injector.Setup(GetType().Assembly);
        }
    }
}