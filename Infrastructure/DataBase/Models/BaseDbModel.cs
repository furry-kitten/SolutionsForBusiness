using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataBase.Models
{
    public abstract class BaseDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
