namespace WebApi8_AdmissionTest.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Status {  get; set; } = true;

        public int TotalPages {  get; set; } = 1 ;

        public int TotalRegisters{ get; set; } = 1;
    }
}
