using Moviit.Infrastructure.CrossCutting.Utils.Configuration;
using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using Moviit.Infrastructure.CrossCutting.Utils.Extensions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Text;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Moviit.Infrastructure.CrossCutting.Utils.Tracer
{

    /// <summary>
    /// Tracer used across the entire solution.
    /// The configuration from the system.diagnostics section (sources, listeners and switches) defines the behavior of this class.
    /// </summary>
    public class Tracer : IDisposable
    {

        #region Variables

        /// <summary>
        /// Constant with the default value to use for the names separator in the trace file. Is only used if the config file for tracing doesn't defines one.
        /// </summary>
        private static string DEFAULT_NAME_VALUE_SEPARATOR = ": ";
        /// <summary>
        /// Constant with the default value to use for the parameters separator in the trace file. Is only used if the config file for tracing doesn't defines one.
        /// </summary>
        private static string DEFAULT_PARAMETER_SEPARATOR = "; ";
        /// <summary>
        /// Variable to hold the value to use for the names separator in the trace file. Is filled from the config file.
        /// </summary>
        private static readonly string NAME_VALUE_SEPARATOR;
        /// <summary>
        /// Variable to hold the value to use for the parameters separator in the trace file. Is filled from the config file.
        /// </summary>
        private static readonly string PARAMETER_SEPARATOR;
        /// <summary>
        /// Variable to hold the value of the source name. Is filled from the config file.
        /// </summary>
        private static readonly string SOURCE_NAME;
        /// <summary>
        /// Variable to hold the value of the source switch. Is filled from the config file.
        /// </summary>
        private static readonly string SWITCH_NAME;
        /// <summary>
        /// Variable to hold the format for the error messages. Is filled from the config file.
        /// </summary>°
        private static readonly string ERROR_FORMAT;
        /// <summary>
        /// Variable to hold the format for the error title. Is filled from the config file.
        /// </summary>
        private static readonly string ERROR_TITLE_FORMAT;
        /// <summary>
        /// Variable to hold the format for the error detail when it doesn't has parameters. Is filled from the config file.
        /// </summary>
        private static readonly string ERROR_DETAIL_NO_PARAMETERS_FORMAT;
        /// <summary>
        /// Variable to hold the format for the error detail when it has parameters. Is filled from the config file.
        /// </summary>
        private static readonly string ERROR_DETAIL_PARAMETERS_FORMAT;
        /// <summary>
        /// Variable to hold the date format to use in the error messages
        /// </summary>
        private static readonly string ERROR_DATE_FORMAT;
        /// <summary>
        /// Variable to hold the type name to get the exceptions messages from the resource file.
        /// If this value is not provided then the translation of errors based on resource files will not be used.
        /// </summary>
        private static readonly string ERROR_RESOURCE_FILE_TYPE_NAME;
        /// <summary>
        /// Variable used to load the configuration only ionce
        /// </summary>
        private static bool _configurationRead = false;
        /// <summary>
        /// Variable used to detect redundant invocations
        /// </summary>
        private bool _isDisposed;
        /// <summary>
        /// Variable used to lock access to code
        /// </summary>
        private object _locker = new object();
        private static object _statickLocker = new object();

        /// <summary>
        /// Main variables. Source and Switch to be used.
        /// </summary>
        private TraceSource _traceSource;
        private SourceSwitch _traceSwitch;

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        static Tracer()
        {
            //Read the configuration only once
            lock (_statickLocker)
            {
                if (!_configurationRead)
                {
                    //Get the configuration keys for tracing
                    SOURCE_NAME = ConfigUtilities.GetValue("SOURCE_NAME");
                    SWITCH_NAME = ConfigUtilities.GetValue("SWITCH_NAME");

                    //Throw an exception if the keys aren't defined
                    if (string.IsNullOrWhiteSpace(SOURCE_NAME) || string.IsNullOrEmpty(SWITCH_NAME))
                    {
                        throw new SystemConfigurationException("Configuration keys for tracing could not be found");
                    }

                    //Get the separator to use in the trace messages
                    NAME_VALUE_SEPARATOR = ConfigUtilities.GetValue("PARAMETER_NAME_VALUE_SEPARATOR", false, DEFAULT_NAME_VALUE_SEPARATOR);
                    PARAMETER_SEPARATOR = ConfigUtilities.GetValue("PARAMETER_SEPARATOR", false, DEFAULT_PARAMETER_SEPARATOR);

                    //Get the formats to use in the trace messages
                    ERROR_FORMAT = ConfigUtilities.GetValue("ERROR_FORMAT");
                    ERROR_TITLE_FORMAT = ConfigUtilities.GetValue("ERROR_TITLE_FORMAT");
                    ERROR_DETAIL_NO_PARAMETERS_FORMAT = ConfigUtilities.GetValue("ERROR_DETAIL_NO_PARAMETERS_FORMAT");
                    ERROR_DETAIL_PARAMETERS_FORMAT = ConfigUtilities.GetValue("ERROR_DETAIL_PARAMETERS_FORMAT");
                    ERROR_DATE_FORMAT = ConfigUtilities.GetValue("ERROR_DATE_FORMAT");

                    //Get the type name to get the exceptions messages from the resource file
                    ERROR_RESOURCE_FILE_TYPE_NAME = ConfigUtilities.GetValue("ERROR_RESOURCE_FILE_TYPE_NAME", false, "_");
                    if (ERROR_RESOURCE_FILE_TYPE_NAME == "_")
                    {
                        ERROR_RESOURCE_FILE_TYPE_NAME = null;
                    }

                    _configurationRead = true;
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tracer()
        {
            _traceSource = new TraceSource(SOURCE_NAME);
            _traceSwitch = new SourceSwitch(SWITCH_NAME);
            _traceSource.Switch = _traceSwitch;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Traces data.
        /// </summary>
        /// <param name="eventType">Type of event associated to the data being traced</param>
        /// <param name="data">Data to be traced</param>
        public void TraceData(TraceEventType eventType, object data)
        {
            //Control if the Tracer is being disposed
            CheckTracerIsBeingDisposed();

            _traceSource.TraceData(eventType, 0, data);
        }

        /// <summary>
        /// Traces data.
        /// </summary>
        /// <param name="eventType">Type of event associated to the data being traced</param>
        /// <param name="data">Array of data to be traced</param>
        public void TraceData(TraceEventType eventType, params object[] data)
        {
            //Control if the Tracer is being disposed
            CheckTracerIsBeingDisposed();

            _traceSource.TraceData(eventType, 0, data);
        }

        /// <summary>
        /// Traces an event.
        /// </summary>
        /// <param name="eventType">Type of the event being traced</param>
        /// <param name="eventMessage">Formatted message associated to the event being traced</param>
        /// <param name="args">Parameters to be included in the formatted message associated to the event being traced</param>
        public void TraceEvent(TraceEventType eventType, string eventMessage, params object[] args)
        {
            TraceEvent(eventType, string.Format(eventMessage, args));
        }

        /// <summary>
        /// Traces an event.
        /// </summary>
        /// <param name="eventType">Type of the event being traced</param>
        /// <param name="eventMessage">Message associated to the event being traced</param>
        public void TraceEvent(TraceEventType eventType, string eventMessage)
        {
            //Control if the Tracer is being disposed
            CheckTracerIsBeingDisposed();

            _traceSource.TraceEvent(eventType, 0, $"{DateTime.Now.ToString(ERROR_DATE_FORMAT)} {eventMessage}");
        }

        /// <summary>
        /// Traces information using a formatted string message with parameters.
        /// </summary>
        /// <param name="infoMessage">Formatted informational message to be traced</param>
        /// <param name="args">Parameters to be included in the formatted informational message</param>
        public void TraceInformation(string infoMessage, params object[] args)
        {
            TraceInformation(string.Format(infoMessage, args));
        }

        /// <summary>
        /// Traces information.
        /// </summary>
        /// <param name="infoMessage">Informational message to be traced</param>
        public void TraceInformation(string infoMessage)
        {
            //Control if the Tracer is being disposed
            CheckTracerIsBeingDisposed();

            _traceSource.TraceInformation($"{DateTime.Now.ToString(ERROR_DATE_FORMAT)} {infoMessage}");
        }

        /// <summary>
        /// Traces a warning using a formatted string message with parameters.
        /// </summary>
        /// <param name="warningMessage">Formatted warning message to be traced</param>
        /// <param name="args">Parameters to be included in the formatted warning message</param>
        public void TraceWarning(string warningMessage, params object[] args)
        {
            TraceWarning(string.Format(warningMessage, args));
        }

        /// <summary>
        /// Traces a warning.
        /// </summary>
        /// <param name="warningMessage">Warning message to be traced</param>
        public void TraceWarning(string warningMessage)
        {
            //Control if the Tracer is being disposed
            CheckTracerIsBeingDisposed();

            _traceSource.TraceEvent(TraceEventType.Warning, 0, $"{DateTime.Now.ToString(ERROR_DATE_FORMAT)} {warningMessage}");
        }

        /// <summary>
        /// Traces an error including information about the method being called and the exception thrown.
        /// </summary>
        /// <param name="invocation">Representation of the method being invoked</param>
        /// <param name="exception">The exception thrown</param>
        public void TraceError(IMethodInvocation invocation, Exception exception)
        {
            //If there is no exception there's nothing to trace
            if (exception == null)
            {
                return;
            }

            TraceError(invocation, ERROR_FORMAT, exception.Message, exception.GetInnerExceptionsDetail());
        }

        /// <summary>
        /// Traces an error including information about the method being called and its parameters.
        /// </summary>
        /// <param name="invocation">Representation of the method being invoked</param>
        /// <param name="errorMessage">Formatted error message to be traced</param>
        /// <param name="args">Parameters to be included in the formatted error message</param>
        public void TraceError(IMethodInvocation invocation, string errorMessage, params object[] args)
        {
            //If there's no invocation then just log the error
            if (invocation == null)
            {
                TraceError(errorMessage, args);
                return;
            }

            //Extract the class, method and arguments from the invocation
            string prefix = string.Format(ERROR_TITLE_FORMAT, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), invocation.Target.GetType().BaseType.Name, invocation.MethodBase.Name);
            ParameterInfo[] parameters = invocation.MethodBase.GetParameters();

            if (invocation.Inputs == null || invocation.Inputs.Count < 1 || parameters == null || parameters.Length < 1 || invocation.Inputs.Count != parameters.Length)
            {
                TraceError(ERROR_DETAIL_NO_PARAMETERS_FORMAT, prefix, string.Format(errorMessage, args));
            }
            else
            {
                string parametersDescription = GetParametersDescription(parameters, invocation.Inputs);

                TraceError(ERROR_DETAIL_PARAMETERS_FORMAT, prefix, parametersDescription, string.Format(errorMessage, args));
            }
        }

        /// <summary>
        /// Traces an error including information about the method being called and the exception thrown.
        /// </summary>
        /// <param name="exception">The exception thrown</param>
        public void TraceError(Exception exception)
        {
            //If there is no exception there's nothing to trace
            if (exception != null)
            {
                //Get the caller
                StackFrame stack = new StackFrame(1);

                //Translate the error if needed
                TranslateException(ref exception);

                //Trace the error
                TraceError(stack, ERROR_FORMAT, exception.Message, exception.GetInnerExceptionsDetail());
            }
        }

        /// <summary>
        /// Traces an error using a formatted string message with parameters.
        /// </summary>
        /// <param name="errorMessage">Formatted error message to be traced</param>
        /// <param name="args">Parameters to be included in the formatted error message</param>
        public void TraceError(string errorMessage, params object[] args)
        {
            TraceError(string.Format(errorMessage, args));
        }

        /// <summary>
        /// Traces an error.
        /// </summary>
        /// <param name="errorMessage">Error message to be traced</param>
        public void TraceError(string errorMessage)
        {
            //Control if the Tracer is being disposed
            CheckTracerIsBeingDisposed();

            _traceSource.TraceEvent(TraceEventType.Error, 0, $"{DateTime.Now.ToString(ERROR_DATE_FORMAT)} {errorMessage}");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Traces an error including information about the method being called and its parameters.
        /// </summary>
        /// <param name="stack">Representation of the caller</param>
        /// <param name="errorMessage">Formatted error message to be traced</param>
        /// <param name="args">Parameters to be included in the formatted error message</param>
        private void TraceError(StackFrame stack, string errorMessage, params object[] args)
        {
            //If there's no invocation then just log the error
            if (stack == null)
            {
                TraceError(errorMessage, args);
                return;
            }

            //Extract the class, method and arguments from the invocation
            string prefix = string.Format(ERROR_TITLE_FORMAT, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), stack.GetType().BaseType.Name, stack.GetMethod().Name);
            ParameterInfo[] parameters = stack.GetMethod().GetParameters();

            if (parameters == null || parameters.Length < 1)
            {
                TraceError(ERROR_DETAIL_NO_PARAMETERS_FORMAT, prefix, string.Format(errorMessage, args));
            }
            else
            {
                string parametersDescription = GetParametersDescription(parameters);

                TraceError(ERROR_DETAIL_PARAMETERS_FORMAT, prefix, parametersDescription, string.Format(errorMessage, args));
            }
        }

        /// <summary>
        /// Returns a textual representation of the parameters being passed to a method and their values.
        /// </summary>
        /// <param name="parameters">An array of parameter descriptors</param>
        /// <param name="inputs">Collection of input values</param>
        /// <returns>Textual representation of the parameters being passed to a method and their values</returns>
        private string GetParametersDescription(ParameterInfo[] parameters, IParameterCollection inputs)
        {
            string inputValue;
            StringBuilder sb = new StringBuilder();

            //Iterate through the parameter collection and add the parameter name and value (with the configured separators) to the return
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i] == null)
                    inputValue = "[null]";
                else
                    inputValue = inputs[i].ToJSON();

                sb.AppendFormat("{0}{1}{2}{3}", parameters[i].Name, NAME_VALUE_SEPARATOR, inputValue, PARAMETER_SEPARATOR);
            }

            return sb.ToString().TrimEnd(PARAMETER_SEPARATOR.ToCharArray());
        }

        /// <summary>
        /// Returns a textual representation of the parameters being passed to a method.
        /// </summary>
        /// <param name="parameters">An array of parameter descriptors</param>
        /// <returns>Textual representation of the parameters being passed to a method</returns>
        private string GetParametersDescription(ParameterInfo[] parameters)
        {
            StringBuilder sb = new StringBuilder();

            //Iterate through the parameter collection and add the parameter name (with the configured separators) to the return
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("{0}{1}", parameters[i].Name, PARAMETER_SEPARATOR);
            }

            return sb.ToString().TrimEnd(PARAMETER_SEPARATOR.ToCharArray());
        }

        /// <summary>
        /// Translates the exception message if is a number and in such case translate the message inside the exception.
        /// </summary>
        /// <param name="exception">Exception wuth the message to check if it needs translation, in such case the message will be translated</param>
        private void TranslateException(ref Exception exception)
        {
            int numericCode;

            //If the exception message is a number and the ERROR_RESOURCE_FILE_TYPE_NAME value is defined in the configuration
            if ((exception != null) &&
                (!string.IsNullOrWhiteSpace(exception.Message)) && (int.TryParse(exception.Message, out numericCode)) &&
                (!string.IsNullOrWhiteSpace(ERROR_RESOURCE_FILE_TYPE_NAME)))
            {
                //Get the error message
                Assembly asm = Assembly.GetEntryAssembly();
                ResourceManager rm = new ResourceManager(ERROR_RESOURCE_FILE_TYPE_NAME, asm);
                string newErrorMessage = rm.GetString($"Ex{exception.Message}");

                //Construct a new exception and assign it to the original exception
                if (!string.IsNullOrWhiteSpace(newErrorMessage))
                {
                    exception = new TranslatedException(newErrorMessage, exception);
                }
            }
        }

        /// <summary>
        /// Control if the Tracer is being disposed, if it is then throws an ObjectDisposedException.
        /// </summary>
        private void CheckTracerIsBeingDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Cannot reference the Tracer object since its resources have already been released");
            }
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Main method for the IDisposable interface
        /// </summary>
        /// <param name="disposing">Signals if this objects is already being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_locker)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                    {
                        //Free all the resources if this is the first call to this method and if this object is being disposed
                        if ((_traceSource != null))
                        {
                            _traceSource.Flush();
                            _traceSource.Close();
                        }
                    }
                }
                _isDisposed = true;
            }
        }

        /// <summary>
        /// Added code to implement the Disposable pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
