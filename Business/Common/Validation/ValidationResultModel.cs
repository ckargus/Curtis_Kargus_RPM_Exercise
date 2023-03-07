namespace Business.Common.Validation
{
    using System.Collections.Generic;

    public class ValidationResultModel<T>
    {
        public List<T> entities = new List<T>();

        public bool isValid { get; set; }
    }
}
