using DisprzTraining.Models;
using DisprzTraining.Business;
using Microsoft.AspNetCore.Mvc;

namespace DisprzTraining.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentlistController : Controller
    {
        private readonly IAppointmentsBL _AppointmentsBL;

        public AppointmentlistController(IAppointmentsBL AppointmentsBL)
        {
            _AppointmentsBL = AppointmentsBL;
        }

        /// <summary>
        /// Retrieves all appointments from the database and returns them as a list.
        /// </summary>
        /// <returns>A list of all appointments in the database.</returns>
        /// <response code="200"> Returns a list of appointments that the user created</response>
        /// <example>
        /// GET: /appointment
        /// </example>
        [HttpGet("/appointment")]
        public IActionResult GetAllAppointment()
        {
            var appointments = _AppointmentsBL.GetAllAppointmentInList();
            return Ok(appointments);
        }

        /// <summary>
        /// Retrieves all appointments scheduled for the specified date.
        /// </summary>
        /// <param name="date">The date for which to retrieve appointments (in the format "yyyy-MM-dd").</param>
        /// <returns>A list of appointments scheduled for the specified date, or a "404 Not Found" error if no appointments are found.</returns>
        /// <response code="200">Returns the list of appointments scheduled for the specified date.</response>
        /// <response code="404">If no appointments are found for the specified date.</response>
        /// <example>
        /// GET: /appointment/2022-01-01
        /// </example>
        [HttpGet("/appointment/{date}")]
        public IActionResult GetAppointmentsByDate(DateTime date)
        {
            var appointments = _AppointmentsBL.GetAppointmentByDateInList(date);
            if (appointments.Any())
                return Ok(appointments);
            else
                return NotFound("No appointments were found");
        }

        /// <summary>
        /// Adds a new appointment to the list.
        /// </summary>
        /// <param name="addAppointmentValue">The details of the appointment to add.</param>
        /// <returns>A status message indicating the outcome of the operation.</returns>
        /// <response code="201">The appointment was created successfully.</response>
        /// <response code="400">If the start time is equal to or greater than the end time of the appointment.</response>
        /// <response code="409">If there is already an appointment scheduled during the same time.</response>
        /// <response code="500">If there was an error adding the appointment.</response>
        /// <example>
        /// POST: /appointment
        /// Body:
        ///{
        ///  "name": "Arun",
        ///  "id": "a06ac7bd-1b6c-4443-a499-deccd3f35661",
        ///  "appointmentDate": "2023-01-19T07:23:12.763Z",
        ///  "appointmentStartTime": "2023-01-19T07:23:12.763Z",
        ///  "appointmentEndTime": "2023-01-19T09:23:12.763Z",
        ///  "appointmentContent": "Meet with mentor",
        ///  "location": "G meet",
        ///  "description": "Discuss about testcase ",
        ///  "color": "#006bff",
        ///  "appointmentStatus": true
        ///}
        /// </example>
        [HttpPost("/appointment")]
        public IActionResult AddAppointment(AppointmentList addAppointmentValue)
        {
            if (addAppointmentValue.appointmentStartTime >= addAppointmentValue.appointmentEndTime)
            {
                if (
                    addAppointmentValue.appointmentStartTime
                    == addAppointmentValue.appointmentEndTime
                )
                    return BadRequest("The appointment start and end times are the same");
                else
                    return BadRequest(
                        "Appointment end time must be greater than the start time"
                    );
            }
            else
            {
                if (_AppointmentsBL.AddAppointmentInList(addAppointmentValue))
                    return Created("", "The appointment has been successfully created");
                else
                    return Conflict("Already have an appointment at in-between time");
            }
        }

        /// <summary>
        /// Updates an existing appointment in the list.
        /// </summary>
        /// <param name="patchAppointmentValue">The details of the appointment to update.</param>
        /// <returns>A status message indicating the outcome of the operation.</returns>
        /// <response code="200">The appointment was updated successfully.</response>
        /// <response code="400">If the start time is equal to or greater than the end time of the appointment.</response>
        /// <response code="409">If there is already an appointment scheduled during the same time.</response>
        /// <example>
        /// PATCH: /appointment
        /// Body:
        /// {
        ///  "name": "Arun",
        ///  "id": "a06ac7bd-1b6c-4443-a499-deccd3f35661",
        ///  "appointmentDate": "2023-01-19T07:23:12.763Z",
        ///  "appointmentStartTime": "2023-01-19T07:23:12.763Z",
        ///  "appointmentEndTime": "2023-01-19T09:23:12.763Z",
        ///  "appointmentContent": "Meet with patrick",
        ///  "location": "G meet",
        ///  "description": "Discuss about testcase ",
        ///  "color": "#006bff",
        ///  "appointmentStatus": true
        ///}
        /// </example>
        [HttpPatch("/appointment")]
        public IActionResult PatchAppointment(PatchAppointmentList patchAppointmentValue)
        {
            if (
                patchAppointmentValue.patchAppointmentStartTime
                >= patchAppointmentValue.patchAppointmentEndTime
            )
            {
                if (
                    patchAppointmentValue.patchAppointmentStartTime
                    == patchAppointmentValue.patchAppointmentEndTime
                )
                    return BadRequest("The appointment start and end times are the same");
                else
                    return BadRequest(
                        "Appointment end time must be greater than the start time"
                    );
            }
            else
            {
                if (_AppointmentsBL.patchAppointmentsInList(patchAppointmentValue))
                    return Created("", "The appointment has been successfully updated");
                else
                    return Conflict("Already have an appointment at in-between time");
            }
        }

        /// <summary>
        /// Delete an appointment from the list
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment to delete</param>
        /// <returns>A status message indicating the outcome of the operation.</returns>
        /// <response code="200">Appointment in a list deleted successfully.</response>
        /// <response code="404">The appointment with the given ID could not be located in the list.</response>
        /// <example>
        /// DELETE: /appointment/a06ac7bd-1b6c-4443-a499-deccd3f35661
        /// </example>
        [HttpDelete("/appointment/{appointmentId}")]
        public IActionResult DeleteAppointment(string appointmentId)
        {
            if (_AppointmentsBL.DeleteAppointmentById(appointmentId))
                return Ok("The appointment has been successfully removed");
            else
                return NotFound("The appointment with the given ID could not be located in the list");
        }
    }
}
