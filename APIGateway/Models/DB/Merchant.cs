using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIGateway.Models.DB
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        [JsonIgnore]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
