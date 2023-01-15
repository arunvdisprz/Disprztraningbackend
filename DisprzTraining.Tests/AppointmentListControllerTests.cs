using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DisprzTraining.Tests
{
    public class AppointmentlistControllertest
    {

        //Get all appointmet
        [Fact]
        public async Task GetAllAppointmentAsync_Returns_200_Success()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = await Appointments.GetAllAppointmentAsync() as OkObjectResult;
            // Assert
            Assert.Equal(result?.StatusCode, 200);
        }

        //Get all appointmet
        [Fact]
        public async Task GetAllAppointmentAsync_Returns_null()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = await Appointments.GetAllAppointmentAsync() as CreatedResult;
            // Assert
            Assert.Equal(result?.StatusCode, null);
        }

        //Get appointment gy date
        [Fact]
        public async Task GetAppointmenByDateAsync_Returns_200_Success()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result =
                await Appointments.GetAppointmenByDateAsync(new DateTime(2023, 1, 8, 6, 33, 19))
                as OkObjectResult;
            // Assert
            Assert.Equal(result?.StatusCode, 200);
        }
        //Get all appointmet
        [Fact]
        public async Task GetAppointmenByDateAsync__Returns__null()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result =
                await Appointments.GetAppointmenByDateAsync(new DateTime(2023, 1, 8, 6, 33, 19))
                as CreatedResult;
            // Assert
            Assert.Equal(result?.StatusCode, null);
        }
        //ADD
        [Fact]
        public void AddAppointment_Returns_201()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            var value = new AppointmentList()
            {
                name = "string",
                id = "string123",
                appointmentDate = new DateTime(2023, 1, 8, 6, 33, 19),
                appointmentStartTime = new DateTime(2023, 1, 8, 4, 33, 19),
                appointmentEndTime = new DateTime(2023, 1, 8, 8, 33, 19),
                appointmentContent = "string",
                location = "string",
                description = "string",
                color = "string"
            };
            // Act
            var result = Appointments.AddAppointment(value) as CreatedResult;
            // Assert
            Assert.Equal(result?.StatusCode, 201);
        }

        [Fact]
        public void AddAppointment_Returns_409()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            var value = new AppointmentList()
            {
                name = "string",
                id = "string",
                appointmentDate = new DateTime(2023, 1, 8, 6, 33, 19),
                appointmentStartTime = new DateTime(2023, 1, 8, 4, 33, 19),
                appointmentEndTime = new DateTime(2023, 1, 8, 3, 33, 19),
                appointmentContent = "string",
                location = "string",
                description = "string",
                color = "string"
            };
            // Act
            var result = Appointments.AddAppointment(value) as ConflictResult;
            // Assert
            Assert.Equal(result?.StatusCode, 409);
        }

        //PATCH
        [Fact]
        public void PatchAppointment_Returns_201()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            var value = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "string123",
                patchAppointmentDate = new DateTime(2023, 1, 8, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 8, 4, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 8, 9, 33, 19),
                patchAppointmentContent = "string",  
                patchColor = "string"
            };
            // Act
            var result = Appointments.PatchAppointment(value) as CreatedResult;
            // Assert
            Assert.Equal(result?.StatusCode, 201);
        }

        [Fact]
        public void PatchAppointment_Returns_409()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            var value = new PatchAppointmentList()
            {
                patchName = "string",
                patchId = "string123",
                patchAppointmentDate = new DateTime(2023, 1, 8, 6, 33, 19),
                patchAppointmentStartTime = new DateTime(2023, 1, 8, 4, 33, 19),
                patchAppointmentEndTime = new DateTime(2023, 1, 8, 3, 33, 19),  
                patchAppointmentContent = "string",
                patchColor = "string"
            };
            // Act
            var result = Appointments.PatchAppointment(value) as ConflictResult;
            // Assert
            Assert.Equal(result?.StatusCode, 409);
        }

        //delete
        [Fact]
        public void DeleteAppointment_Returns_200()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.DeleteAppointment("string123") as OkResult;
            // Assert
            Assert.Equal(result?.StatusCode, 200);
        }

        [Fact]
        public void DeleteAppointment_Returns_404()
        {
            // Arrange
            IAppointmentsBL AppointmentsBL = new AppointmentsBL();
            AppointmentlistController Appointments = new(AppointmentsBL);

            // Act
            var result = Appointments.DeleteAppointment("string12") as NotFoundResult;
            // Assert
            Assert.Equal(result?.StatusCode, 404);
        }
    }
}
