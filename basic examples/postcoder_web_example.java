// PostCoder Web Service V3 example
// Allies Computing Ltd 2013 
//
// Demonstrates UK Address lookup rest web service
//
// Product URL: http://www.alliescomputing.com/address-lookup/postcoder-web
// Technical Specs: http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs
// Example output: http://ws.postcoder.com/pcw/PCW45-12345-12345-1234X/address/UK/NR147PZ
//
// This demo shows how to perform an address lookup and parse the results into Java objects.
//
// Out of the box it uses a restricted API Key that limits the search to the postcode NR14 7PZ
//
// To experiment with different search terms, signup for a free evaluation API/Search key at:
// http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now
//
// Usage
// 1) Save file as PCWV3Client (to match the class name)
// 3) Compile, e.g. javac PCWV3Client.java
// 4) Run, e.g. java PCWV3Client
// Next steps:
//  - To explore different searches, Signup for an API/Search Key and assign to searchKey variable
// 
// Alternatively the code below can be pasted into an online java compiler such as 
// http://www.compileonline.com/compile_java_online.php
//
import java.util.*;
import java.lang.*;
import java.net.HttpURLConnection;  
import java.net.URL;
import javax.xml.bind.*;
import javax.xml.bind.annotation.*;

public class PCWV3Client {

    // address class
    private static class Address {
        public String organisation;
        public String premise;
        public String dependentstreet;
        public String street;
        public String doubledependentlocality;
        public String dependentlocality;
        public String posttown;
        public String county;
        public String postcode;    
        public String summaryline;
        @Override public String toString() {
            return summaryline;
        }
    }  
    
    // root element of service output
    // essentially an array of addresses
    // jaxb bindings
    @XmlRootElement(name="Addresses") 
    @XmlAccessorType(XmlAccessType.FIELD)
    private static class Addresses {
        // 
        @XmlElement(name="Address")
        public Address[] address;
    }    
        
    // code entry point
    public static void main(String []args)
    {
        // API/search key - MUST be supplied to unlock search, 
        // if this is blank a restricted key is used which limits search to NR147PZ
        String searchKey = "";          
        // processing type - for other options see 
        String method = "address"; 
        // search string - MUST be supplied, if blank defaults to NR147PZ
        String searchTerm = "NR14 7PZ";              
             
		System.out.println("PostCoder Web V3 Java Client Snippet\n");
        try
        {
    		if(searchKey.isEmpty()) { 
                // no search key supplied - use the restricted evaluation key
                searchKey = "PCW45-12345-12345-1234X";
                searchTerm = "NR14 7PZ";                
                System.out.println(
                    "No search-key: using restricted evaluation key - resets search term to NR14 7PZ\n"
                    + "To obtain a free evaluation key, signup via "
                    + "http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now\n");
            }
		
			// format the url		
			String uri = String.format("http://ws.postcoder.com/pcw/%s/%s/UK/%s", 
                                            searchKey, 
                                            method, 
                                            java.net.URLEncoder.encode(searchTerm,"UTF-8"));
			// for other uri options see http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs
            
            // call the service
            HttpURLConnection connection = (HttpURLConnection) new URL(uri).openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Accept", "application/xml"); // xml or json permitted, jaxb requires xml 
         
            // check for call failure
        	if (connection.getResponseCode() != HttpURLConnection.HTTP_OK) { // = HTTP 200 code (OK)
    			throw new RuntimeException("Failed : HTTP error code : " + connection.getResponseCode());
    		}            
            
            // convert output to java array using jaxb
            Addresses addresses = (Addresses) JAXBContext.newInstance(Addresses.class)
                                                .createUnmarshaller()
                                                .unmarshal(connection.getInputStream());
            connection.disconnect();                                                

            // trivial output - display list of address summaries to console
			System.out.println("Results for:" + searchTerm + "\n");
            if(addresses.address != null) {            
                for(int i = 0; i < addresses.address.length; ++i) {
                    Address address = addresses.address[i];
                    System.out.println(String.format("%s",address));
                }
            }		    
        }
        catch(Exception e) {
            e.printStackTrace();
        }
     }
}
