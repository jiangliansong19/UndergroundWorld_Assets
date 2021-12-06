using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTrees : MonoBehaviour
{

    private bool _isCutting;
    private Transform cuttingLogoTransform;

    private ResourceTypeHolder resourceTypeHolder;

    private float cuttingPercent; //100%表示树被砍倒了

    private void Awake()
    {
        cuttingLogoTransform = transform.Find("wood-axe");
        cuttingLogoTransform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        resourceTypeHolder = transform.GetComponent<ResourceTypeHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            AddCuttingPercent(0.2f);
        }
    }

    public void SetIsCutting(bool isCutting)
    {
        _isCutting = isCutting;

        cuttingLogoTransform.gameObject.SetActive(true);
    }

    public void AddCuttingPercent(float percent)
    {
        cuttingPercent += percent;
        //if (cuttingPercent >= 1.00f)
        //{
        //    ResourcesManager.Instance.AddResource(new ResourceTypeAmount() { resourceType = resourceTypeHolder.GetResourceTypeSO(), amount = 8 });

        //    Destroy(gameObject);
        //}
    }
}
