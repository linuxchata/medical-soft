using System;

namespace Contracts
{
    /// <summary>
    /// Represent an appointment interface.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Gets or sets identification.
        /// </summary>
        Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        DateTime EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets item 1 id.
        /// </summary>
        int Item1Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets item 2 id.
        /// </summary>
        int Item2Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets item 1.
        /// </summary>
        string Item1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets item 2.
        /// </summary>
        string Item2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        string Comment
        {
            get;
            set;
        }
    }
}
