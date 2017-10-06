using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App
{
    class TeamAdapter : BaseAdapter<team>
    {
        private Activity context;
   
        private List<team> mteams;
        public TeamAdapter(Activity context, List<team> team) : base() {
            this.context = context;
            
            mteams = team;
            
        }
        
     
        public override int Count
        {
            get { return mteams.Count; }
        }

        public override team this[int position]
        {
            get
            {
                return mteams[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.teamrow, parent, false);
            }
            view.FindViewById<TextView>(Resource.Id.Text1).Text = "Title:" + mteams[position].title;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Creator:" + mteams[position].creator;


            return view;

        }

      

    }
}