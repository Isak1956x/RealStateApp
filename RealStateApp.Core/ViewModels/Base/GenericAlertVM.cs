namespace RealStateApp.Core.Application.ViewModels.Base
{
    public class GenericAlertVM<T>
    {
        public string Title { get; set; }
        public T Value { get; set; }
        public string Message { get; set; }
        public string AlertType { get; set; }

        public string Controller { get; set; }
        public string ActionSource { get; set; } = "Index";
        public string ActionDestination { get; set; } = "Index";
    }
}
