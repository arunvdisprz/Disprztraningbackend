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
        //Get all appointmet
        //The test is checking that the method returns a status code of 200 (success) and that the returned value is a list of "AppointmentList" objects.
        //The test also checks that the returned value is not null.
        [Fact]
        public void GetAllAppointment_Returns_200_Success()
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

        //The test is checking that the method returns a status code of 200, indicating success, and that the returned value is of the correct type and not null.
        // The test is also using an instance of "AppointmentsBL" (business layer) class to get the list of appointments by date.
        //Get appointment gy date
        [Fact]
        public void GetAppointmenByDate_Returns_200_Success()
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

        //The test is checking that the method returns a status code of 404, indicating that the requested resource could not be found,
        //And that the returned value is of the correct type and has the correct message.
        //The test is also using an instance of "AppointmentsBL" (business layer) class to get the list of appointments by date.
        [Fact]
        public void GetAppointmenByDate_Returns_404_NotFound()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.GetAppointmentsByDate(new DateTime(2023, 1, 13, 6, 33, 19));

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No appointments found", notFoundObjectResult.Value);
        }

        //ADD
        //The test is checking that the method returns a status code of 400 (BadRequest), indicating that the request was invalid,
        //when the start time of the appointment is greater than the end time.It is also using an instance of "AppointmentsBL" (business layer) class to add the appointment.
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
                "Given appointment start time greater than appointment end time",
                badRequestResult.Value
            );
        }

        //The test is checking that the method returns a status code of 400 (BadRequest), indicating that the request was invalid,
        //when the start time of the appointment is equal to the end time.
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
                "Given the appointment start time and end time are equal",
                badRequestResult.Value
            );
        }

        //The test is checking that the method returns a status code of 201 (Created), indicating that a new resource has been successfully created,
        //when the input is valid and appointment start time is less than end time.
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
            Assert.Equal("Appointment created successfully", createdResult.Value);
        }

        //The test is checking that the method returns a status code of 409 (Conflict),
        //Indicating that the request could not be completed due to a conflict with the current state of the resource.
        // Specifically,the test is checking the scenario when an appointment already exists at the same date and time.
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
            Assert.Equal(
                "Already have a appointment at this particular time",
                conflictResult.Value
            );
        }

        //PATCH
        // The test is checking that the method returns a status code of 400 (BadRequest), indicating that the request was invalid,
        //when the start time of the appointment is greater than the end time.
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
                patchColor = "string"
            };

            // Act
            var result = Appointments.PatchAppointment(appointment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(
                "Given the appointment start time greater than appointment end time",
                badRequestResult.Value
            );
        }

        //The test is checking that the method returns a status code of 400 (BadRequest),
        //Indicating that the request was invalid, when the start time of the appointment is equal to the end time.
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
                "Given the appointment start time and end time are equal",
                badRequestResult.Value
            );
        }

        //The test is checking that the method returns a status code of 201 (Created),
        //Indicating that an existing resource has been successfully updated,
        //when the input is valid and appointment start time is less than end time.
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
                patchAppointmentDate = new DateTime(2023, 1, 12, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 12, 4, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 12, 9, 33, 19),
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            // Act
            var result = Appointments.PatchAppointment(appointment);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal("Appointment updated successfully", createdResult.Value);
        }

        //The test is checking that the method returns a status code of 409 (Conflict),
        //Indicating that the request could not be completed due to a conflict with the current state of the resource.
        //Specifically, the test is checking the scenario when an appointment already exists at the same date and time.
        [Fact]
        public void PatchAppointment_Returns_409()
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
            Assert.Equal(
                "Already have a appointment at this particular time",
                conflictResult.Value
            );
        }

        //delete
        //The test is checking that the method returns a status code of 200 (Ok) indicating that the request was successful,
        //when the appointment is deleted successfully.It is also using an instance of "AppointmentsBL" (business layer) class to delete the appointment.
        [Fact]
        public void DeleteAppointment_Returns_200()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result =
                Appointments.DeleteAppointment("a06ac7bd-1b6c-4443-a499-deccd3f35660") as OkResult;
            // Assert
            Assert.Equal(result?.StatusCode, 200);
            Assert.IsType<OkResult>(result);
        }

        //The test is checking that the method returns a status code of 404 (Not Found) indicating that the requested resource could not be found,
        // when the id passed to the method is not found in the list of appointments.
        [Fact]
        public void DeleteAppointment_Returns_404()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.DeleteAppointment("string12");
            // Assert
            var NotFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(
                "Given appointment id is not found in the list",
                NotFoundObjectResult.Value
            );
        }
    }
}
