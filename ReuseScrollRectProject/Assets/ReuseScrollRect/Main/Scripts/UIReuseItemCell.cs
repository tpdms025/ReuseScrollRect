using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIReuseItemCell : MonoBehaviour
{
    public string prefabName { get; set; }
    public int m_Index { get; private set; }
    public virtual void UpdateData(int idx, IReuseCellData CellData)
    {
        m_Index = idx;
        gameObject.name = prefabName + idx.ToString();
    }

    #region Match  LayoutElement
    public LayoutElement m_Element;
    // Set Element PreferredWidth
    public virtual void SetLayoutElementPreferredWidth(float value)
    {
        m_Element.preferredWidth = value;
    }

    // Set Element PreferredHeight
    public virtual void SetLayoutElementPreferredHeight(float value)
    {
        m_Element.preferredHeight = value;
    }
    #endregion
}
