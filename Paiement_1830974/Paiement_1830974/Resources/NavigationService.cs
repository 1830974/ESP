using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Paiement_1830974.Resources
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : ObservableObject;
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private Frame _frame;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<T>() where T : ObservableObject
        {
            if (_frame == null)
            {
                throw new InvalidOperationException("Frame is not set. Call SetFrame before navigating.");
            }

            var viewModel = _serviceProvider.GetRequiredService<T>();
            var viewType = GetViewTypeForViewModel(typeof(T));
            var view = (Page)_serviceProvider.GetRequiredService(viewType);
            view.DataContext = viewModel;
            _frame.Navigate(view);
        }

        private Type GetViewTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.Name.Replace("VM", "");
            return Type.GetType($"Paiement_1830974.Views.{viewName}");
        }
    }
}
