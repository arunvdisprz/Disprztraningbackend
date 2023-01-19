using DisprzTraining.Models;
using DisprzTraining.Business;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        /// Get all appointments
        /// </summary>

        /// <response code="200"> Returns a list of appointments that the user created</response>
        /// <response code="404"> Returns no appointment found</response>
     
        [HttpGet("/appointment")]
        public async Task<IActionResult> GetAllAppointmentAsync()
        {
            var result=await _AppointmentsBL.GetAllAppointmentInListAsync();
            return result.Any() ? Ok(result) : NotFound(new Exception("No appointment found."));
        }

        /// <summary>
        /// Get appointments gy date
        /// </summary>

        ///<remarks>
        /// Sample request:
        ///
        ///      date : 2023-01-08
        ///     
        /// </remarks>

        /// <response code="200"> Returns a list of appointments that the user created on a particular date</response>
        /// <response code="404"> Returns no appointment found on a particular date </response>

        [HttpGet("/appointment/{date}")]
        public async Task<IActionResult> GetAppointmenByDateAsync(DateTime date)
        {
             var result=await _AppointmentsBL.GetAppointmentByDateInListAsync(date);
            return result.Any() ? Ok(result) : NotFound(new Exception("No appointment found on a particular date."));
        }

        /// <summary>
        /// Post a new appointment
        /// </summary>
        /// <returns></returns>

        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "name": "string",
        ///       "id": "string",
        ///       "appointmentDate": "2023-01-19T07:23:12.763Z",
        ///       "appointmentStartTime": "2023-01-19T07:23:12.763Z",
        ///       "appointmentEndTime": "2023-01-19T09:23:12.763Z",
        ///       "appointmentContent": "string",
        ///       "location": "string",
        ///       "description": "string",
        ///       "color": "string",
        ///       "appointmentStatus": true
        ///      }
        ///
        /// </remarks>

        /// <response code="201">Returns appointment created successfully</response>
        /// <response code="400">Returns bad request because of appointment start time greater than or equal to end time</response>
        /// <response code="409">Returns conflict because of already have a appointment at this particular time</response>\

        [HttpPost("/appointment")]
        public IActionResult AddAppointment(AppointmentList addAppointmentValue)
        {
           if (addAppointmentValue.appointmentStartTime >= addAppointmentValue.appointmentEndTime){
              if (addAppointmentValue.appointmentStartTime == addAppointmentValue.appointmentEndTime){
                  return BadRequest(new Exception("Given the appointment start time and end time are equal"));
               }else{
                  return BadRequest(new Exception("Given appointment start time greater than appointment end time"));}
           }
           else{
              if (_AppointmentsBL.AddAppointmentInList(addAppointmentValue))
              {
                return Created("","Appointment created successfully");
               }
               else
               {
                return Conflict(new Exception("Already have a appointment at this particular time"));
               }
           }
            
        }

        /// <summary>
        /// update a exiting appointment 
        /// </summary>
        /// <returns></returns>

        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "name": "string",
        ///       "id": "string",
        ///       "appointmentDate": "2023-01-19T07:23:12.763Z",
        ///       "appointmentStartTime": "2023-01-19T07:23:12.763Z",
        ///       "appointmentEndTime": "2023-01-19T09:23:12.763Z",
        ///       "appointmentContent": "string",
        ///       "location": "string",
        ///       "description": "string",
        ///       "color": "string",
        ///       "appointmentStatus": true
        ///      }
        ///
        /// </remarks>

        /// <response code="201">Returns appointment updated successfully</response>
        /// <response code="400">Returns bad request because of appointment start time greater than or equal to end time</response>
        /// <response code="409">Returns conflict because of already have a appointment at this particular time</response>\
        
        [HttpPatch("/appointment")]
        public IActionResult PatchAppointment(PatchAppointmentList patchAppointmentValue)
        {
            if (patchAppointmentValue.patchAppointmentStartTime >= patchAppointmentValue.patchAppointmentEndTime){
               if (patchAppointmentValue.patchAppointmentStartTime == patchAppointmentValue.patchAppointmentEndTime){
                 return BadRequest(new Exception("Given the appointment start time and end time are equal"));
               }
               else{
                  return BadRequest(new Exception("Given the appointment start time greater than appointment end time"));}
           }
           else{
              if  (_AppointmentsBL.patchAppointmentsInList(patchAppointmentValue))
              {
                return Created("","Appointment updated successfully");
               }
               else
               {
                return Conflict(new Exception("Already have a appointment at this particular time"));
               }
           }
        }

        /// <summary>
        /// Deletes an existing appointment by id.
        /// </summary>

        /// <remarks>
        /// Sample request:
        ///
        ///        id: a06ac7bd-1b6c-4443-a499-deccd3f35660
        ///
        /// </remarks>

        /// <response code="200">Appointment in a list deleted successfully.</response>
        /// <response code="404">Given appointment id is not found in the list.</response>

        [HttpDelete("/appointment/{deleteId}")]
        public IActionResult DeleteAppointment(string deleteId)
        {
            if (_AppointmentsBL.DeleteAppointmentById(deleteId))
            {
                return Ok();
            }
            else
            {
                return NotFound(new Exception("Given appointment id is not found in the list"));
            }
        }
    }
}
