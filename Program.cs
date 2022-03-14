using CsvHelper;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;


public readonly record struct KvObject(double lat,double lng, int objectId);
public readonly record struct KvSearchQuery(int dealTypeId,int countyId, int parishId, int roomNr ){
        public string url =>  $"https://www.kv.ee/?act=search.objectcoords&deal_type={dealTypeId}&page=1&orderby=ob&page_size=100000&search_type=new&county={countyId}&parish={parishId}&zoom=25&rooms_min={roomNr}&rooms_max={roomNr}";
};


class Program {
    static void Main(string[] args) {        
        KvSearchQuery searchQuery = new KvSearchQuery(dealTypeId:1,countyId:7,parishId:1050,roomNr:1);
        string url = searchQuery.url;
        Console.WriteLine(url);
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);
        //Does Runtime Type Checking
        List<List<string>> kvObjectQueryResults = JsonConvert.DeserializeObject<List<List<string>>>(doc.DocumentNode.InnerHtml);

        List<KvObject> kvObjects = new List<KvObject>();
        foreach (var item in kvObjectQueryResults)
        {
            kvObjects.Add(new KvObject(lat:Convert.ToDouble(item[0]),lng:Convert.ToDouble(item[1]),objectId:Convert.ToInt32(item[2])));
        }

    }

}

