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
}
