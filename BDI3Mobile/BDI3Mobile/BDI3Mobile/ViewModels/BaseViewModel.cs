using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BDI3Mobile.ViewModels
{
    /// <summary>
    /// V2 of BaseViewModel
    /// </summary>
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		private bool isBusy;

		static BaseViewModel()
		{
		}
		private string _pageTitle;
		public string PageTitle
		{
			get => _pageTitle;
			set => SetAndRaisePropertyChanged(ref _pageTitle, value);
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsBusy
		{
			get => isBusy;
			set => SetAndRaisePropertyChanged(ref isBusy, value);
		}
		public virtual Task InitializeAsync() => Task.CompletedTask;

		public virtual Task UninitializeAsync() => Task.CompletedTask;

		protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


		protected void SetAndRaisePropertyChanged<TRef>(
		   ref TRef field, TRef value, [CallerMemberName] string propertyName = null)
		{
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetAndRaisePropertyChangedIfDifferentValues<TRef>(
			ref TRef field, TRef value, [CallerMemberName] string propertyName = null)
			where TRef : class
		{
			if (field == null || !field.Equals(value))
			{
				SetAndRaisePropertyChanged(ref field, value, propertyName);
			}
		}

	}
}

