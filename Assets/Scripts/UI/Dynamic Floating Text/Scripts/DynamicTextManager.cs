using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI.Dynamic_Floating_Text.Scripts;

public class DynamicTextManager : MonoBehaviour
{

    public static DynamicTextData defaultData;
    public static DynamicTextData enemyHeal;
    public static DynamicTextData playerHeal;
    public static DynamicTextData playerDamage;
    public static GameObject canvasPrefab;
    public static Transform mainCamera;

    [SerializeField] private DynamicTextData _defaultData;
    [SerializeField] private DynamicTextData _enemyHeal;
    [SerializeField] private DynamicTextData _playerHeal;
    [SerializeField] private DynamicTextData _playerDamage;
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private Transform _mainCamera;

    private void Awake()
    {
        defaultData = _defaultData;
        enemyHeal = _enemyHeal;
        playerHeal = _playerHeal;
        playerDamage = _playerDamage;
        mainCamera = _mainCamera;
        canvasPrefab = _canvasPrefab;
    }

    public static void CreateText2D(Vector2 position, string text, DynamicTextData data)
    {
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText2D>().Initialise(text, data);
    }

    public static void CreateText(Vector3 position, string text, DynamicTextData data)
    {
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
    }

}
