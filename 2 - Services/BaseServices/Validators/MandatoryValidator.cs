using Moviit.Services.BaseServices.Faulting;
using Moviit.Services.BaseServices.Faulting.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Moviit.Services.BaseServices.Validators
{

    /// <summary>
    /// This validator takes arrays of mandatory field names to check if those parameters are not null nor empty in the method invocation.
    /// A succesful validation is the one with at least one array of mandatory field names which invocation parameters are not null nor empty.
    /// Sample: [MandatoryValidator(new string[] { "agreementId" }, new string[] { "agreementNumber", "backendId" })]
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MandatoryValidator : Attribute, IParameterInspector, IOperationBehavior
    {

        #region Variables

        /// <summary>
        /// Collection of arrays of mandatory field names. Will be loaded in the initialization.
        /// </summary>
        private readonly IList<string[]> _listOfMandatoryFields;

        /// <summary>
        /// Dictionary to hold the operation parameters name and position.
        /// </summary>
        private Dictionary<string, int> dicParameters;

        #endregion

        #region Constructor

        /// <summary>
        /// Validator constructor. It must receive one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields">Array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields)
            : this(mandatoryFields, null, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2)
            : this(mandatoryFields1, mandatoryFields2, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields3">Third array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2, string[] mandatoryFields3)
            : this(mandatoryFields1, mandatoryFields2, mandatoryFields3, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields3">Third array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields4">Forth array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2, string[] mandatoryFields3, string[] mandatoryFields4)
            : this(mandatoryFields1, mandatoryFields2, mandatoryFields3, mandatoryFields4, null, null, null, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields3">Third array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields4">Forth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields5">Fifth array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2, string[] mandatoryFields3, string[] mandatoryFields4, string[] mandatoryFields5)
            : this(mandatoryFields1, mandatoryFields2, mandatoryFields3, mandatoryFields4, mandatoryFields5, null, null, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields3">Third array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields4">Forth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields5">Fifth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields6">Sixth array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2, string[] mandatoryFields3, string[] mandatoryFields4, string[] mandatoryFields5, string[] mandatoryFields6)
            : this(mandatoryFields1, mandatoryFields2, mandatoryFields3, mandatoryFields4, mandatoryFields5, mandatoryFields6, null, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields3">Third array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields4">Forth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields5">Fifth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields6">Sixth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields7">Seventh array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2, string[] mandatoryFields3, string[] mandatoryFields4, string[] mandatoryFields5, string[] mandatoryFields6, string[] mandatoryFields7)
            : this(mandatoryFields1, mandatoryFields2, mandatoryFields3, mandatoryFields4, mandatoryFields5, mandatoryFields6, mandatoryFields7, null)
        {
        }

        /// <summary>
        /// Validator constructor. It must receive at least one array of mandatory field names.
        /// </summary>
        /// <param name="mandatoryFields1">First array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields2">Second array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields3">Third array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields4">Forth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields5">Fifth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields6">Sixth array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields7">Seventh array of mandatory field names to check for not null and not empty parameters</param>
        /// <param name="mandatoryFields8">Eighth array of mandatory field names to check for not null and not empty parameters</param>
        public MandatoryValidator(string[] mandatoryFields1, string[] mandatoryFields2, string[] mandatoryFields3, string[] mandatoryFields4, string[] mandatoryFields5, string[] mandatoryFields6, string[] mandatoryFields7, string[] mandatoryFields8)
        {
            //Check the configuration of the validator first
            if (((mandatoryFields1 == null) || (mandatoryFields1.Length == 0)) &&
                ((mandatoryFields2 == null) || (mandatoryFields2.Length == 0)) &&
                ((mandatoryFields3 == null) || (mandatoryFields3.Length == 0)) &&
                ((mandatoryFields4 == null) || (mandatoryFields4.Length == 0)) &&
                ((mandatoryFields5 == null) || (mandatoryFields5.Length == 0)) &&
                ((mandatoryFields6 == null) || (mandatoryFields6.Length == 0)) &&
                ((mandatoryFields7 == null) || (mandatoryFields7.Length == 0)) &&
                ((mandatoryFields8 == null) || (mandatoryFields8.Length == 0)))
            {
                //At least one array of mandatory field names needs to be defined
                throw Utils.CreateFault(FaultMessages.FaultMessage1001, 1001);
            }

            //Add the non empty arrays of mandatory field names to the collection
            _listOfMandatoryFields = new List<string[]>();
            if ((mandatoryFields1 != null) && (mandatoryFields1.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields1);
            if ((mandatoryFields2 != null) && (mandatoryFields2.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields2);
            if ((mandatoryFields3 != null) && (mandatoryFields3.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields3);
            if ((mandatoryFields4 != null) && (mandatoryFields4.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields4);
            if ((mandatoryFields5 != null) && (mandatoryFields5.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields5);
            if ((mandatoryFields6 != null) && (mandatoryFields6.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields6);
            if ((mandatoryFields7 != null) && (mandatoryFields7.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields7);
            if ((mandatoryFields8 != null) && (mandatoryFields8.Length > 0)) _listOfMandatoryFields.Add(mandatoryFields8);
        }

        #endregion

        #region Public Methods

        #region IParameterInspector Methods

        /// <summary>
        /// Called before client calls are sent and after service responses are returned
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="inputs">The objects passed to the method by the client</param>
        /// <returns>The correlation state that is returned as the correlationState parameter in IParameterInspector.AfterCall(String,Object[],Object,Object)</returns>
        public object BeforeCall(string operationName, object[] inputs)
        {
            if (inputs == null)
            {
                throw Utils.CreateFault(string.Format(FaultMessages.FaultMessage1002, operationName) + GetMandatoryFieldsListForErrorMessage(), 1002);
            }

            bool validationPass = false;

            //Iterate through all the arrays of mandatory fields
            for (int i = 0; i < _listOfMandatoryFields.Count(); i++)
            {
                bool nullFound = false;
                //Iterate through all the mandatory fields of the current array of mandatory fields
                for (int j = 0; j < _listOfMandatoryFields[i].Length; j++)
                {
                    //Get the position of the current field being evaluated
                    int position = dicParameters[_listOfMandatoryFields[i][j]];

                    if (inputs[position] != null)
                    {
                        switch (Type.GetTypeCode(inputs[position].GetType()))
                        {
                            case TypeCode.Decimal:
                                if ((decimal)inputs[position] <= 0) nullFound = true;
                                break;
                            case TypeCode.Int64:
                                if ((long)inputs[position] <= 0) nullFound = true;
                                break;
                            case TypeCode.Int32:
                                if ((int)inputs[position] <= 0) nullFound = true;
                                break;
                            case TypeCode.Int16:
                                if ((short)inputs[position] <= 0) nullFound = true;
                                break;
                            case TypeCode.String:
                                if (string.IsNullOrWhiteSpace((string)inputs[position])) nullFound = true;
                                break;
                            default:
                                if (inputs[position] == null) nullFound = true;
                                break;
                        }

                        if ((j == _listOfMandatoryFields[i].Length - 1) && (!nullFound))
                        {
                            validationPass = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                //Early exit
                if (validationPass)
                {
                    break;
                }
            }

            if (!validationPass)
            {
                throw Utils.CreateFault(string.Format(FaultMessages.FaultMessage1002, operationName) + GetMandatoryFieldsListForErrorMessage(), 1002);
            }

            //Returns null because the correlation state it is not intended to be used
            return null;
        }

        /// <summary>
        /// Called after client calls are returned and before service responses are sent
        /// </summary>
        /// <param name="operationName">The name of the invoked operation</param>
        /// <param name="outputs">Any output objects</param>
        /// <param name="returnValue">The return value of the operation</param>
        /// <param name="correlationState">Any correlation state returned from the IParameterInspector.BeforeCall(String, Object[]) method, or null</param>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            //Do nothing
        }

        #endregion

        #region IOperationBehavior Methods

        /// <summary>
        /// Implements a modification or extension of the client across an operation
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined</param>
        /// <param name="clientOperation">The run-time object that exposes customization properties for the operation described by operationDescription</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            //Do nothing
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation described by operationDescription</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            //Every field name must exists in the operation parameters
            ParameterInfo[] parameters;
            if (operationDescription.TaskMethod != null)
            {
                parameters = operationDescription.TaskMethod.GetParameters();
            }
            else
            {
                parameters = operationDescription.SyncMethod.GetParameters();
            }

            dicParameters = new Dictionary<string, int>();
            for (int i = 0; i < parameters.Length; i++)
            {
                dicParameters.Add(parameters[i].Name, parameters[i].Position);
            }

            //Iterate through all the arrays of mandatory field names
            for (int i = 0; i < _listOfMandatoryFields.Count(); i++)
            {
                //Iterate through all the mandatory field names of the current array of mandatory fields
                for (int j = 0; j < _listOfMandatoryFields[i].Length; j++)
                {
                    if (!dicParameters.ContainsKey(_listOfMandatoryFields[i][j]))
                    {
                        throw Utils.CreateFault(string.Format(FaultMessages.FaultMessage1003, _listOfMandatoryFields[i][j]), 1003);
                    }
                }
            }

            dispatchOperation.ParameterInspectors.Add(this);
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined</param>
        public void Validate(OperationDescription operationDescription)
        {
            //Do nothing
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined</param>
        /// <param name="bindingParameters">The collection of objects that binding elements require to support the behavior</param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            //Do nothing
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get the mandatory fields list in a readable string.
        /// </summary>
        /// <returns>The mandatory fields list in a readable string</returns>
        private string GetMandatoryFieldsListForErrorMessage()
        {
            StringBuilder sb = new StringBuilder(string.Format("{0}{1}{2}", Environment.NewLine, "Mandatory Fields:", Environment.NewLine));

            for (int i = 0; i < _listOfMandatoryFields.Count; i++)
            {
                sb.AppendLine(string.Format("[{0}]", string.Join(" - ", _listOfMandatoryFields[i])));
            }

            return sb.ToString();
        }

        #endregion

    }
}
