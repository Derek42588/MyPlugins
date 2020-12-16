using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Xml;

namespace MyPlugins
{
    public class TaskCreate : IPlugin
    {

        int tax = 0;
        public TaskCreate(string unsecureConfig, string secureConfig)
        {
            XmlDocument xmlUnsecureConfig = new XmlDocument();

            xmlUnsecureConfig.LoadXml(unsecureConfig);

            XmlNodeList elemlist = xmlUnsecureConfig.GetElementsByTagName("value1");
            tax = Convert.ToInt32(elemlist.Item(0).InnerText);
        }
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);



            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity contact = (Entity)context.InputParameters["Target"];


                try
                {
                    // Plug-in business logic goes here.  

                    Entity taskRecord = new Entity("task");

                    string contactName = string.Empty;
                    if (contact.Attributes.Contains("firstname")) {
                        contactName = $"{contact.Attributes["firstname"]} {contact.Attributes["lastname"]}";
                    } else
                    {
                        contactName = $"{contact.Attributes["lastname"]}";
                    }

                    taskRecord.Attributes.Add("subject", "Follow up");
                    
                    taskRecord.Attributes.Add("description", $"Please follow up with {contactName}.  Tax is: {tax}");

                    // Date
                    taskRecord.Attributes.Add("scheduledend", DateTime.Now.AddDays(2));

                    // Option set value as "high" (priority)

                    taskRecord.Attributes.Add("prioritycode", new OptionSetValue(2));

                    // Parent record or Look Up

                    //taskRecord.Attributes.Add("regardingobjectid", new EntityReference("contact", contact.Id));
                    //shortcut for above:

                    taskRecord.Attributes.Add("regardingobjectid", contact.ToEntityReference());

                    Guid taskGuid = service.Create(taskRecord);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
