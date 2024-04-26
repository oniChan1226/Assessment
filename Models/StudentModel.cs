namespace StudentPortal.API.Models
{
    public class StudentModel
    {
        public Guid RollNumber { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }
        //comma seperated string of address :)
        public string Address { get; set; }
    }
}
