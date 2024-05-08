using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalDoListAPI.Models;

public class Task
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("AccountModel")]
    public int AccID { get; set; }

    public string Name { get; set; } = "default";

    public string Description { get; set; } = "default";

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime DeadlineDate { get; set; }

    public bool IsDone { get; set; }

}
