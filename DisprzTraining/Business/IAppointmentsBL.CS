using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentsBL
    {
        Task<List<AppointmentList>> GetAllAppointmentInListAsync();
        Task<List<AppointmentList>>GetAppointmentByDateInListAsync(DateTime GetId);
        Boolean AddAppointmentInList(AppointmentList addAppointmentValue);
        Boolean patchAppointmentsInList(PatchAppointmentList patchAppointmentValue);
        Boolean  DeleteAppointmentById(string deleteId);
    }
}