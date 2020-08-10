using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace debug_failure.Areas.Identity.Data
{
    public class User: IdentityUser {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return string.Join(" ", new[]{FirstName, LastName}); }}

        public virtual ICollection<UserData> UserData { get; set; }
    }

    public class UserData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Data { get; set; }

        public virtual User Owner { get; set; }
    }
}
