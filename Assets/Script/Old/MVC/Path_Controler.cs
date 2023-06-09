using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Old
{
    public class Path_Controler
    {
        private Path_Controler_View view;

        public void Init()
        {
            var data = view.GetData();
            view.GetView(data.datas[0]);
        }
    }
    public class Path_Controler_View : ViewList_Base<PathItem_View, PathList_Data, PathItemData>
    {
        public override void InitPerView()
        {

        }

        protected override void OnHide()
        {

        }

        protected override void OnInit()
        {

        }

        protected override void OnRefresh()
        {

        }

        protected override void OnShow()
        {

        }
    }
    public class PathList_Data : DataList_Base<PathItemData>
    {

    }
}
