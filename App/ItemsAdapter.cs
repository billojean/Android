using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace App
{
    class ItemsAdapter : BaseAdapter<Items>
    {
        private Activity context;

        private List<Items> mitems;

        public ItemsAdapter(Activity context, List<Items> items) : base()
        {
            this.context = context;

            mitems = items;

        }


        public override int Count
        {
            get { return mitems.Count; }
        }

        public override Items this[int position]
        {
            get
            {
                return mitems[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

       public void RemoveItem(int position)
        {

            mitems.RemoveAt(position);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            
            View view = convertView;
            
            if (view == null)
            {


                view = context.LayoutInflater.Inflate(Resource.Layout.itemrow,parent,false);
                
            }

            switch (mitems[position].item_kind.Trim())
            {
                case "Car":
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.car);
                    break;
                case "Truck":
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.tr);
                    break;
                case "Laptop":
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.laptop);
                    break;
                case "Spare Part":
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.sparepart);
                    break;
            }
            
            view.FindViewById<TextView>(Resource.Id.Text1).Text = "Kind:" + mitems[position].item_kind.Trim();
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Item ID:" + mitems[position].item_id.Trim();

            return view;
            
        }

      
    }
}