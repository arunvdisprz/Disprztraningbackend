using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentsBL
    {
        List<AppointmentList> GetAllAppointmentInList();
        List<AppointmentList> GetAppointmentByDateInList(DateTime date);
        Boolean AddAppointmentInList(AppointmentList addAppointmentValue);
        Boolean CheckCondition(
            DateTime newStartTime,
            DateTime newEndTime,
            DateTime StartTime,
            DateTime EndTime
        );
        Boolean patchAppointmentsInList(PatchAppointmentList patchAppointmentValue);
        Boolean DeleteAppointmentById(string deleteId);
    }
}
