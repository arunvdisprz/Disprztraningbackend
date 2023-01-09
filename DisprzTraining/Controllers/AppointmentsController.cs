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

        [HttpGet("/appointment")]
        public async Task<IActionResult> GetAllAppointmentAsync()
        {
            return Ok(await _AppointmentsBL.GetAllAppointmentInListAsync());
        }

        [HttpGet("/appointment/{date}")]
        public async Task<IActionResult> GetAppointmenByDateAsync(DateTime date)
        {
            return Ok(await _AppointmentsBL.GetAppointmentByDateInListAsync(date));
        }

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
