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
        /// <returns></returns>
        [HttpGet("/appointment")]
        public async Task<IActionResult> GetAllAppointmentAsync()
        {
            return Ok(await _AppointmentsBL.GetAllAppointmentInListAsync());
        }

        /// <summary>
        /// Get appointment gy date
        /// </summary>
        /// <returns></returns>
        [HttpGet("/appointment/{date}")]
        public async Task<IActionResult> GetAppointmenByDateAsync(DateTime date)
        {
            return Ok(await _AppointmentsBL.GetAppointmentByDateInListAsync(date));
        }

        /// <summary>
        /// Post a new appointment
        /// </summary>
        /// <returns></returns>
        [HttpPost("/appointment")]
        public IActionResult AddAppointment(AppointmentList addAppointmentValue)
        {
            if (_AppointmentsBL.AddAppointmentInList(addAppointmentValue))
            {
                return Created("", "");
            }
            else
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Patch a exiting appointment 
        /// </summary>
        /// <returns></returns>
        [HttpPatch("/appointment")]
        public IActionResult PatchAppointment(PatchAppointmentList patchAppointmentValue)
        {
            if (_AppointmentsBL.patchAppointmentsInList(patchAppointmentValue))
            {
                 return Created("", "");
            }
            else
            {
                return Conflict();
            }
        }

         /// <summary>
        ///  Delete a exiting appointment 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/appointment/{deleteId}")]
        public IActionResult DeleteAppointment(string deleteId)
        {
            if (_AppointmentsBL.DeleteAppointmentById(deleteId))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
