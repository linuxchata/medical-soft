using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Common.Commands;
using Common.Enumeration;
using Contracts.ViewModel.Base;
using Models;

namespace Common.ViewModel
{
    /// <summary>
    /// Represents dialog base view model.
    /// </summary>
    /// <typeparam name="T">Common class that represents the model.</typeparam>
    public abstract class ViewModelDialogBase2<T> : ViewModelDialogBase<T>, IViewModelDialogBase2<T>
        where T : ModelBase<T>
    {
        /// <summary>
        /// Handle command.
        /// </summary>
        private ICommand handleCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelDialogBase2{T}"/> class.
        /// </summary>
        protected ViewModelDialogBase2()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelDialogBase2{T}"/> class.
        /// </summary>
        /// <param name="mode">The mode.</param>
        protected ViewModelDialogBase2(WorkModeType mode)
        {
            this.Mode = mode;
        }

        /// <summary>
        /// Gets handle command.
        /// </summary>
        public virtual ICommand HandleCommand
        {
            get
            {
                return this.handleCommand ?? (this.handleCommand = new CommonCommand(
                    param => Task.Factory.StartNewWithDefaultCulture(() => this.Handle(param)),
                    param => this.CanHandle));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handle command is enabled.
        /// </summary>
        protected virtual bool CanHandle
        {
            get
            {
                var isValid = false;

                if (this.Model != null)
                {
                    isValid = this.Model.IsValid;
                }

                isValid &= this.Status != LoadingStatus.Loading;

                return isValid;
            }
        }

        /// <summary>
        /// Gets or sets mode of the work (Add/Edit).
        /// </summary>
        protected WorkModeType Mode { get; set; }

        /// <summary>
        /// Load data.
        /// </summary>
        protected virtual void Load()
        {
        }

        /// <summary>
        /// Load data.
        /// </summary>
        /// <param name="model">T model.</param>
        protected virtual void Load(T model)
        {
        }

        /// <summary>
        /// Handle model.
        /// </summary>
        protected virtual void Handle()
        {
        }

        /// <summary>
        /// Handle model.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected virtual void Handle(object parameter)
        {
            if (parameter == null)
            {
                this.Handle();
            }
        }

        /// <summary>
        /// Close dialog.
        /// </summary>
        protected virtual void CloseDialog()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(this.OnRequestClose));
        }
    }
}
