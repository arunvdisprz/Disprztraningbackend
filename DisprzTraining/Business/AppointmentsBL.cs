using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public class AppointmentsBL : IAppointmentsBL
    {
        public  static List<AppointmentList> allAppointmentList = new List<AppointmentList>()
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
        public int count { get; set; }
        public DateTime middleTime { get; set; }

        //Retrieves all appointments in the list asynchronously.
        //The task result contains a list of appointments sorted by appointment date.
        public async Task<List<AppointmentList>> GetAllAppointmentInListAsync()
        {
            allAppointmentList.Sort((x, y) => x.appointmentStartTime.CompareTo(y.appointmentStartTime));
            return await Task.FromResult(allAppointmentList);
        }

        ///Retrieves appointments by date in the list asynchronously.
        //The task result contains a list of appointments filtered by the specified date and sorted by appointment start time.
        public async Task<List<AppointmentList>> GetAppointmentByDateInListAsync(DateTime date)
        {
            filteredAppointmentList = allAppointmentList
                .Where(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", date)
                )
                .OrderBy(appointment => appointment.appointmentStartTime)
                .ToList();
            return await Task.FromResult(filteredAppointmentList);
        }

        // Adds an appointment to the list.
        // A Boolean value indicating whether the appointment was successfully added (True) or if the appointment conflicts with an existing appointment (False).
        public Boolean AddAppointmentInList(AppointmentList addAppointmentValue)
        {
            filteredAppointmentList = allAppointmentList
                .Where(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", addAppointmentValue.appointmentDate)
                )
                .ToList();

            middleTime = addAppointmentValue.appointmentStartTime.AddMinutes(
                (
                    addAppointmentValue.appointmentEndTime.Minute
                    + 2
                    - addAppointmentValue.appointmentStartTime.Minute
                ) / 2
            );

            var checkList = filteredAppointmentList.Any(
                num =>
                    (
                        (middleTime > num.appointmentStartTime)
                            && (middleTime < num.appointmentEndTime)
                        || (
                            addAppointmentValue.appointmentStartTime > num.appointmentStartTime
                                && addAppointmentValue.appointmentStartTime < num.appointmentEndTime
                            || addAppointmentValue.appointmentEndTime > num.appointmentStartTime
                                && addAppointmentValue.appointmentEndTime < num.appointmentEndTime
                        )
                        || (
                            num.appointmentStartTime > addAppointmentValue.appointmentStartTime
                                && num.appointmentStartTime < addAppointmentValue.appointmentEndTime
                            || num.appointmentEndTime > addAppointmentValue.appointmentStartTime
                                && num.appointmentEndTime < addAppointmentValue.appointmentEndTime
                        )
                    )
            );
            if (!checkList)
            {
                allAppointmentList.Add(addAppointmentValue);
                return (true);
            }
            else
            {
                return (false);
            }
        }

        // Updates an appointment in the list.
        //A Boolean value indicating whether the appointment was successfully updated (True) or if the updated appointment conflicts with an existing appointment (False).
        public Boolean patchAppointmentsInList(PatchAppointmentList patchAppointmentValue)
        {
            filteredAppointmentList = allAppointmentList
                .Where(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", patchAppointmentValue.patchAppointmentDate)
                )
                .ToList();

            middleTime = patchAppointmentValue.patchAppointmentStartTime.AddMinutes(
                (
                    patchAppointmentValue.patchAppointmentEndTime.Minute
                    + 2
                    - patchAppointmentValue.patchAppointmentStartTime.Minute
                ) / 2
            );

            foreach (var num in filteredAppointmentList)
            {
                if (num.id == patchAppointmentValue.patchId)
                {
                    continue;
                }
                else
                {
                    if (
                        (middleTime > num.appointmentStartTime)
                            && (middleTime < num.appointmentEndTime)
                        || (
                            patchAppointmentValue.patchAppointmentStartTime
                                > num.appointmentStartTime
                                && patchAppointmentValue.patchAppointmentStartTime
                                    < num.appointmentEndTime
                            || patchAppointmentValue.patchAppointmentEndTime
                                > num.appointmentStartTime
                                && patchAppointmentValue.patchAppointmentEndTime
                                    < num.appointmentEndTime
                        )
                        || (
                            num.appointmentStartTime
                                > patchAppointmentValue.patchAppointmentStartTime
                                && num.appointmentStartTime
                                    < patchAppointmentValue.patchAppointmentEndTime
                            || num.appointmentEndTime
                                > patchAppointmentValue.patchAppointmentStartTime
                                && num.appointmentEndTime
                                    < patchAppointmentValue.patchAppointmentEndTime
                        )
                    )
                    {
                        count++;
                        break;
                    }
                }
            }
            if (count == 0)
            {
                //The FirstOrDefault is short-circuiting, meaning it will stop searching for an element as soon as it finds a match
                var num1 = allAppointmentList.FirstOrDefault(
                    x => x.id == patchAppointmentValue.patchId
                );
                if (num1 != null)
                {
                    num1.appointmentStartTime = patchAppointmentValue.patchAppointmentStartTime;
                    num1.appointmentEndTime = patchAppointmentValue.patchAppointmentEndTime;
                    num1.appointmentContent = patchAppointmentValue.patchAppointmentContent;
                    num1.color = patchAppointmentValue.patchColor;
                    num1.appointmentStatus = patchAppointmentValue.patchAppointmentStatus;
                }
                count = 0;
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
            var index = allAppointmentList.FindIndex(x => x.id == deleteId);

            if (index == -1)
            {
                return false;
            }
            else
            {
                allAppointmentList.RemoveAt(index);
                return true;
            }
        }
    }
}
