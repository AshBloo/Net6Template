using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminLTE.Coravel
{

    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class TestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }
        public string Position { get; set; }
    }
}
