using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old
{
    public class DataList_Base<T> : Data_Base where T : Data_Base
    {
        public List<T> datas;
    }
}