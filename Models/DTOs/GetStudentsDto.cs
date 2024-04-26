namespace StudentPortal.API.Models.DTOs
{
    public class GetStudentsDto
    {
        public Guid RollNumber { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }
        public string Address { get; set; }
    }
}
