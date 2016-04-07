using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;


namespace Seasons
{
    [Activity(Label = "Seasons", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string season;
        ImageView _img;
        static DateTime _date = DateTime.Now;
        TextView _seasonDisplay;
        Button _selectDate;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main_View);

            _seasonDisplay = FindViewById<TextView>(Resource.Id.date);
            _selectDate = FindViewById<Button>(Resource.Id.test);
            _img = FindViewById<ImageView>(Resource.Id.image);

            DisplaySeason(_date);

            _selectDate.Click += DateSelect_OnClick;
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment dpf = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                DisplaySeason(time);
            });
            dpf.Show(FragmentManager, "Select_Date");
        }

        protected void DisplaySeason(DateTime d)
        {
            SetSeason(d);
            _seasonDisplay.Text = season;
            SetImageSeason();
            _selectDate.Text = d.ToShortDateString();
        }

        protected void SetImageSeason()
        {
            if (season.Equals("Spring"))
                _img.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.spring));
            else if (season.Equals("Summer"))
                _img.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.summer));
            else if (season.Equals("Autumn"))
                _img.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.autumn));
            else if (season.Equals("Winter"))
                _img.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.winter));
            else
                _img.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.no_image));
        }

        protected void SetSeason(DateTime d)
        {
            if (d.Year != 2016)
                season = "Non handled Date ";
            else
            if (DateTime.Compare(d, new DateTime(2016, 03, 20)) >= 0 && (DateTime.Compare(d, new DateTime(2016, 06, 21)) < 0))
                season = "Spring";
            else
                if (DateTime.Compare(d, new DateTime(2016, 06, 21)) >= 0 && (DateTime.Compare(d, new DateTime(2016, 09, 22)) < 0))
                season = "Summer";
            else
                if (DateTime.Compare(d, new DateTime(2016, 09, 22)) >= 0 && (DateTime.Compare(d, new DateTime(2016, 12, 22)) < 0))
                season = "Autumn";
            else
                season = "Winter";
        }

        public class DatePickerFragment : DialogFragment,
                               DatePickerDialog.IOnDateSetListener
        {

            Action<DateTime> _dateSelectedHandler = delegate { };

            public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
            {
                DatePickerFragment frag = new DatePickerFragment();
                frag._dateSelectedHandler = onDateSelected;
                return frag;
            }

            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                DateTime date = MainActivity._date;
                DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                               this,
                                                               date.Year,
                                                               date.Month,
                                                               date.Day);
                return dialog;
            }

            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
                _dateSelectedHandler(selectedDate);
                _date = new DateTime(year, monthOfYear, dayOfMonth);
            }
        }

    }

}

