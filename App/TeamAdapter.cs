using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace App
{
    class TeamAdapter : BaseAdapter<Team>
    {
        private Activity context;
   
        private List<Team> mteams;
        public TeamAdapter(Activity context, List<Team> team) : base() {
            this.context = context;
            
            mteams = team;
            
        }
        
     
        public override int Count
        {
            get { return mteams.Count; }
        }

        public override Team this[int position]
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
            view.FindViewById<TextView>(Resource.Id.Text1).Text = "Title:" + mteams[position].Title;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Creator:" + mteams[position].Creator;


            return view;

        }

      

    }
}