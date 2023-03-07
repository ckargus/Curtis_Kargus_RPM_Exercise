namespace DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class WeeklyAverageUsDieselPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName="Date")]
        public DateTime WeekOf { get; set; }

        [Required]
        public double AveragePrice { get; set; }
    }
}
