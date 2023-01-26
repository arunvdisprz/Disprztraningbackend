using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;

namespace DisprzTraining.Tests
{
    public class AppointmentlistControllertest
    {
        /// <summary>
        /// A method called "GetAllAppointment_Returns_200_Success" that tests the "GetAllAppointment" method in the "AppointmentlistController" class to ensure it returns a 200 OK status and a list of "AppointmentList" objects.
        /// </summary>
        [Fact]
        public void GetAllAppointment_ReturnsAppointmentList()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.GetAllAppointment() as OkObjectResult;
            // Assert
            Assert.Equal(result?.StatusCode, 200);
            Assert.IsType<List<AppointmentList>>(result?.Value);
            Assert.NotNull(result);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 200, indicating success, and that the returned value is of the correct type and not null.
        /// </summary>
        [Fact]
        public void GetAppointmenByDate_Appointmentdate_ReturnsAppointmentList()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result =
                Appointments.GetAppointmentsByDate(new DateTime(2023, 1, 12, 6, 33, 19))
                as OkObjectResult;
            // Assert
            Assert.Equal(result?.StatusCode, 200);
            Assert.IsType<List<AppointmentList>>(result?.Value);
            Assert.NotNull(result);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 404, indicating that the requested resource could not be found,
        /// And that the returned value is of the correct type and has the correct message.
        /// </summary>
        [Fact]
        public void GetAppointmenByDate_Appointmentdate_ReturnsNotFound()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.GetAppointmentsByDate(new DateTime(2022, 1, 13, 6, 33, 19));

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No appointments were found", notFoundObjectResult.Value);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 400 (BadRequest), indicating that the request was invalid,
        /// when the start time of the appointment is greater than the end time.It is also using an instance of "AppointmentsBL" (business layer) class to add the appointment.
        /// </summary>
        [Fact]
        public void AddAppointment_StartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new AppointmentList
            {
                appointmentDate = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentStartTime = new DateTime(2023, 12, 01, 12, 0, 0),
                appointmentEndTime = new DateTime(2023, 01, 12, 11, 0, 0)
            };

            // Act
            var result = Appointments.AddAppointment(appointment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(
                "Appointment end time must be greater than the start time",
                badRequestResult.Value
            );
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 400 (BadRequest), indicating that the request was invalid,
        /// when the start time of the appointment is equal to the end time.
        /// </summary>
        [Fact]
        public void AddAppointment_StartTimeEqualToEndTime_ReturnsBadRequest()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new AppointmentList
            {
                appointmentDate = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentStartTime = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentEndTime = new DateTime(2023, 01, 12, 12, 0, 0)
            };

            // Act
            var result = Appointments.AddAppointment(appointment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(
                "The appointment start and end times are the same",
                badRequestResult.Value
            );
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 201 (Created), indicating that a new resource has been successfully created,
        /// when the input is valid and appointment start time is less than end time.
        /// </summary>
        [Fact]
        public void AddAppointment_ValidInput_ReturnsCreated()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new AppointmentList
            {
                appointmentDate = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentStartTime = new DateTime(2023, 01, 12, 12, 0, 0),
                appointmentEndTime = new DateTime(2023, 01, 12, 13, 0, 0)
            };

            // Act
            var result = Appointments.AddAppointment(appointment);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal("The appointment has been successfully created", createdResult.Value);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 409 (Conflict),
        /// Indicating that the request could not be completed due to a conflict with the current state of the resource.
        /// Specifically,the test is checking the scenario when an appointment already exists at the same date and time.
        /// </summary>
        [Fact]
        public void AddAppointment_AppointmentAlreadyExist_ReturnsConflict()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new AppointmentList
            {
                appointmentDate = new DateTime(2023, 1, 12),
                appointmentStartTime = new DateTime(2023, 1, 12, 13, 00, 00),
                appointmentEndTime = new DateTime(2023, 1, 12, 13, 30, 00),
            };

            // Act
            var result = Appointments.AddAppointment(appointment);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Already have an appointment at in-between time", conflictResult.Value);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 400 (BadRequest), indicating that the request was invalid,
        /// when the start time of the appointment is greater than the end time.
        /// </summary>
        [Fact]
        public void PatchAppointment_StartTimeGreaterThanEndTime_ReturnsBadRequest()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 4, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 3, 33, 19),
                patchAppointmentContent = "string",
                patchColor = "#ff0055"
            };

            // Act
            var result = Appointments.PatchAppointment(appointment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(
                "Appointment end time must be greater than the start time",
                badRequestResult.Value
            );
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 400 (BadRequest),
        /// Indicating that the request was invalid, when the start time of the appointment is equal to the end time.
        /// </summary>
        [Fact]
        public void PatchAppointment_StartTimeEqualToEndTime_ReturnsBadRequest()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 4, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 4, 33, 19),
                patchAppointmentContent = "string",
                patchColor = "string"
            };

            // Act
            var result = Appointments.PatchAppointment(appointment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(
                "The appointment start and end times are the same",
                badRequestResult.Value
            );
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 201 (Created),
        /// Indicating that an existing resource has been successfully updated,
        /// when the input is valid and appointment start time is less than end time.
        /// </summary>
        [Fact]
        public void PatchAppointment_ValidInput_ReturnsCreated()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);
            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                patchAppointmentDate = new DateTime(2023, 01, 13, 12, 0, 0),
                patchAppointmentStartTime = new DateTime(2023, 01, 13, 12, 0, 0),
                patchAppointmentEndTime = new DateTime(2023, 01, 13, 13, 0, 0),
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            // Act
            var result = Appointments.PatchAppointment(appointment);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal("The appointment has been successfully updated", createdResult.Value);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 409 (Conflict),
        /// Indicating that the request could not be completed due to a conflict with the current state of the resource.
        /// Specifically, the test is checking the scenario when an appointment already exists at the same date and time.
        /// </summary>
        [Fact]
        public void PatchAppointment__AppointmentAlreadyExist_ReturnsConflict()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            var appointment = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "a06ac7bd-1b6c-4443-a499-deccd3f35660",
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 13, 00, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 13, 30, 19),
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            // Act
            var result = Appointments.PatchAppointment(appointment);
            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Already have an appointment at in-between time", conflictResult.Value);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 200 (Ok) indicating that the request was successful,
        /// when the appointment is deleted successfully.It is also using an instance of "AppointmentsBL" (business layer) class to delete the appointment.
        /// </summary>
        [Fact]
        public void DeleteAppointment_AppointmentId_ReturnsOk()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.DeleteAppointment("a06ac7bd-1b6c-4443-a499-deccd3f35660");
            // Assert
            var ObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("The appointment has been successfully removed", ObjectResult.Value);
        }

        /// <summary>
        /// The test is checking that the method returns a status code of 404 (Not Found) indicating that the requested resource could not be found,
        /// when the id passed to the method is not found in the list of appointments.
        /// </summary>
        [Fact]
        public void DeleteAppointment_AppointmentId_ReturnsNotFound()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.DeleteAppointment("string12");
            // Assert
            var NotFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(
                "The appointment with the given ID could not be located in the list",
                NotFoundObjectResult.Value
            );
        }
    }
}
