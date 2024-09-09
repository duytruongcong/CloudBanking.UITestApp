using Android.App;
using Android.Views;
using Android.Widget;
using CloudBanking.BaseControl;
using System;
using System.Collections.Generic;

namespace CloudBanking.UITestApp
{
    public class ScreenViewModel
    {
        public string Title { get; set; }

        public string RightIconResName { get; set; }

        public Action ItemAction { get; set; }

    }

    public class TestItemAdapter : BaseAdapter<ScreenViewModel>
    {
        IList<ScreenViewModel> _items;
        Activity _context;

        public Action<int> OnItemIconClicked { get; set; }

        public TestItemAdapter(Activity activity, IList<ScreenViewModel> items)
        {
            _items = items;
            _context = activity;
        }

        public override int Count => _items == null ? 0 : _items.Count;

        public override ScreenViewModel this[int position] => _items == null ? null : _items[position];


        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            var inflater = LayoutInflater.From(_context);

            ViewHolder viewHolder = null;

            if (view == null || !(view.Tag is ViewHolder))
            {

                view = inflater.Inflate(Resource.Layout.TestItemLayout, parent, false);

                viewHolder = new ViewHolder()
                {
                    TvTitle = view.FindViewById<TvLabel>(Resource.Id.tv_title),
                    IvIcon = view.FindViewById<CustomImageView>(Resource.Id.iv_icon),
                };

                view.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)view.Tag;
            }

            viewHolder.Position = position;

            viewHolder.TvTitle.Text = _items[position].Title;

            if (_items[position].RightIconResName != null)
            {
                viewHolder.IvIcon.Visibility = ViewStates.Visible;
                viewHolder.IvIcon.LoadImageFromSetupImageName(_items[position].RightIconResName);
            }
            else
            {
                viewHolder.IvIcon.Visibility = ViewStates.Gone;
            }

            viewHolder.TvTitle.SetFont(_context, UIConstants.STRING_FONT_BOLD);

            return view;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public int Position { get; set; }
            public TvLabel TvTitle { get; set; }
            public CustomImageView IvIcon { get; set; }
        }
    }
}
