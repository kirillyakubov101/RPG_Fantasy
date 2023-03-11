using UnityEngine;
using System.Linq;

namespace FantasyTown.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private static SavingWrapper _instance;
        private SavingData _data;
         

        private void Awake()
        {
            if (_instance == null) { _instance = this; }

            else { Destroy(gameObject); return; }

            _data = SavingData.Instance;
        }

        public static SavingWrapper Instance { get { return _instance; } private set { } }

        public SavingData Data { get => _data;  }

        public void SaveAll()
        {
            var allSavedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();
            foreach (var obj in allSavedObjects)
            {
                obj.CaptureState();
            }

            SavingSystem.SaveFile(_data);
        }

        public void LoadAll()
        {
            SavingData.Instance = SavingSystem.LoadFile() as SavingData;
            _data = SavingData.Instance;
            var allSavedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToArray();
            
            foreach (var obj in allSavedObjects)
            {
                obj.RestoreState();
            }
        }
    }
}


