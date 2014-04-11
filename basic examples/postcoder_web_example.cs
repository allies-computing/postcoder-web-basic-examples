// PostCoder Web Service V3 example
// Allies Computing Ltd 2013 
//
// Demonstrates UK Address lookup rest web service
//
// Product URL: http://www.alliescomputing.com/address-lookup/postcoder-web
// Technical Specs: http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs
// Example output: http://ws.postcoder.com/pcw/PCW45-12345-12345-1234X/address/UK/NR147PZ
//
// This demo shows how to perform an address lookup and parse the results into C# objects.
//
// Out of the box it uses a restricted API Key that limits the search to the postcode NR14 7PZ
//
// To experiment with different search terms, signup for a free evaluation API/Search key at:
// http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now
//
//  To explore different searches, Signup for an API/Search Key and assign to searchKey variable
// 
// Alternatively the code below can be pasted into an online c# compiler such as 
// http://www.compileonline.com/compile_csharp_online.php
//
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Serialization;

public class PCWV3Client
{

    // address class
    [Serializable]
    public class Address
    {
        public string summaryline { get; set; }
        public string organisation { get; set; }
        public string premise { get; set; }
        public string dependentstreet { get; set; }
        public string street { get; set; }
        public string doubledependentlocality { get; set; }
        public string dependentlocality { get; set; }
        public string posttown { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }

        public override string ToString() { return summaryline; }
    }

    // root element of service output
    // essentially an array of addresses
    [XmlRoot("Addresses")]
    public class Addresses
    {

        [XmlElement("Address")]
        public List<Address> Address { get; set; }
    }

    // code entry point
    public static void Main(string[] args)
    {
        // API/search key - MUST be supplied to unlock search, 
        // if this is blank a restricted key is used which limits search to NR147PZ
        string searchKey = "";
        // processing type - for other options see 
        string method = "address";
        // search string - MUST be supplied, if blank defaults to NR147PZ
        string searchTerm = "NR14 7PZ";

		Console.WriteLine("PostCoder Web V3 C# Client Snippet" + Environment.NewLine);
        try
        {
            if (String.IsNullOrEmpty(searchKey))
            {
                // no search key supplied - use the restricted evaluation key
                searchKey = "PCW45-12345-12345-1234X";
                searchTerm = "NR14 7PZ";
                Console.WriteLine(
                    "No search-key: using restricted evaluation key - resets search term to NR14 7PZ" + Environment.NewLine
                    + "To obtain a free evaluation key, signup via "
                    + "http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now" + Environment.NewLine);
            }

            // format the url		
            string uri = String.Format("http://ws.postcoder.com/pcw/{0}/{1}/UK/{2}",
                                            searchKey,
                                            method,
                                            System.Uri.EscapeDataString(searchTerm)
                                            );
            // for other uri options see http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs

            // call the service
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Accept = "application/xml"; // xml or json permitted

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // check for call failure
                if (response.StatusCode != HttpStatusCode.OK) // = HTTP 200 code (OK)
                {
                    throw new ApplicationException(String.Format("Failed : HTTP error code : {0}", response.StatusCode));
                }

                // grab the response
                var addresses = (Addresses)((new XmlSerializer(typeof(Addresses))).Deserialize(response.GetResponseStream()));
                response.Close();

                // trivial output - display list of address summaries to console
                Console.WriteLine(String.Format("Results for: {0}", searchTerm));
                if (addresses.Address != null)
                {
                    foreach (var address in addresses.Address)
                    {
                        Console.WriteLine(address);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}

