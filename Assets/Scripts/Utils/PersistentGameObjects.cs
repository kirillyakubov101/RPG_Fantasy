using UnityEngine;

namespace FantasyTown.Utils.Persistent
{
    //[RuntimeInitializeOnLoadMethod]
    public class PersistentGameObjects : MonoBehaviour
    {
        private void Awake()
        {
            int count = FindObjectsOfType<PersistentGameObjects>().Length;

            if (count > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}


