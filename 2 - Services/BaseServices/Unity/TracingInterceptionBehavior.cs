using Moviit.Infrastructure.CrossCutting.Utils.Configuration;
using Moviit.Infrastructure.CrossCutting.Utils.Tracer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Moviit.Services.BaseServices.Unity
{

    /// <summary>
    /// This class implements the IInterceptionBehavior interface (interception behaviors implement this interface and are called for each invocation of the pipelines that they're included in) to allow logging and tracing.
    /// </summary>
    public class TracingInterceptionBehavior : IInterceptionBehavior
    {

        #region Variables

        /// <summary>
        /// Variable to reflex the behavior set in the configuration
        /// </summary>
        private bool? _enabled = null;

        /// <summary>
        /// Dictionary to cache the delegates Task created through reflection in the GetWrapperCreator method
        /// </summary>
        private readonly ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>> wrapperCreators = new ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Task>>();

        #endregion

        #region IInterceptionBehavior Methods

        /// <summary>
        /// Implementation of the behavioral processing.
        /// It logs the entry to this method, call the next on the interceptors pipeline, logs the return of that invocation or the exception if an error occurs and finally return to the caller the result received.
        /// Also this method takes care of async operations if is needed.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <returns>Return value from the target</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            //Log the call before the invocation.
            Trace(string.Format("{0}. Invoking method {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), input.MethodBase), TraceEventType.Information);

            //Invoke the next behavior in the chain (if this is the last interceptor in the pipeline then it invokes the target method).
            IMethodReturn result = getNext()(input, getNext);

            //After invoking the method on the original target log the return or the exception if an error occurs

            //Check if this method returns a Task
            MethodInfo method = input.MethodBase as MethodInfo;
            if ((result.ReturnValue != null) && (method != null) && (typeof(Task).IsAssignableFrom(method.ReturnType)))
            {
                //If this method returns a Task then override the original return value and handle the async operation
                Task task = (Task)result.ReturnValue;
                return input.CreateMethodReturn(GetWrapperCreator(method.ReturnType)(task, input), result.Outputs);
            }
            else if (result.Exception != null)
            {
                //If this method has a single return and an exception is thrown then log the exception
                TraceError(input, result.Exception);
            }
            else
            {
                //If this method has a single return then just log it
                Trace(string.Format("{0}. Method {1} returned {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), input.MethodBase, result.ReturnValue), TraceEventType.Information);
            }

            //Return the result to the caller
            return result;
        }

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts
        /// </summary>
        /// <returns>The interfaces required by the behavior for the objects it intercepts</returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            //Return empty because the association is made on the Service Host Factory
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Returns a flag indicating if this behavior will actually do anything when invoked.
        /// </summary>
        /// <value>True if this behavior will execute, false otherwise</value>
        /// <returns>True if this behavior will execute, false otherwise</returns>
        public bool WillExecute
        {
            get
            {
                if (_enabled.HasValue)
                {
                    return _enabled.Value;
                }
                else
                {
                    string value = ConfigUtilities.GetValue("UNITY_INTERCEPTION_TRACING_ENABLED");
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        _enabled = Convert.ToBoolean(value);
                    }
                    else
                    {
                        _enabled = false;
                    }
                    return _enabled.Value;
                }
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Traces a message
        /// </summary>
        /// <param name="message">Message to be traced</param>
        /// <param name="eventType">Event type being traced</param>
        private void Trace(string message, TraceEventType eventType)
        {
            using (Tracer tracer = new Tracer())
            {
                tracer.TraceData(eventType, message);
            }
        }

        /// <summary>
        /// Traces an error
        /// </summary>
        /// <param name="invocation">IMethodInvocation to be traced</param>
        /// <param name="ex">Exception to be traced</param>
        private void TraceError(IMethodInvocation invocation, Exception ex)
        {
            using (Tracer tracer = new Tracer())
            {
                tracer.TraceError(invocation, ex);
            }
        }

        /// <summary>
        /// This method will create the delegates through reflection and cache them in a ConcurrentDictionary.
        /// These wrapper creator delegates are of type Func&lt;Task, IMethodInvocation, Task&gt; to get the original task and the IMethodInvocation object representing the call of the invocation to the asynchronous method and return a wrapper Task.
        /// </summary>
        /// <param name="taskType">Task to be wrapped</param>
        /// <returns>The concurrent dictionary with the addition of the new delegate of type Func&lt;Task, IMethodInvocation, Task&gt; to be called</returns>
        private Func<Task, IMethodInvocation, Task> GetWrapperCreator(Type taskType)
        {
            return wrapperCreators.GetOrAdd(taskType,
                                            (Type t) =>
                                            {
                                                if (t == typeof(Task))
                                                {
                                                    //Single Task
                                                    return CreateWrapperTask;
                                                }
                                                else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>))
                                                {
                                                    //Generic task
                                                    return (Func<Task, IMethodInvocation, Task>)this.GetType()
                                                        .GetMethod("CreateGenericWrapperTask", BindingFlags.Instance | BindingFlags.NonPublic)
                                                            .MakeGenericMethod(new Type[]
                                                                              {
                                                                                  t.GenericTypeArguments[0]
                                                                              })
                                                                .CreateDelegate(typeof(Func<Task, IMethodInvocation, Task>), this);
                                                }
                                                else
                                                {
                                                    //Other cases are not supported and the self task is added
                                                    return (task, _) => task;
                                                }
                                            });
        }

        /// <summary>
        /// This method returns a Task that waits for the original Task to complete and logs the return or the exception if an errors occurs
        /// </summary>
        /// <param name="task">Task to be wrapped</param>
        /// <param name="input">Input to be passed to the task</param>
        /// <returns>A Task that waits for the original Task to complete</returns>
        private async Task CreateWrapperTask(Task task, IMethodInvocation input)
        {
            try
            {
                await task.ConfigureAwait(false);
                Trace(string.Format("{0}. Method {1} finished", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), input.MethodBase), TraceEventType.Information);
            }
            catch (Exception ex)
            {
                TraceError(input, ex);
                throw;
            }
        }

        /// <summary>
        /// This method returns a Task that waits for the original Task to complete and logs the return or the exception if an errors occurs
        /// </summary>
        /// <typeparam name="T">Type returned by the task</typeparam>
        /// <param name="task">Task to be wrapped</param>
        /// <param name="input">Input to be passed to the task</param>
        /// <returns>A Task that waits for the original Task to complete</returns>
        private Task CreateGenericWrapperTask<T>(Task task, IMethodInvocation input)
        {
            return DoCreateGenericWrapperTask<T>((Task<T>)task, input);
        }

        /// <summary>
        /// This method returns a Task that waits for the original Task to complete and logs the return or the exception if an errors occurs
        /// </summary>
        /// <typeparam name="T">Type returned by the task</typeparam>
        /// <param name="task">Task to be wrapped</param>
        /// <param name="input">Input to be passed to the task</param>
        /// <returns>A Task that waits for the original Task to complete</returns>
        private async Task<T> DoCreateGenericWrapperTask<T>(Task<T> task, IMethodInvocation input)
        {
            try
            {
                T value = await task.ConfigureAwait(false);
                Trace(string.Format("{0}. Method {1} returned {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), input.MethodBase, value), TraceEventType.Information);
                return value;
            }
            catch (Exception ex)
            {
                TraceError(input, ex);
                throw;
            }
        }

        #endregion

    }
}
