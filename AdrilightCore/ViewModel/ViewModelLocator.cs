/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:adrilight"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Ninject;
using System;

namespace adrilight.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    internal class ViewModelLocator
    {
        private readonly IKernel kernel;

        public ViewModelLocator()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
                throw new InvalidOperationException("the parameter-less constructor of ViewModelLocator is expected to only ever be called in design time!");

            this.kernel = App.SetupDependencyInjection(true);

        }
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator(IKernel kernel)
        {
            this.kernel = kernel ?? throw new System.ArgumentNullException(nameof(kernel));
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return kernel.Get<SettingsViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}