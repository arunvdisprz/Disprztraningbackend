using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public class AppointmentsBL : IAppointmentsBL
    {
        public static List<AppointmentList> allAppointmentList = new List<AppointmentList>()
        {
            new AppointmentList()
            {
                name = "Arun",
                id = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                appointmentDate = new DateTime(2023, 1, 12),
                appointmentStartTime = new DateTime(2023, 1, 12, 23, 00, 00),
                appointmentEndTime = new DateTime(2023, 1, 12, 23, 59, 00),
                appointmentContent = "Meet with mentor",
                location = "Office",
                description = "nil",
                color = "#ff0055",
                appointmentStatus = true
            },
            new AppointmentList()
            {
                name = "Arun",
                id = "a06ac7bd-1b6c-4443-a499-deccd3f35661",
                appointmentDate = new DateTime(2023, 1, 12),
                appointmentStartTime = new DateTime(2023, 1, 12, 13, 00, 00),
                appointmentEndTime = new DateTime(2023, 1, 12, 13, 59, 00),
                appointmentContent = "QA Session",
                location = "G meet",
                description = "nil",
                color = "#ff0055",
                appointmentStatus = true
            }
        };

        public static List<AppointmentList> filteredAppointmentList = new List<AppointmentList> { };

        /// <summary>
        /// A method called "GetAllAppointmentInList" that returns a sorted list of all appointments in the collection "allAppointmentList" by start time.
        /// </summary>
        /// <returns>List of AppointmentList: returns a sorted list of all appointments in the collection "allAppointmentList" by start time.</returns>
        public List<AppointmentList> GetAllAppointmentInList()
        {
            allAppointmentList.Sort(
                (x, y) => x.appointmentStartTime.CompareTo(y.appointmentStartTime)
            );
            return allAppointmentList;
        }

        /// <summary>
        /// A method called "GetAppointmentByDateInList" that takes a DateTime and returns a list of appointments from the collection "allAppointmentList" that have the same date as the input.
        /// </summary>
        /// <param name="date">The date to filter the appointments by.</param>
        /// <returns>List of AppointmentList: returns a list of appointments that have the same date as the input and are ordered by start time.</returns>
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

        /// <summary>
        /// A method called "AddAppointmentInList" that takes an object of type "AppointmentList" and adds an appointment to a collection "allAppointmentList" and also checks for time conflict.
        /// </summary>
        /// <param name="addAppointmentValue">An object that holds the values for the new appointment to be added to the collection.</param>
        /// <returns>Boolean: returns 'true' if an appointment is successfully added and 'false' if the addition is not successful due to a time conflict.</returns>
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

        /// <summary>
        /// A method called "CheckCondition" that takes four arguments and checks if there is a time conflict between the new start and end times and the existing start and end times.
        /// </summary>
        /// <param name="newStartTime">The new start time for the appointment.</param>
        /// <param name="newEndTime">The new end time for the appointment.</param>
        /// <param name="startTime">The existing start time for the appointment.</param>
        /// <param name="endTime">The existing end time for the appointment.</param>
        /// <returns>Boolean: returns 'true' if there is a time conflict and 'false' if there is no conflict.</returns>
        public Boolean CheckCondition(
            DateTime newStartTime,
            DateTime newEndTime,
            DateTime startTime,
            DateTime endTime
        )
        {
            return (newStartTime < endTime && newEndTime > startTime);
        }

        /// <summary>
        /// A method called "patchAppointmentsInList" that takes an object of type "PatchAppointmentList" and updates an appointment from a collection "allAppointmentList" based on its matching id and also check for the time conflict.
        /// </summary>
        /// <param name="patchAppointmentValue">An object that holds the new values for the appointment and the id of the appointment to be updated in the collection.</param>
        /// <returns>Boolean: returns 'true' if an appointment is successfully updated and 'false' if the update is not successful.</returns>
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
                appointment.appointmentDate = patchAppointmentValue.patchAppointmentDate;
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

        /// <summary>
        /// A method called "DeleteAppointmentById" that takes a string argument "appointmentId" and deletes an appointment from a collection "allAppointmentList" based on its matching id.
        /// </summary>
        /// <param name="appointmentId">The id of the appointment to be deleted from the collection.</param>
        /// <returns>Boolean: returns 'true' if an appointment is successfully deleted and 'false' if no matching appointment is found in the collection.</returns>
        public Boolean DeleteAppointmentById(string appointmentId)
        {
            //It returns the index of the first element in the collection that matches the specified condition, or -1 if no element is found.
            var index = allAppointmentList.FindIndex(
                appointment => appointment.id == appointmentId
            );

            if (index == -1)
                return false;
            else

                allAppointmentList.RemoveAt(index);
            return true;
        }
    }
}
