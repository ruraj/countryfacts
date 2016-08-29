using System;
using System.Linq;
using AngleSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AngleSharp.Dom;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using Amba.SpreadsheetLight;

namespace Facts
{
  // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
  // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
  public class Service1 : FactService
  {

    async public Task<String> GetData()
    {
      try
      {
        var result = await GetDataImpl();
        return JsonConvert.SerializeObject(result);
      } catch (Exception e)
      {
        e.ToString();
      }
      return null;
    }

    async private Task<List<Value>> GetDataImpl()
    {
      // Setup the configuration to support document loading
      var config = Configuration.Default.WithDefaultLoader();
      // Load the names of all The Big Bang Theory episodes from Wikipedia
      var address = "https://www.cia.gov/library/publications/the-world-factbook/geos/by.html";
      // Asynchronously get the document in a new context using the configuration
      var document = await BrowsingContext.New(config).OpenAsync(address);

      // $('h2[sectiontitle="Energy"]').parent().parent().next().find('div:nth-child(3n-2)').filter((i, e) => $(e).text().indexOf('Electricity') >= 0).map((i, e) => { return { name: $(e).text(), value: $(e).next().text()} })
      // Let's get this JS equivalent down...
      var values = (List<Value>)document.QuerySelectorAll("h2[sectiontitle='Energy']").ToList()
        .SelectMany(x => x.ParentElement.NextElementSibling.QuerySelectorAll("div:nth-child(3n-2)").ToArray())
        .Where(i => i.TextContent.Contains("Electricity"))
        .Select(m => makeValue(m)).ToList();

      return values;
    }

    private Value makeValue(IElement m)
    {
      return new Value(m.TextContent.Substring(0, m.TextContent.Length - 1), m.NextSibling.NextSibling.TextContent, "N/A");

      // TODO Get value and unit separated... probably won't be possible without some pre-def API though.
      //var valueString = m.NextElementSibling.TextContent.Split(' ');

      //return new Facts.Value(m.TextContent, valueString[0], valueString[1]);
    }

    public Stream GetExcel()
    {
      var filePath = makeExcel(GetDataImpl().Result);

      String headerInfo = "attachment; filename=burundi.xlsx";

      WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"] = headerInfo;

      WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";

      return File.OpenRead(filePath);
    }

    private string makeExcel(List<Value> values)
    {
      try
      {
        var tempFilePath = Path.GetTempFileName();

        SLDocument sl = new SLDocument();

        // Main title and Column Headers

        sl.SetCellValue(1, 1, "Burundi Electricity Facts");
        sl.SetCellValue(2, 1, "Field");
        sl.SetCellValue(2, 2, "Value");
        sl.SetCellValue(2, 3, "Unit");

        var rowcount = 0;

        // Fill in rows
        foreach (Value value in values)
        {
          rowcount += 1;

          sl.SetCellValue(rowcount, 1, value.Name);
          sl.SetCellValue(rowcount, 2, value.Val);
          sl.SetCellValue(rowcount, 3, value.Unit);
        }

        sl.SaveAs(tempFilePath);

        return tempFilePath;

        //var excel = new Application();
        //excel.Visible = false;
        //excel.DisplayAlerts = false;
        //var workBook = excel.Workbooks.Add(Type.Missing);

        //var workSheet = (Worksheet)workBook.ActiveSheet;
        //workSheet.Name = "Burundi";

        //workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 2]].Merge();
        //workSheet.Cells[1, 1] = "Burundi Electricity Facts";
        //workSheet.Cells.Font.Size = 12;

        //var rowcount = 0;

        //// Write the column headers
        //workSheet.Cells[2, 1] = "Field";
        //workSheet.Cells[2, 2] = "Value";
        //workSheet.Cells[2, 3] = "Unit";

        //// Fill in rows
        //foreach (Value value in values)
        //{
        //  rowcount += 1;

        //  workSheet.Cells[rowcount, 1] = value.Name;
        //  workSheet.Cells[rowcount, 2] = value.Val;
        //  workSheet.Cells[rowcount, 3] = value.Unit;
        //}

        //var tempFilePath = Path.GetTempFileName();

        //workBook.SaveAs(tempFilePath);
        //workBook.Close();
        //excel.Quit();

        //return tempFilePath;
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
        throw ex;
      }
    }
  }
}
