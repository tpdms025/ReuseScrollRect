using System.Collections.Generic;
using UnityEngine;
using System;

// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.

public class DemoTestScrollView : MonoBehaviour
{
    public UIReuseGrid grid { get; private set; }

    public List<DemoDB> database;

    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
    }

    private void Start()
    {
        database = CreateDemoDB();

        for (int i = 0; i < database.Count; ++i)
        {
            DemoCellData cell = new DemoCellData();
            cell.index = i;
            cell.name = database[i].name;

            grid.AddItem(cell);
        }

        grid.RefreshAllCell();
    }

    /// <summary>
    /// 임시로 DB를 만든다.
    /// </summary>
    /// <returns></returns>
    private List<DemoDB> CreateDemoDB()
    {
        List<DemoDB> _database = new List<DemoDB>();
        for (int i = 0; i < 10; i++)
        {
            string name = "ItemCell_" + i.ToString();
            _database.Add(new DemoDB(name));
        }
        return _database;
    }
}


[Serializable]
public class DemoDB
{
    public string name;

    public DemoDB(string _name)
    {
        name = _name;
    }
}
