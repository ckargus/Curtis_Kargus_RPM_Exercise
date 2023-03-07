namespace Business.Common.Validation
{
    public static class ValidationErrorCodes
    {
        public const string WeeklyAverageUsDieselPriceAveragePriceMustBeGreaterThanZero = "Weekly Average Us Diesel Price Must Be Greater Than Zero";
        public const string WeeklyAverageUsDieselPriceMustHaveDate = "Weekly Average Us Diesel Price Must Not Be Null or Empty";
        public const string WeeklyAverageUsDieselPriceAlreadyExists = "Weekly Average Us Diesel Price Already Exists";
        public const string WeeklyAverageUsDieselPriceModelNotNull = "Weekly Average Us Diesel Price Cannot Be Null";
        public const string WeeklyAverageUsDieselPriceModelListNotNullOrEmpty = "Weekly Average Us Diesel Price Model List Cannot Be Null Or Empty";
        public const string WeeklyAverageUsDieselPriceModelListContainsDuplicates = "Weekly Average Us Diesel Price Model List Cannot Contain Duplicates";
    }
}
