namespace Common
{
    /// <summary>
    /// Represent class that lets you override a named parameter passed to a constructor.
    /// </summary>
    public class ResolverParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolverParameter"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the constructor parameter.</param>
        /// <param name="parameterValue">Value to pass for the constructor.</param>
        public ResolverParameter(string parameterName, object parameterValue)
        {
            this.ParameterValue = parameterValue;
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Gets name of the constructor parameter.
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Gets value to pass for the constructor.
        /// </summary>
        public object ParameterValue { get; private set; }
    }
}