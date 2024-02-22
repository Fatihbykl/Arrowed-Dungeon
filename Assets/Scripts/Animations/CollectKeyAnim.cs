using Cysharp.Threading.Tasks;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectKeyAnim : MonoBehaviour
{
    [Header("Key Rise Animation Settings")]
    [SerializeField] private float keyHeightBeforeMove = 3f;
    [SerializeField] private float riseDuration = 1f;
    
    [Header("Key Follow Animation Settings")]
    [SerializeField] private float keySpeed = 15f;
    [SerializeField] private float distanceBetweenTargetAndKey = 0.5f;
    [SerializeField] private float targetEndHeight = 1.5f;
    
    private Label coinText;
    private VisualElement keyIcon;
    private GameObject playerObject;
    private Vector3 targetPosition;
    private Transform keyTransform;
    private Tweener keyTweener;

    private void OnEnable()
    {
        GameplayEvents.KeyCollected += OnKeyCollected;
    }

    private void OnDisable()
    {
        GameplayEvents.KeyCollected -= OnKeyCollected;
    }

    private async void OnKeyCollected(int arg1, int arg2, GameObject collectedKey, GameObject target)
    {
        if (gameObject != collectedKey) { return; }

        playerObject = target;
        keyTransform = transform;
        StartCollectedAnimation();
        await UniTask.WaitForSeconds(riseDuration);
        StartKeyFollow();
    }

    private void StartCollectedAnimation()
    {
        keyTransform.DOMoveY(keyHeightBeforeMove, riseDuration);
    }

    private void StartKeyFollow()
    {
        keyTweener = keyTransform.DOMove(targetPosition, keySpeed).SetSpeedBased(true).SetAutoKill(false);
        keyTweener.OnUpdate(() =>
        {
            var distance = Vector3.Distance(keyTransform.position, targetPosition);
            if (distance > distanceBetweenTargetAndKey)
            {
                targetPosition = playerObject.transform.position;
                targetPosition.y = targetEndHeight;
                keyTweener.ChangeEndValue(targetPosition, true);
            }
            else
            {
                DOTween.KillAll();
                Destroy(keyTransform.gameObject);
                GameplayEvents.KeyAnimationFinished.Invoke();
            }
        });
    }

}