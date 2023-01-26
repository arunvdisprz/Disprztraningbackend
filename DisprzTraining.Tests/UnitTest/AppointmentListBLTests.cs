using DisprzTraining.Business;
using DisprzTraining.Models;
using Xunit;

namespace DisprzTraining.Tests
{
    public class AppointmentBLTests
    {
        /// <summary>
        /// The test is checking that the method returns a list of type "AppointmentList" when it is called.
        /// This method is using the AppointmentsBL class to get all the appointments.
        /// </summary>
        [Fact]
        public void GetAllAppointmentInList_ReturnsAppointmentList()
        {
            var appointmentsBL = new AppointmentsBL();

            // Act
            var result = appointmentsBL.GetAllAppointmentInList();

            // Assert
            Assert.IsType<List<AppointmentList>>(result);
        }

        /// <summary>
        /// The test is checking that the method returns the expected number of appointments on the given date.
        /// The method takes a single parameter, a date, which is used to identify the appointments that need to be retrieved.
        /// </summary>
        [Fact]
        public void GetAppointmentByDateInList_AppointmentDate_ReturnsAppointmentList()
        {
            var appointmentsBL = new AppointmentsBL();

            // Act
            var result = appointmentsBL.GetAppointmentByDateInList(
                new DateTime(2023, 1, 25, 6, 33, 19)
            );

            // Assert
            Assert.Equal(1, result.Count);
            Assert.IsType<List<AppointmentList>>(result);
        }

        /// <summary>
        /// The test is checking that the method returns zero appointments if there is no appointment on the passed date.
        /// The method takes a single parameter, a date, which is used to identify the appointments that need to be retrieved.
        /// </summary>
        [Fact]
        public void GetAppointmentByDateInList_AppointmentDate_ReturnsEmptyList()
        {
            var appointmentsBL = new AppointmentsBL();

            // Act
            var result = appointmentsBL.GetAppointmentByDateInList(
                new DateTime(2023, 1, 13, 6, 33, 19)
            );

            // Assert
            Assert.Equal(0, result.Count);
        }

        /// <summary>
        /// The method takes a single parameter, an "appointment" object, which contains details about the appointment to be added.
        /// The test is checking that the method returns "true" after adding the appointment to the list.
        /// </summary>
        [Fact]
        public void AddAppointmentInList_ValidInput_ReturnsTrue()
        {
            // Arrange
            var appointment = new AppointmentList
            {
                appointmentDate = new DateTime(2023, 01, 14, 12, 0, 0),
                appointmentStartTime = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentEndTime = new DateTime(2023, 01, 12, 13, 0, 0)
            };
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.AddAppointmentInList(appointment);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// The test is checking that the method returns "false" after trying to add the appointment to the list.
        /// This test is testing the scenario when the method is called with an appointment object that is already exist in the list, it should return false.
        /// </summary>
        [Fact]
        public void AddAppointmentInList_InValidInput_ReturnsFalse()
        {
            // Arrange
            var appointment = new AppointmentList
            {
                appointmentDate = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentStartTime = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentEndTime = new DateTime(2023, 01, 12, 14, 0, 0)
            };
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.AddAppointmentInList(appointment);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The new appointment starts at the current time and ends at 2 hours later, and the existing appointment starts 30 minutes later and ends 3 hours later.
        /// As the new appointment starts before the existing appointment ends, so it should return true, which is expected.
        /// </summary>
        [Fact]
        public void CheckCondition_OverlappingAppointments_ReturnsTrue()
        {
            // Arrange
            var newStartTime = DateTime.Now;
            var newEndTime = DateTime.Now.AddHours(2);
            var startTime = DateTime.Now.AddMinutes(30);
            var endTime = DateTime.Now.AddHours(3);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// In this test case, it is ensured that the newStartTime is set 3 hours after the current time, and the newEndTime is set 4 hours after the current time.
        /// Similarly, the startTime is set to the current time and the endTime is set 2 hours after the current time. This ensures that the newStartTime
        /// And newEndTime are not overlapping with the startTime and endTime.
        /// </summary>
        [Fact]
        public void CheckCondition_NonOverlappingAppointments_ReturnsFalse()
        {
            // Arrange
            var newStartTime = DateTime.Now.AddHours(3);
            var newEndTime = DateTime.Now.AddHours(4);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(2);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// In this test case, it is ensured that the newStartTime is set 3 hours after the current time, and the newEndTime is set 4 hours after the current time.
        /// Similarly, the startTime is set to the current time and the endTime is set 2 hours after the current time. This configuration ensures that the newStartTime
        /// And newEndTime are not overlapping with the startTime and endTime.
        /// </summary>
        [Fact]
        public void CheckCondition_NewStartTimeEqualToEndTime_ReturnsFalse()
        {
            // Arrange
            var newStartTime = DateTime.Now;
            var newEndTime = DateTime.Now.AddHours(-2);
            var startTime = DateTime.Now.AddHours(2);
            var endTime = DateTime.Now;
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The test arranges by setting the "newStartTime" to 2 hours before the current time, the "newEndTime" to the current time,
        /// the "startTime" to the current time and the "endTime" to 2 hours after the current time. The "CheckCondition" method is then called with these values as the input and the output is stored in the "result" variable.
        /// The test asserts that the "result" variable should be "False", indicating that the new start time and new end time are not overlapping with the start time and end time.
        /// </summary>
        [Fact]
        public void CheckCondition_NewEndTimeEqualToStartTime_ReturnsFalse()
        {
            // Arrange
            var newStartTime = DateTime.Now.AddHours(-2);
            var newEndTime = DateTime.Now;
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(2);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The test arranges by setting the "newStartTime" to 3 hours after the current time, the "newEndTime" to 4 hours after the current time,
        /// The "startTime" to the current time and the "endTime" to 2 hours after the current time. The "CheckCondition" method is then called with these values as the input and the output is stored in the "result" variable.
        /// The test asserts that the "result" variable should be "False", indicating that the new start time is greater than the end time and it is  a valid input.
        /// </summary>
        [Fact]
        public void CheckCondition_NewStartTimeGreaterThanEndTime_ReturnsFalse()
        {
            // Arrange
            var newStartTime = DateTime.Now.AddHours(3);
            var newEndTime = DateTime.Now.AddHours(4);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(2);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The test arranges by setting the "newStartTime" to the current time,
        /// The "newEndTime" to 1 hour after the current time, the "startTime" to 2 hours after the current time and the "endTime" to 3 hours after the current time.
        /// The "CheckCondition" method is then called with these values as the input and the output is stored in the "result" variable.
        /// The test asserts that the "result" variable should be "False", indicating that the new end time is less than the start time and it is a valid input.
        /// </summary>
        [Fact]
        public void CheckCondition_NewEndTimeLessThanStartTime_ReturnsFalse()
        {
            // Arrange
            var newStartTime = DateTime.Now;
            var newEndTime = DateTime.Now.AddHours(1);
            var startTime = DateTime.Now.AddHours(2);
            var endTime = DateTime.Now.AddHours(3);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The new end time set to 1 hour after the current time, the existing start time set to 2 hours after the current time,
        /// And the existing end time set to 3 hours after the current time.
        /// This test is checking that the CheckCondition method is working as expected,
        /// when the new start and end times are not equal to the start and end times that are being passed as arguments, it should return false.
        /// </summary>
        [Fact]
        public void CheckCondition_NewStartAndEndTimeEqualToStartAndEndTime_ReturnsTrue()
        {
            // Arrange
            var newStartTime = DateTime.Now;
            var newEndTime = DateTime.Now.AddHours(2);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(2);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// The new end time set to 3 hours after the current time, the existing start time set to the current time,
        /// And the existing end time set to 2 hours after the current time.
        /// This test is checking that the CheckCondition method is working as expected,
        /// when the new start time is equal to the existing start time and the new end time is greater than the existing end time, it should return true.
        /// </summary>
        [Fact]
        public void CheckCondition_NewStartTimeEqualToStartTimeAndNewEndTimeGreaterThanEndTime_ReturnsTrue()
        {
            // Arrange
            var newStartTime = DateTime.Now;
            var newEndTime = DateTime.Now.AddHours(3);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(2);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );
            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// The new start time set to 30 minutes before the current time, the new end time set to 2 hours after the current time,
        /// The existing start time set to the current time, and the existing end time set to 2 hours after the current time.
        /// This test is checking that the CheckCondition method is working as expected,
        /// when the new start time is less than the existing start time and the new end time is equal to the existing end time, it should return true.
        /// </summary>
        [Fact]
        public void CheckCondition_NewStartTimeLessThanStartTimeAndNewEndTimeEqualToEndTime_ReturnsTrue()
        {
            // Arrange
            var newStartTime = DateTime.Now.AddMinutes(-30);
            var newEndTime = DateTime.Now.AddHours(2);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddHours(2);
            var appointmentsBL = new AppointmentsBL();
            // Act
            Boolean result = appointmentsBL.CheckCondition(
                newStartTime,
                newEndTime,
                startTime,
                endTime
            );

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// The method then returns true to indicate that the update was successful.
        /// </summary>
        [Fact]
        public void PatchAppointmentsInList_ValidInput_UpdatesReturnsTrue()
        {
            // Arrange
            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 3, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 4, 33, 19),
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.patchAppointmentsInList(appointment);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// This test case is useful for checking that the patchAppointmentsInList method is functioning correctly by correctly identifying invalid input
        /// And returning False in those cases.
        /// </summary>
        [Fact]
        public void PatchAppointmentsInList_InValidInput_ReturnsFalse()
        {
            // Arrange
            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 13, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 14, 33, 19),
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.patchAppointmentsInList(appointment);
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The test creates a new "PatchAppointmentList" object and assigns values to its properties, including a patchId with a specific value.
        /// The "patchAppointmentsInList" function is then called with the created object as a parameter, and the returned result is asserted to be false.
        /// This test is likely checking that the "patchAppointmentsInList" function returns false when it is passed an invalid id.
        /// </summary>
        [Fact]
        public void PatchAppointmentsInList_InValidId_ReturnsFalse()
        {
            // Arrange
            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499",
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 13, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 14, 33, 19),
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.patchAppointmentsInList(appointment);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// This test that is focused on testing the functionality of the DeleteAppointmentById method in isolation.
        /// The test creates an instance of the AppointmentsBL class, calls the DeleteAppointmentById method passing in a hardcoded appointment ID as an argument
        /// And then asserts that the method returns true.
        /// </summary>
        [Fact]
        public void DeleteAppointmentById_ValidInput_ReturnsTrue()
        {
            // Arrange
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.DeleteAppointmentById(
                "a06ac7bd-1b6c-4443-a499-deccd3f35660"
            );
            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// This test case is checking that when an invalid appointment Id is passed to the DeleteAppointmentById method of the AppointmentsBL class, it should return false.
        /// The test is checking whether the method is able to handle invalid inputs and return the appropriate response.
        /// </summary>
        [Fact]
        public void DeleteAppointmentById_InValidInput_ReturnsFalse()
        {
            // Arrange
            var appointmentsBL = new AppointmentsBL();
            // Act
            var result = appointmentsBL.DeleteAppointmentById(
                "a06ac7bd-1b6c-4443-a499-deccd3f35669"
            );
            // Assert
            Assert.False(result);
        }
    }
}
