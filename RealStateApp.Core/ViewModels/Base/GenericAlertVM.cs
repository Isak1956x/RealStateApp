namespace RealStateApp.Core.Application.ViewModels.Base
{
    public class GenericAlertVM<T>
    {
        public string Title { get; set; }
        public T Value { get; set; }
        public string Message { get; set; }
        public string AlertType { get; set; } // e.g., "success", "error", "warning", "info"
    }
}
