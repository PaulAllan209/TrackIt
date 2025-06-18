using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Core.Services.Shipping.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when a shipment cannot be found.
    /// </summary>
    public sealed class ShipmentNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentNotFoundException" /> class.
        /// </summary>
        public ShipmentNotFoundException() : base("The specified shipment could not be found.")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentNotFoundException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ShipmentNotFoundException(string? message) : base(message!)
        {

        }
    }
}
