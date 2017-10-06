using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Object = Java.Lang.Object;

namespace App
{
    public class VehiclesAdapter : BaseAdapter<Vehicles>, IFilterable
    {
        private List<Vehicles> _originalData;
        private List<Vehicles> _items;
        private readonly Activity _context;

        public VehiclesAdapter(Activity activity, IEnumerable<Vehicles> cars)
        {
            _items = cars.OrderBy(s => s.Id).ToList();
            _context = activity;

            Filter = new TakeItemFilter(this);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.itemrow, null);
            switch (_items[position].Kind.Trim())
            {
                case "Car":
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.car);
                    break;

                case "Truck":
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.tr);
                    break;
            }

                   
                    view.FindViewById<TextView>(Resource.Id.Text1).Text = "Kind:"+ _items[position].Kind.Trim();
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Item ID:" + _items[position].Id.Trim();

            return view;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Vehicles this[int position]
        {
            get { return _items[position]; }
        }

        public Filter Filter { get; private set; }

        public override void NotifyDataSetChanged()
        {
           
            base.NotifyDataSetChanged();
        }

        private class TakeItemFilter : Filter
        {
            private readonly VehiclesAdapter _adapter;
            public TakeItemFilter(VehiclesAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<Vehicles>();
                if (_adapter._originalData == null)
                    _adapter._originalData = _adapter._items;

                if (constraint == null) return returnObj;

                if (_adapter._originalData != null && _adapter._originalData.Any())
                {
                    // Compare constraint to all names lowercased. 
                    // It they are contained they are added to results.
                    results.AddRange(
                        _adapter._originalData.Where(
                            u => u.Id.ToLower().Contains(constraint.ToString())));
                }

               
                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter._items = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Vehicles>()).ToList();

                _adapter.NotifyDataSetChanged();

                
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}