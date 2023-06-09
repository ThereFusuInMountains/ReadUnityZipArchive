using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old
{
    public abstract class ViewList_Base<View, Datas, Data> : View_Base<Datas>
    where View : View_Base<Data>
    where Datas : DataList_Base<Data>
    where Data : Data_Base
    {
        protected View perView;
        protected List<View> views;

        public void AddView(View view)
        {
            views.Add(view);
        }
        public void RemoveView(View view)
        {
            views.Remove(view);
        }

        public View GetView(Data data)
        {
            var length = views.Count;
            for (int i = 0; i < length; i++)
            {
                var view = views[i];
                var temp_data = view.GetData();
                if (temp_data == default(Data))
                {
                    continue;
                }
                if (temp_data.Equals(data))
                {
                    return view;
                }
            }
            return default;
        }

        public abstract void InitPerView();
    }
}