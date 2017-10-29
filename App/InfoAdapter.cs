using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections;
using System.Collections.Generic;

namespace App
{
    public class InfoAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        public InfoAdapter(Activity context, string[] items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];

            return view;
        }
    }
}