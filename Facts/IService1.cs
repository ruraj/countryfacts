using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Facts
{
  // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
  [ServiceContract]
  public interface FactService
  {

    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json)]
    Task<String> GetData();

    [OperationContract]
    [WebGet]
    Stream GetExcel();
  }

  [DataContract]
  public class Value
  {
    string name;
    string val;
    string unit;

    public Value(string n, string v, string u)
    {
      name = n;
      val = v;
      unit = u;
    }

    [DataMember]
    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    [DataMember]
    public string Val
    {
      get { return val; }
      set { val = value; }
    }

    [DataMember]
    public string Unit
    {
      get { return unit; }
      set { unit = value; }
    }
  }

  //public class Program
  //{
  //  static void Main(string[] args)
  //  {
  //    WebServiceHost host = new WebServiceHost(typeof(Service), new Uri("http://localhost:8000/"));
  //    try
  //    {
  //      ServiceEndpoint ep = host.AddServiceEndpoint(typeof(FactService), new WebHttpBinding(), "");
  //      host.Open();
  //      using (ChannelFactory<FactService> cf = new ChannelFactory<FactService>(new WebHttpBinding(), "http://localhost:8000"))
  //      {
  //        cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
  //      }

  //      Console.WriteLine("Press <ENTER> to terminate");
  //      Console.ReadLine();

  //      host.Close();
  //    }
  //    catch (CommunicationException cex)
  //    {
  //      Console.WriteLine("An exception occurred: {0}", cex.Message);
  //      host.Abort();
  //    }
  //  }
  //}
}
