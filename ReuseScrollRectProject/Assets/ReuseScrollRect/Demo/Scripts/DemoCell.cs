using UnityEngine;
using UnityEngine.UI;


public class DemoCell : UIReuseItemCell
{
    #region Fields
    [SerializeField] private Image preview;
    [SerializeField] private Text titleText;
    [SerializeField] private Text valueText;
    #endregion

    public override void UpdateData(int idx, IReuseCellData _CellData)
    {
        base.UpdateData(idx, _CellData);

        DemoCellData item = _CellData as DemoCellData;
        if (item == null)
            return;

        preview.name = item.imageName;
        titleText.text = idx.ToString();
        valueText.text = item.name;
    }
}
