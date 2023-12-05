namespace AptitudeTest.Core.ViewModels
{
    public class ValidateImportFileVM
    {

        public bool isValidate { get; set; } = true;
        public List<string>? validationMessage { get; set; }
    }
}
