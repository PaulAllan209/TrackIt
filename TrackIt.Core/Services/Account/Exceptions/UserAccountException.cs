﻿namespace TrackIt.Core.Services.Account
{
    /// <summary>
    /// Represents errors that occur with user account related operations.
    /// </summary>
    public class UserAccountException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="UserAccountException" /> class.</summary>
        public UserAccountException() : base("A User Account Exception has occurred.")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserAccountException(string? message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountException" /> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> 
        /// in Visual Basic) if no inner exception is specified.
        /// </param>
        public UserAccountException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
