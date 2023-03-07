namespace DataAccess.Entities.EntityBuilder
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class WeeklyAverageUsDieselPriceEntityBuilder : IEntityTypeConfiguration<WeeklyAverageUsDieselPrice>
    {
        public void Configure(EntityTypeBuilder<WeeklyAverageUsDieselPrice> builder)
        {
            builder.HasAlternateKey(x => x.WeekOf);
        }
    }
}
