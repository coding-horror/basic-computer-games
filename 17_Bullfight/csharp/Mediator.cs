using System.Diagnostics;

namespace Game
{
    /// <summary>
    /// Facilitates sending messages between the two game loops.
    /// </summary>
    /// <remarks>
    /// This class serves as a little piece of glue in between the main program
    /// loop and the bull fight coroutine.  When the main program calls one of
    /// its methods, the mediator creates the appropriate input data that the
    /// bull fight coroutine later retrieves with <see cref="GetInput{T}"/>.
    /// </remarks>
    public class Mediator
    {
        private object? m_input;

        public void Dodge(RiskLevel riskLevel) =>
            m_input = (Action.Dodge, riskLevel);

        public void Kill(RiskLevel riskLevel) =>
            m_input = (Action.Kill, riskLevel);

        public void Panic() =>
            m_input = (Action.Panic, default(RiskLevel));

        public void RunFromRing() =>
            m_input = true;

        public void ContinueFighting() =>
            m_input = false;

        /// <summary>
        /// Gets the next input from the user.
        /// </summary>
        /// <typeparam name="T">
        /// The type of input to receive.
        /// </typeparam>
        public T GetInput<T>()
        {
            Debug.Assert(m_input is not null, "No input received");
            Debug.Assert(m_input.GetType() == typeof(T), "Invalid input received");
            var result = (T)m_input;
            m_input = null;
            return result;
        }
    }
}
