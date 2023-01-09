// using System.Net;
// using System.Text.Json;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Core;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;

// //using System.Runtime.Serialization.DataContract;

// namespace sscpback.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class AppointmentlistController : ControllerBase
// {
//     public static List<Appointmentlist> allAppointmentList = new List<Appointmentlist>()
//     {
//         // new Appointmentlist()
//         // {
//         //     name = null,
//         //     id = "f748a05c-6",
//         //     Appointmentdate = "21st Dec  2022",
//         //     Starttime = "1:42",
//         //     Endtime = "2:42",
//         //     Appointmentcontent = "meet with ravi1"
//         // },
//         // new Appointmentlist()
//         // {
//         //     name = null,
//         //     id = "f748a05c-7",
//         //     Appointmentdate = "21st Dec  2022",
//         //     Starttime = "4:42",
//         //     Endtime = "5:42",
//         //     Appointmentcontent = "meet with ravi2"
//         // },
//         // new Appointmentlist()
//         // {
//         //     name = null,
//         //     id = "f748a05c-8",
//         //     Appointmentdate = "21st Dec  2022",
//         //     Starttime = "7:42",
//         //     Endtime = "8:42",
//         //     Appointmentcontent = "meet with ravi3"
//         // }
//     };

//     public static List<Appointmentlist> filteredAppointmentList = new List<Appointmentlist> { };
//     public int count { get; set; }

//     [HttpGet()]
//     public IActionResult Getall()
//     {
//         return Ok(allAppointmentList);
//     }

//     [HttpGet("{GetId}")]
//     public IActionResult GetOne(string GetId)
//     {
//         filteredAppointmentList = allAppointmentList
//             .Where(x => x.Appointmentdate == GetId)
//             .ToList();
//         filteredAppointmentList.Sort((x, y) => x.Starttime.CompareTo(y.Starttime));
//         return Ok(filteredAppointmentList);
//     }

//     [HttpPost]
//     public IActionResult Addapp(Appointmentlist addAppointmentValue)
//     {
//         filteredAppointmentList = allAppointmentList
//             .Where(x => x.Appointmentdate == addAppointmentValue.Appointmentdate)
//             .ToList();

//         foreach (var num in filteredAppointmentList)
//         {
//             if (
//                 (
//                     (
//                         (listval.appstarttime >= num.appstarttime)
//                             && (listval.appstarttime <= num.appendtime)
//                         || (
//                             (listval.appendtime >= num.appstarttime)
//                             && (listval.appendtime <= num.appendtime)
//                         )
//                     )
//                     || (
//                         (num.appstarttime >= addAppointmentValue.appstarttime)
//                             && (num.appstarttime <= addAppointmentValue.appendtime)
//                         || (
//                             (num.appendtime >= addAppointmentValue.appstarttime)
//                             && (num.appendtime <= addAppointmentValue.appendtime)
//                         )
//                     )
//                     || ((listval.appstarttime >= addAppointmentValue.appendtime))
//                 )
//             )
//             {
//                 count++;
//                 break;
//             }
//         }
//         if (count == 0)
//         {
//             allAppointmentList.Add(listval);
//             count = 0;
//             filteredAppointmentList = allAppointmentList
//                 .Where(x => x.Appointmentdate == addAppointmentValue.Appointmentdate)
//                 .ToList();
//             filteredAppointmentList.Sort((x, y) => x.Starttime.CompareTo(y.Starttime));
//             return Created("", filteredAppointmentList);
//         }
//         else
//         {
//             return Conflict();
//         }
//     }

//     [HttpPatch]
//     public IActionResult Patchapp(Patchappointmentlist patchAppointmentValue)
//     {
//         filteredAppointmentList = allAppointmentList
//             .Where(x => x.Appointmentdate == patchAppointmentValue.currentdate)
//             .ToList();

//         foreach (var num1 in filteredAppointmentList.ToList())
//         {
//             if (num1.id == patchAppointmentValue.id)
//             {
//                 filteredAppointmentList.Remove(num1);
//             }
//         }

//         foreach (var num in filteredAppointmentList)
//         {
//             if (
//                 (
//                     (
//                         (patchAppointmentValue.appstarttime >= num.appstarttime)
//                             && (patchAppointmentValue.appstarttime <= num.appendtime)
//                         || (
//                             (patchAppointmentValue.appendtime >= num.appstarttime)
//                             && (patchAppointmentValue.appendtime <= num.appendtime)
//                         )
//                     )
//                     || (
//                         (num.appstarttime >= patchAppointmentValue.appstarttime)
//                             && (num.appstarttime <= patchAppointmentValue.appendtime)
//                         || (
//                             (num.appendtime >= patchAppointmentValue.appstarttime)
//                             && (num.appendtime <= patchAppointmentValue.appendtime)
//                         )
//                     )
//                     || ((patchAppointmentValue.appstarttime >= patchAppointmentValue.appendtime))
//                 )
//             )
//             {
//                 count++;
//                 break;
//             }
//         }
//         filteredAppointmentList = allAppointmentList
//             .Where(x => x.Appointmentdate == patchAppointmentValue.currentdate)
//             .ToList();

//         if (count == 0)
//         {
//             foreach (var num1 in filteredAppointmentList)
//             {
//                 if (num1.id == patchAppointmentValue.id)
//                 {
//                     num1.appstarttime=patchAppointmentValue.appstarttime;
//                     num1.appendtime=patchAppointmentValue.appendtime;
//                     num1.Starttime = patchAppointmentValue.Starttime;
//                     num1.Endtime = patchAppointmentValue.Endtime;
//                     num1.Appointmentcontent = patchAppointmentValue.Appointmentcontent;
//                 }
//             }
//             foreach (var num in allAppointmentList)
//             {
//                 if (num.id == patchAppointmentValue.id)
//                 {
//                     num.appstarttime=patchAppointmentValue.appstarttime;
//                     num.appendtime=patchAppointmentValue.appendtime;
//                     num.Starttime = patchAppointmentValue.Starttime;
//                     num.Endtime = patchAppointmentValue.Endtime;
//                     num.Appointmentcontent = patchAppointmentValue.Appointmentcontent;
//                 }
//             }
//             filteredAppointmentList.Sort((x, y) => x.Starttime.CompareTo(y.Starttime));
//             return Created("", filteredAppointmentList);
//         }
//         else
//         {
//             return Conflict();
//         }
//     }

//     [HttpDelete("{deleteId}")]
//     public IActionResult Deleteapp(string deleteId)
//     {
//         foreach (var num in allAppointmentList.ToList())
//         {
//             if (num.id == deleteId)
//             {
//                 allAppointmentList.Remove(num);
//             }
//         }
//         foreach (var num1 in filteredAppointmentList.ToList())
//         {
//             if (num1.id == deleteId)
//             {
//                 filteredAppointmentList.Remove(num1);
//             }
//         }
//         filteredAppointmentList.Sort((x, y) => x.Starttime.CompareTo(y.Starttime));
//         return Ok(filteredAppointmentList);
//     }
// }
