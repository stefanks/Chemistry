namespace Chemistry
{
    public class PeriodicTableValidationResult
    {
        public string Message { get; private set; }
        public bool ValidationPassed { get; private set; }

        public PeriodicTableValidationResult(bool validationPassed, string message)
        {
            this.ValidationPassed = validationPassed;
            this.Message = message;
        }
    }
}