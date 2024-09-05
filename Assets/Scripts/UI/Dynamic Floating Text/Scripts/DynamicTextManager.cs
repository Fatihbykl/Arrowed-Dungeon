using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Dynamic_Floating_Text.Scripts
{
    public class DynamicTextManager : MonoBehaviour
    {
        [SerializeField] private DynamicTextData defaultData;
        [SerializeField] private DynamicTextData enemyDamageData;
        [SerializeField] private DynamicTextData enemyHealData;
        [SerializeField] private DynamicTextData playerHealData;
        [SerializeField] private DynamicTextData playerDamageData;
        [SerializeField] private DynamicTextData goldData;
        [SerializeField] private DynamicTextData gemData;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private GameObject canvasPrefab;

        public DynamicTextData DefaultData => defaultData;
        public DynamicTextData EnemyDamageData => enemyDamageData;
        public DynamicTextData EnemyHealData => enemyHealData;
        public DynamicTextData PlayerHealData => playerHealData;
        public DynamicTextData PlayerDamageData => playerDamageData;
        public DynamicTextData GoldData => goldData;
        public DynamicTextData GemData => gemData;
        public Transform MainCamera => mainCamera;
        
        public static DynamicTextManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Dynamic Text Manager in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
        }

        public void CreateText2D(Vector2 position, string text, DynamicTextData data)
        {
            GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
            newText.transform.GetComponent<DynamicText2D>().Initialise(text, data);
        }

        public void CreateText(Transform targetTransform, string text, DynamicTextData data)
        {
            GameObject newText = Instantiate(canvasPrefab, GetRandomTextPosition(targetTransform), Quaternion.identity);
            newText.transform.GetComponent<DynamicText>().Initialise(text, data);
        }

        private Vector3 GetRandomTextPosition(Transform target)
        {
            var textPos = new Vector3(target.position.x, 2f, target.position.z);
            textPos.x += (Random.value - 0.5f) / 3f;
            textPos.y += Random.value;
            textPos.z += (Random.value - 0.5f) / 3f;

            return textPos;
        }
    }
}
