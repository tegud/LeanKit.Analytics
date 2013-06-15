using System.Net.Mail;

namespace LeanKit.Data
{
    public class TicketAssignedUser
    {
        public static TicketAssignedUser UnAssigned = new TicketAssignedUser();

        public int Id { get; set; }

        public string Name { get; set; }

        public MailAddress Email { get; set; }
    }
}