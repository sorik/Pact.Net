using System;

namespace Provider.Exceptions
{
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string message = "Member not found") : base(message)
        {
        }
    }
}