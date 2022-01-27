using System;
using System.Numerics;
using UnityEngine;

namespace Demo
{
    [Serializable]
    public class DemoCellData : IReuseCellData
    {
        private int m_index;
        public int index { get { return m_index; } set { m_index = value; } }

        private int m_id;
        public int id { get { return m_id; } set { m_id = value; } }

        [SerializeField]
        private string m_name;
        public string name { get { return m_name; } set { m_name = value; } }

        private string m_imageName;
        public string imageName { get { return m_imageName; } set { m_imageName = value; } }

        //Ãß°¡
        private int m_level;
        public int level { get { return m_level; } set { m_level = value; } }

        private BigInteger m_cost;
        public BigInteger cost { get { return m_cost; } set { m_cost = value; } }
    }
}