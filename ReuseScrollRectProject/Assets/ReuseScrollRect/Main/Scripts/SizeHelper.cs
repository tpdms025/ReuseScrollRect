using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIReuseGrid))]
public class SizeHelper : MonoBehaviour, LoopScrollSizeHelper
{
    private LoopScrollRectMulti m_LoopScrollRect;

    [HideInInspector]
    public ReuseBankBase m_ListBank;

    private void Start()
    {
        m_LoopScrollRect = GetComponent<UIReuseGrid>().m_ScrollRect;
        m_LoopScrollRect.sizeHelper = this;
        m_ListBank = GetComponent<UIReuseGrid>().m_ReuseBank;
    }

    public Vector2 GetItemsSize(int itemsCount)
    {
        if (itemsCount <= 0) return new Vector2();
        int count = m_ListBank.GetListDataLength();
        Vector2 sum = new Vector2();
        for (int i = 0; i < count; i++)
        {
            if (itemsCount <= i) break;
            int t = (itemsCount - 1 - i) / count + 1;
            sum += t * m_ListBank.GetCellPreferredSize(i);
        }
        return sum;
    }
}
