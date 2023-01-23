using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DisprzTraining.Tests
{
    public class AppointmentBLTests
    {
        //The test is checking that the method returns a list of type "AppointmentList" when it is called.
        //This method is using the AppointmentsBL class to get all the appointments.
        [Fact]
        public async Task Test_GetAllAppointmentInListAsync()
        {
            var appointmentsBL = new AppointmentsBL();

            // Act
            var result = await appointmentsBL.GetAllAppointmentInListAsync();

            // Assert
            Assert.IsType<List<AppointmentList>>(result);
        }

        //The test is checking that the method returns the expected number of appointments on the given date.
        //The method takes a single parameter, a date, which is used to identify the appointments that need to be retrieved.
        [Fact]
        public async Task Test_GetAppointmentByDateInListAsync()
        {
            var appointmentsBL = new AppointmentsBL();

            // Act
            var result = await appointmentsBL.GetAppointmentByDateInListAsync(
                new DateTime(2023, 1, 12, 6, 33, 19)
            );

            // Assert
            Assert.Equal(2, result.Count);
        }

        //The test is checking that the method returns zero appointments if there is no appointment on the passed date.
        //The method takes a single parameter, a date, which is used to identify the appointments that need to be retrieved.
        [Fact]
        public async Task Test_GetAppointmentByDateInListAsync_zero()
        {
            var appointmentsBL = new AppointmentsBL();

            // Act
            var result = await appointmentsBL.GetAppointmentByDateInListAsync(
                new DateTime(2023, 1, 13, 6, 33, 19)
            );

            // Assert
            Assert.Equal(0, result.Count);
        }

        //The method takes a single parameter, an "appointment" object, which contains details about the appointment to be added.
        // The test is checking that the method returns "true" after adding the appointment to the list.
        [Fact]
        public void Test_AddAppointmentInList_true()
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

        //The test is checking that the method returns "false" after trying to add the appointment to the list.
        //This test is testing the scenario when the method is called with an appointment object that is already exist in the list, it should return false.
        [Fact]
        public void Test_AddAppointmentInList_false()
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

        //f the patchId does exist, the method updates the details of the appointment with the new values provided in the PatchAppointmentList object.
        // The method then returns true to indicate that the update was successful.
        [Fact]
        public void PatchAppointmentsInList_ValidInput_UpdatesAppointment()
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

        //This test case is useful for checking that the patchAppointmentsInList method is functioning correctly by correctly identifying invalid input
        //And returning False in those cases.
        [Fact]
        public void PatchAppointmentsInList_InValidInput_UpdatesAppointment()
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

        //This test that is focused on testing the functionality of the DeleteAppointmentById method in isolation.
        //The test creates an instance of the AppointmentsBL class, calls the DeleteAppointmentById method passing in a hardcoded appointment ID as an argument
        //And then asserts that the method returns true.
        [Fact]
        public void DeleteAppointmentById_ValidInput_RemovesAppointment()
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

        //This test case is checking that when an invalid appointment Id is passed to the DeleteAppointmentById method of the AppointmentsBL class, it should return false.
        // The test is checking whether the method is able to handle invalid inputs and return the appropriate response.
        [Fact]
        public void DeleteAppointmentById_InValidInput_RemovesAppointment()
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
