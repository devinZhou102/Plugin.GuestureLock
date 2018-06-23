using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuetureLock.Sample
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
        DetailViewModel viewModel;
        public DetailPage()
        {
            InitializeComponent();

            viewModel = new DetailViewModel();
            BindingContext = viewModel;
            Appearing += DetailPage_Appearing;
            Disappearing += DetailPage_Disappearing;

		}

        private void DetailPage_Disappearing(object sender, EventArgs e)
        {
            LockView.CheckCompleteEvent -= LockView_CheckCompleteEvent;
        }

        private void LockView_CheckCompleteEvent(List<int> checkList)
        {
            var result = "";
            foreach(var item in checkList)
            {
                result += item + " ";
            }
            TextResult.Text = result;
        }

        private void DetailPage_Appearing(object sender, EventArgs e)
        {
            LockView.CheckCompleteEvent += LockView_CheckCompleteEvent;
        }

    }
}