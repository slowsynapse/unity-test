using UnityEngine;

namespace Utils.injection
{
    public class InjectableBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Injector.Instance.Resolve(this, gameObject.GetInstanceID());
        }
    }
}