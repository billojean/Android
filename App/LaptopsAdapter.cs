using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Object = Java.Lang.Object;

namespace App
{
    public class LaptopsAdapter : BaseAdapter<Laptops>, IFilterable
    {
        private List<Laptops> _originalData;
        private List<Laptops> _items;
        private readonly Activity _context;

        public LaptopsAdapter(Activity activity, IEnumerable<Laptops> laptops)
        {
            _items = laptops.OrderBy(s => s.Id).ToList();
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

            view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.laptop);

            
            view.FindViewById<TextView>(Resource.Id.Text1).Text = "Kind:Laptop";
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Item ID:" + _items[position].Id.Trim();

            return view;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Laptops this[int position]
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
            private readonly LaptopsAdapter _adapter2;
            public TakeItemFilter(LaptopsAdapter adapter)
            {
                _adapter2 = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<Laptops>();
                if (_adapter2._originalData == null)
                    _adapter2._originalData = _adapter2._items;

                if (constraint == null) return returnObj;

                if (_adapter2._originalData != null && _adapter2._originalData.Any())
                {
                    // Compare constraint to all names lowercased. 
                    // It they are contained they are added to results.
                    results.AddRange(
                        _adapter2._originalData.Where(
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
                    _adapter2._items = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Laptops>()).ToList();

                _adapter2.NotifyDataSetChanged();

                
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}