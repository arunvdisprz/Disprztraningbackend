using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public class AppointmentsBL : IAppointmentsBL
    {
        public static List<AppointmentList> allAppointmentList = new List<AppointmentList>()
        {
            new AppointmentList()
            {
                name = "string",
                id = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                appointmentDate = new DateTime(2023, 1, 12),
                appointmentStartTime = new DateTime(2023, 1, 12, 23, 00, 00),
                appointmentEndTime = new DateTime(2023, 1, 12, 23, 59, 00),
                appointmentContent = "string",
                location = "string",
                description = "string",
                color = "string",
                appointmentStatus = true
            },
            new AppointmentList()
            {
                name = "string",
                id = "a06ac7bd-1b6c-4443-a499-deccd3f35661",
                appointmentDate = new DateTime(2023, 1, 12),
                appointmentStartTime = new DateTime(2023, 1, 12, 13, 00, 00),
                appointmentEndTime = new DateTime(2023, 1, 12, 13, 59, 00),
                appointmentContent = "string",
                location = "string",
                description = "string",
                color = "string",
                appointmentStatus = true
            }
        };

        public static List<AppointmentList> filteredAppointmentList = new List<AppointmentList> { };

        //Retrieves all appointments in the list asynchronously.
        //The task result contains a list of appointments sorted by appointment date.
        public List<AppointmentList> GetAllAppointmentInList()
        {
            allAppointmentList.Sort(
                (x, y) => x.appointmentStartTime.CompareTo(y.appointmentStartTime)
            );
            return allAppointmentList;
        }

        ///Retrieves appointments by date in the list asynchronously.
        //The task result contains a list of appointments filtered by the specified date and sorted by appointment start time.
        public List<AppointmentList> GetAppointmentByDateInList(DateTime date)
        {
            filteredAppointmentList = allAppointmentList
                .FindAll(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", date)
                )
                .OrderBy(appointment => appointment.appointmentStartTime)
                .ToList();
            return filteredAppointmentList;
        }

        // Adds an appointment to the list.
        // A Boolean value indicating whether the appointment was successfully added (True) or
        //If the appointment conflicts with an existing appointment (False).
        public Boolean AddAppointmentInList(AppointmentList addAppointmentValue)
        {
            filteredAppointmentList = allAppointmentList
                .FindAll(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", addAppointmentValue.appointmentDate)
                )
                .ToList();

            var checkList = filteredAppointmentList.Any(
                appointment =>
                    CheckCondition(
                        addAppointmentValue.appointmentStartTime,
                        addAppointmentValue.appointmentEndTime,
                        appointment.appointmentStartTime,
                        appointment.appointmentEndTime
                    )
            );

            if (!checkList)
            {
                allAppointmentList.Add(addAppointmentValue);
                return (true);
            }
            else
                return (false);
        }

        //The CheckCondition method is a function that takes four DateTime parameters: newStartTime, newEndTime, StartTime, and EndTime.
        //These parameters represent the start and end times of two appointments that the method will compare to determine if they are overlapping.
        //The method first calculates the middle time of the new appointment by adding the difference between the end
        //And start minutes of the new appointment, divided by 2, to the start time of the new appointment.
        // If any of these conditions are true, the method will return true, indicating that the appointments are overlapping.
        // If none of these conditions are true, the method will return false, indicating that the appointments are not overlapping.
        public Boolean CheckCondition(
            DateTime newStartTime,
            DateTime newEndTime,
            DateTime startTime,
            DateTime endTime
        )
        {
            return (newStartTime < endTime && newEndTime > startTime);
        }

        // Updates an appointment in the list.
        //A Boolean value indicating whether the appointment was successfully updated (True) or
        //If the updated appointment conflicts with an existing appointment (False).
        public Boolean patchAppointmentsInList(PatchAppointmentList patchAppointmentValue)
        {
            filteredAppointmentList = allAppointmentList
                .FindAll(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", patchAppointmentValue.patchAppointmentDate)
                )
                .ToList();

            var checkList = filteredAppointmentList.Any(
                appointment =>
                    appointment.id != patchAppointmentValue.patchId
                    && CheckCondition(
                        patchAppointmentValue.patchAppointmentStartTime,
                        patchAppointmentValue.patchAppointmentEndTime,
                        appointment.appointmentStartTime,
                        appointment.appointmentEndTime
                    )
            );

            var appointment = allAppointmentList.Find(x => x.id == patchAppointmentValue.patchId);

            if (!checkList && appointment != null)
            {
                appointment.appointmentStartTime = patchAppointmentValue.patchAppointmentStartTime;
                appointment.appointmentEndTime = patchAppointmentValue.patchAppointmentEndTime;
                appointment.appointmentContent = patchAppointmentValue.patchAppointmentContent;
                appointment.color = patchAppointmentValue.patchColor;
                appointment.appointmentStatus = patchAppointmentValue.patchAppointmentStatus;
                return true;
            }
            else
            {
                return false;
            }
        }

        //deleteId : A string value representing the ID of the appointment that needs to be deleted.deleteId .
        //Boolean - Returns true if the appointment was deleted successfully, false otherwise.
        public Boolean DeleteAppointmentById(string deleteId)
        {
            //It returns the index of the first element in the collection that matches the specified condition, or -1 if no element is found.
            var index = allAppointmentList.FindIndex(appointment => appointment.id == deleteId);

            if (index == -1)
                return false;
            else

                allAppointmentList.RemoveAt(index);
            return true;
        }
    }
}
