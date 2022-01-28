using System.Collections.Generic;
using UnityEngine;

public class ReuseBankBase
{
    private List<IReuseCellData> m_listData;

    // Cell Sizes
    public List<Vector2> m_CellSizes;

    public ReuseBankBase()
    {
        m_listData = new List<IReuseCellData>();
        m_CellSizes = new List<Vector2>();
    }



    // Get Cell in list by index
    public IReuseCellData GetCellData(int index)
    {
        return m_listData[index];
    }

    // Get Cell count in list
    public int GetListDataLength()
    {
        if (m_listData != null)
            return m_listData.Count;
        else
            return 0;
    }

    // Get cell preferred type index by index
    public int GetCellPreferredTypeIndex(int index)
    {
        var TempConten = GetCellData(index);

        int TempData = TempConten.index;
        int ResultIndex = Mathf.Abs(TempData) % m_CellSizes.Count;

        return ResultIndex;
    }

    // Get cell preferred size by index
    public Vector2 GetCellPreferredSize(int index)
    {
        int ResultIndex = GetCellPreferredTypeIndex(index);

        if (m_CellSizes.Count.Equals(0))
            return Vector2.zero;
        else
            return m_CellSizes[ResultIndex];
    }

    /// <summary>
    /// 리스트를 생성했는지 확인한다.
    /// </summary>
    /// <returns></returns>
    public bool IsInit()
    {
        return m_listData != null ? true : false;
    }

    public void AddCellData(IReuseCellData newData)
    {
        m_listData.Add(newData);
    }

    public void InsertCellData(int index, IReuseCellData newData)
    {
        m_listData.Insert(index, newData);
    }

    public void DelCellDataByIndex(int index)
    {
        if (m_listData.Count <= index)
        {
            return;
        }
        m_listData.RemoveAt(index);
    }
    public void DeleteCellData(IReuseCellData data)
    {
        m_listData.Remove(data);
    }

    public void ClearListData()
    {
        m_listData.Clear();
    }

    public void SetListData(List<IReuseCellData> newListData)
    {
        m_listData = newListData;
    }

    public List<IReuseCellData> GetListData()
    {
        return m_listData;
    }
}
