using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BottomMenuSO : ScriptableObject
{
    [System.Serializable]
    public class BottomMenuItem
    {
        public string name;
        public BottomMenuType type;
        public Sprite sprite;
        [SerializeField] public List<BuildingTypeSO> buildingList;
    }





    public List<BottomMenuItem> menuList;
}
