using UnityEngine;

public class ElemDestroy : MonoBehaviour
{
    void Start()
    {
        Messenger.AddListener(GameEvent.DESTROY, destroy);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DESTROY, destroy);
    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
}
