using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// using Json.JsonProperty;

namespace DisprzTraining.Models
{
    public class AppointmentList
    {
        public string? name { get; set; }
        public string? id { get; set; }
        public DateTime appointmentDate { get; set; }
        public DateTime appointmentStartTime { get; set; }
        public DateTime appointmentEndTime { get; set; }
        public string startTime { get; set; } = string.Empty;
        public string endTime { get; set; } = string.Empty;
        public string? appointmentContent { get; set; }
        public string? location { get; set; }
        public string? description { get; set; }
        public string? color { get; set; }
    }
}
