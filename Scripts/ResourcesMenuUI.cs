using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示资源数量
/// </summary>
public class ResourcesMenuUI : MonoBehaviour
{
    [SerializeField] private RectTransform parantCanvas;

    private Transform templateTransform;
    private float itemHeight = 20f;


    private void Awake()
    {
        templateTransform = transform.Find("ResourcesMenuItem");
        templateTransform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<ResourceTypeSO, ulong> resourceDict = ResourcesManager.Instance.GetResourcesDictionary();

        int i = 0;
        foreach (ResourceTypeSO item in resourceDict.Keys)
        {
            if (resourceDict[item] > 0)
            {
                Transform aTransform = Instantiate(templateTransform, transform);
                aTransform.gameObject.SetActive(true);
                aTransform.Find("Icon").GetComponent<Image>().sprite = item.sprite;
                aTransform.Find("Amount").GetComponent<TextMeshProUGUI>().text = resourceDict[item].ToString();

                aTransform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -itemHeight * i, 0);
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
