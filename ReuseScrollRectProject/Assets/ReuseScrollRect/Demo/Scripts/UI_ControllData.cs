using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControllData : MonoBehaviour
{
    public Button m_ButtonAddData;
    public Button m_ButtonSetData;
    public Button m_ButtonDelData;
    public Button m_ButtonSortData;
    public Button m_ButtonReverseSortData;
    public Button m_ButtonSrollToCell;
    public Button m_ButtonSrollToCellWithTime;

    public InputField m_InputFieldSrollToCell_CellIndex;
    public InputField m_InputButtonSrollToCell_MoveSpeed;

    public InputField m_InputFieldSrollToCellWithTime_CellIndex;
    public InputField m_InputButtonSrollToCellWithTime_MoveSpeed;

    public DemoTestScrollView m_demoTestScroll;
    private UIReuseGrid m_grid;


    // Start is called before the first frame update
    private void Start()
    {
        m_grid = m_demoTestScroll.grid;

        m_ButtonAddData.onClick.AddListener(OnButtonAddDataClick);
        m_ButtonSetData.onClick.AddListener(OnButtonSetDataClick);
        m_ButtonDelData.onClick.AddListener(OnButtonDelDataClick);
        m_ButtonSortData.onClick.AddListener(OnButtonSortDataClick);
        m_ButtonReverseSortData.onClick.AddListener(OnButtonReverseSortDataClick);
        m_ButtonSrollToCell.onClick.AddListener(OnButtonSrollToCellClick);
        m_ButtonSrollToCellWithTime.onClick.AddListener(OnButtonSrollToCellWithTimeClick);
    }

    private void OnDestroy()
    {
        m_ButtonAddData.onClick.RemoveListener(OnButtonAddDataClick);
        m_ButtonSetData.onClick.RemoveListener(OnButtonSetDataClick);
        m_ButtonDelData.onClick.RemoveListener(OnButtonDelDataClick);
        m_ButtonSortData.onClick.RemoveListener(OnButtonSortDataClick);
        m_ButtonReverseSortData.onClick.RemoveListener(OnButtonReverseSortDataClick);
        m_ButtonSrollToCell.onClick.RemoveListener(OnButtonSrollToCellClick);
        m_ButtonSrollToCellWithTime.onClick.RemoveListener(OnButtonSrollToCellWithTimeClick);
    }

    private void OnButtonAddDataClick()
    {
        int RandomResult = Random.Range(0, 10);

        DemoCellData cell = new DemoCellData();
        cell.name = "A";

        m_grid.AddItem(cell, true);
    }

    //현재 버그있음.
    private void OnButtonSetDataClick()
    {
        List<DemoCellData> contents = new List<DemoCellData>{
                new DemoCellData(){name = "a",}, new DemoCellData(){name = "b",}, new DemoCellData(){name = "c",}
            };

        //Casting
        //List<IReuseCellData> list = new List<DemoCellData>().ConvertAll(x=>(IReuseCellData)x);
        List<IReuseCellData> list = new List<IReuseCellData>();
        foreach (DemoCellData cell in contents)
        {
            list.Add(cell);
        }
        m_grid.SetItem(list, true);
    }

    private void OnButtonDelDataClick()
    {
        m_grid.RemoveFirstItem();
    }

    private void OnButtonSortDataClick()
    {
        m_grid.SortCellData_IndexOrder();
    }

    private void OnButtonReverseSortDataClick()
    {
        m_grid.SortCellData_IndexOrder(true);
    }

    private void OnButtonSrollToCellClick()
    {
        int Index = 0;
        int.TryParse(m_InputFieldSrollToCell_CellIndex.text, out Index);

        int MoveSpeed = 0;
        int.TryParse(m_InputButtonSrollToCell_MoveSpeed.text, out MoveSpeed);

        m_grid.SrollToCell(Index, MoveSpeed);
    }

    private void OnButtonSrollToCellWithTimeClick()
    {
        int Index = 0;
        int.TryParse(m_InputFieldSrollToCellWithTime_CellIndex.text, out Index);

        float MoveTime = 0;
        float.TryParse(m_InputButtonSrollToCellWithTime_MoveSpeed.text, out MoveTime);

        m_grid.SrollToCellWithinTime(Index, MoveTime);
    }
}
