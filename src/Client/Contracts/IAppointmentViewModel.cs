using Contracts.ViewModel;
using Models;

namespace Client.Contracts
{
    /// <summary>
    /// Represents view model interface for appointment.
    /// </summary>
    public interface IAppointmentViewModel : IAppointmentViewModelBase<AppointmentModel, ItemModel>
    {
    }
}