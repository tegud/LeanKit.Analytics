using System.Net.Mail;

namespace LeanKit.Data
{
    public class TicketActivityAssignedUser
    {
        public static TicketActivityAssignedUser UnAssigned = new TicketActivityAssignedUser();

        public int Id { get; set; }

        public string Name { get; set; }

        public MailAddress Email { get; set; }
    }
}