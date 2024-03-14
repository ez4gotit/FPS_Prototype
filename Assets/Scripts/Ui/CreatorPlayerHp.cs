using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class CreatorPlayerHp : MonoBehaviour
    {
        public static CreatorPlayerHp instance;
        [SerializeField] private GameObject PrefabHp;
        [SerializeField] private Transform ParentSpawnImageHp;

        private void Awake()
        {
            if (instance != null) Destroy(instance);
            instance = this;

        }

        //+Add Player Hp.
        public void Creator()
        {
            Instantiate(PrefabHp, Vector3.zero, Quaternion.identity, PrefabHp.transform);
        }
    }
}