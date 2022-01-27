using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReuseScroll;

namespace Demo
{
    public class UI_ControllData : MonoBehaviour
    {
        public Button m_ButtonAddData;
        public Button m_ButtonSetData;
        public Button m_ButtonDelData;
        public Button m_ButtonSortData;
        public Button m_ButtonReverseSortData;
        public Button m_ButtonSrollToCell;

        public InputField m_InputFieldSrollToCell_CellIndex;
        public InputField m_InputButtonSrollToCell_MoveSpeed;

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
        }

        private void OnDestroy()
        {
            m_ButtonAddData.onClick.RemoveListener(OnButtonAddDataClick);
            m_ButtonSetData.onClick.RemoveListener(OnButtonSetDataClick);
            m_ButtonDelData.onClick.RemoveListener(OnButtonDelDataClick);
            m_ButtonSortData.onClick.RemoveListener(OnButtonSortDataClick);
            m_ButtonReverseSortData.onClick.RemoveListener(OnButtonReverseSortDataClick);
            m_ButtonSrollToCell.onClick.RemoveListener(OnButtonSrollToCellClick);
        }

        private void OnButtonAddDataClick()
        {
            int RandomResult = Random.Range(0, 10);

            DemoCellData cell = new DemoCellData();
            cell.name = "A";

            m_grid.AddItem(cell,true);
        }

        private void OnButtonSetDataClick()
        {
            List<DemoCellData> contents = new List<DemoCellData>{
                new DemoCellData(){name = "a",}, new DemoCellData(){name = "b",}, new DemoCellData(){name = "c",}
            };

            //Casting
            List<IReuseCellData> list = new List<DemoCellData>().ConvertAll(x=>(IReuseCellData)x);
            m_grid.SetItem(list);
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
            m_grid.SortCellData_IndexOrder(false);
        }

        private void OnButtonSrollToCellClick()
        {
            int Index = 0;
            int.TryParse(m_InputFieldSrollToCell_CellIndex.text, out Index);

            int MoveSpeed = 0;
            int.TryParse(m_InputButtonSrollToCell_MoveSpeed.text, out MoveSpeed);

            m_grid.SrollToCell(Index, MoveSpeed);
        }
    }
}