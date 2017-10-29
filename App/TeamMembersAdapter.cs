using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace App
{
    class TeamMembersAdapter : BaseAdapter<TeamMembers>
    {
        private Activity context;

        private List<TeamMembers> mteammembers;
        public TeamMembersAdapter(Activity context, List<TeamMembers> members) : base()
        {
            this.context = context;

            this.mteammembers = members;

        }


        public override int Count
        {
            get { return mteammembers.Count; }
        }

        public override TeamMembers this[int position]
        {
            get
            {
                return mteammembers[position];
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
                view = context.LayoutInflater.Inflate(Resource.Layout.myteamrow,parent, false);
               
            }
            byte[] Image = mteammembers[position].Pic;
            if (Image != null)
            {
                
                Bitmap bmp = BitmapFactory.DecodeByteArray(Image, 0, Image.Length);
                view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(bmp);
            }
           
            view.FindViewById<TextView>(Resource.Id.Text1).Text = mteammembers[position].FirstName+"  " + mteammembers[position].LastName;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "Identity:" + mteammembers[position].t_identity;
            view.FindViewById<TextView>(Resource.Id.Text3).Text = "Email:" + mteammembers[position].Email;
            view.FindViewById<TextView>(Resource.Id.Text4).Text = "Office Phone:" + mteammembers[position].OfficePhone;
            view.FindViewById<TextView>(Resource.Id.Text5).Text = "Mobile Phone:" + mteammembers[position].MobilePhone;
            view.FindViewById<TextView>(Resource.Id.Text6).Text = "Department:" + mteammembers[position].Department;
            return view;

        }



    }
}