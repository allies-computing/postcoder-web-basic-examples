<?php
  /*
  PostCoder Web Service V3 example
	Allies Computing Ltd 2014 
	
	Demonstrates UK Address lookup rest web service
	
	Product URL: http://www.alliescomputing.com/address-lookup/postcoder-web
	Technical Specs: http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs
	Example output: http://ws.postcoder.com/pcw/PCW45-12345-12345-1234X/address/UK/NR147PZ

	This demo shows how to perform an address lookup and parse the results into php objects.
	
	Out of the box it uses a restricted API Key that limits the search to the postcode NR14 7PZ
	
	To experiment with different search terms, signup for a free evaluation API/Search key at:
	http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now

	Alternatively the code below can be pasted into an online interpreter such as 
	http://www.compileonline.com/execute_php_online.php

    Note: This script has been developed for php version >= 5.2.0
    cURL and json_decode/json_encode functions are used.
  */
  
  // API/search key - MUST be supplied to unlock search, 
  // if this is blank a restricted key is used which limits search to NR147PZ  
  $searchKey = '';    // replace with your search key
  $searchterm  = 'NR147PZ';         // string to use for an address search
  
  echo 'PostCoder Web V3 PHP Client Snippet<br><br>';
  if(empty($searchKey)) {
  	$searchKey = 'PCW45-12345-12345-1234X';
  	$searchterm = "NR14 7PZ";   
  	echo 'No search-key: using restricted evaluation key - resets search term to NR14 7PZ<br>'
  		. 'To obtain a free evaluation key, signup via'
  		. 'http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now<br>'
      . '<br>';	
  }
  // build the URL, using the 'address' search method:
  $URL = 'http://ws.postcoder.com/pcw/' . $searchKey .'/address/UK/' . rawurlencode($searchterm) ;
  // for other uri options see http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs
  
  // use cURL to send the request and get the response
  $session = curl_init($URL); 
  // Tell cURL to return the request data
  curl_setopt($session, CURLOPT_RETURNTRANSFER, true); 
  // use application/json to specify json return values, the default is XML.
  $headers = array('Content-Type: application/json');
  curl_setopt($session, CURLOPT_HTTPHEADER, $headers);
  
  // Execute cURL on the session handle
  $response = curl_exec($session);
  
  // Close the cURL session
  curl_close($session);
  
  // decode the response
  $addresses = json_decode($response);
  
  // trivial output 
  echo 'Results for: ' . $searchterm . '<br><br>';
  if (count($addresses) > 0){  
    foreach ($addresses as $address){
      echo  $address->summaryline,"<br>";
    }
  }
  else{
    echo 'No results, please try again.';
  }
?>
