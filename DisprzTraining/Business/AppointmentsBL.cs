using DisprzTraining.Models;
using DisprzTraining.Controllers;

namespace DisprzTraining.Business
{
    public class AppointmentsBL : IAppointmentsBL
    {
        public static List<AppointmentList> allAppointmentList = new List<AppointmentList>()
        {
         new AppointmentList()
           {
             name = "string",
             id = "string123",
             appointmentDate = new DateTime(2023, 1, 8, 6, 33, 19),
             appointmentStartTime = new DateTime(2023, 1, 8, 4, 33, 19),
             appointmentEndTime = new DateTime(2023, 1, 8, 8, 33, 19),
             appointmentContent = "string",
             location = "string",
             description = "string",
             color = "string",
             appointmentStatus= true
            }

        };

        public static List<AppointmentList> filteredAppointmentList = new List<AppointmentList> { };
        public int count { get; set; }
        public DateTime middleTime { get; set; }

        public async Task<List<AppointmentList>> GetAllAppointmentInListAsync()
        {
            allAppointmentList.Sort(
                (x, y) => x.appointmentDate.CompareTo(y.appointmentDate));
            return (allAppointmentList);
        }

        public async Task<List<AppointmentList>> GetAppointmentByDateInListAsync(DateTime date)
        {
            filteredAppointmentList = allAppointmentList
                .Where(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", date)
                )
                .ToList();
            filteredAppointmentList.Sort(
                (x, y) => x.appointmentStartTime.CompareTo(y.appointmentStartTime)
            );
            return (filteredAppointmentList);
        }

        public Boolean AddAppointmentInList(AppointmentList addAppointmentValue)
        {
            middleTime = addAppointmentValue.appointmentStartTime.AddMinutes(
                (
                    addAppointmentValue.appointmentEndTime.Minute
                    + 2
                    - addAppointmentValue.appointmentStartTime.Minute
                ) / 2
            );

            filteredAppointmentList = allAppointmentList
                .Where(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", addAppointmentValue.appointmentDate)
                )
                .ToList();

            if (addAppointmentValue.appointmentStartTime >= addAppointmentValue.appointmentEndTime)
            {
                return false;
            }
            else
            {
                foreach (var num in filteredAppointmentList)
                {
                    if (
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
                    {
                        count++;
                        break;
                    }
                }
                if (count == 0)
                {
                    allAppointmentList.Add(addAppointmentValue);
                    count = 0;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
        }

        public Boolean patchAppointmentsInList(PatchAppointmentList patchAppointmentValue)
        {
            middleTime = patchAppointmentValue.patchAppointmentStartTime.AddMinutes(
                (
                    patchAppointmentValue.patchAppointmentEndTime.Minute
                    + 2
                    - patchAppointmentValue.patchAppointmentStartTime.Minute
                ) / 2
            );

            filteredAppointmentList = allAppointmentList
                .Where(
                    appointment =>
                        String.Format("{0:d}", appointment.appointmentDate)
                        == String.Format("{0:d}", patchAppointmentValue.patchAppointmentDate)
                )
                .ToList();

            var item = filteredAppointmentList.Find(x => x.id == patchAppointmentValue.patchId);
            filteredAppointmentList.Remove(item);

            if (
                patchAppointmentValue.patchAppointmentStartTime
                >= patchAppointmentValue.patchAppointmentEndTime
            )
            {
                return false;
            }
            else
            {
                foreach (var num in filteredAppointmentList)
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
                if (count == 0)
                {
                    var num1 = allAppointmentList.Find(
                        x => x.id == patchAppointmentValue.patchId
                    );
                    num1.appointmentStartTime = patchAppointmentValue.patchAppointmentStartTime;
                    num1.appointmentEndTime = patchAppointmentValue.patchAppointmentEndTime;
                    num1.appointmentContent = patchAppointmentValue.patchAppointmentContent;
                    num1.color = patchAppointmentValue.patchColor;
                    num1.appointmentStatus=patchAppointmentValue.patchAppointmentStatus;
                    count = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean DeleteAppointmentById(string deleteId)
        {
            var item = allAppointmentList.Find(x => x.id == deleteId);

            if (item == null)
            {
                return false;
            }
            else
            {
                allAppointmentList.Remove(item);
                return true;
            }
        }
    }
}
