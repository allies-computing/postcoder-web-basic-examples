' PostCoder Web Service V3 example
' Allies Computing Ltd 2013 
'
' Demonstrates UK Address lookup rest web service
'
' Product URL: http://www.alliescomputing.com/address-lookup/postcoder-web
' Technical Specs: http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs
' Example output: http://ws.postcoder.com/pcw/PCW45-12345-12345-1234X/address/UK/NR147PZ
'
' This demo shows how to perform an address lookup and parse the results into VB.Net objects.
'
' Out of the box it uses a restricted API Key that limits the search to the postcode NR14 7PZ
'
' To experiment with different search terms, signup for a free evaluation API/Search key at:
' http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now
'
'  To explore different searches, Signup for an API/Search Key and assign to searchKey variable
' 
' Alternatively the code below can be pasted into an online c# compiler such as 
' http://www.compileonline.com/compile_vb.net_online.php
'
Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Text
Imports System.Xml.Serialization

Public Class PCWV3Client


    ' address class
    Public Class Address

        Public organisation As String
        Public premise As String
        Public dependentstreet As String
        Public street As String
        Public doubledependentlocality As String
        Public dependentlocality As String
        Public posttown As String
        Public county As String
        Public postcode As String

        Public summaryline As String

        Public Overrides Function ToString() As String
            Return summaryline
        End Function

    End Class

    ' root element of service output
    ' essentially an array of addresses
    <XmlRootAttribute("Addresses")> _
    Public Class Addresses
        <XmlElementAttribute("Address")> _
        Public Address As List(Of Address)
    End Class

    ' code entry point
    Public Shared Sub Main()
        ' API/search key - MUST be supplied to unlock search, 
        ' if this is blank a restricted key is used which limits search to NR147PZ
        Dim searchKey As String = ""
        ' processing type - for other options see 
        Dim method As String = "address"
        ' search string - MUST be supplied, if blank defaults to NR147PZ
        Dim searchTerm As String = "NR14 7PZ"

        Console.WriteLine("PostCoder Web V3 VB.Net Client Snippet" & vbNewLine)
        Try
            If (String.IsNullOrEmpty(searchKey)) Then
                ' no search key supplied - use the restricted evaluation key
                searchKey = "PCW45-12345-12345-1234X"
                searchTerm = "NR14 7PZ"
                Console.WriteLine("No search-key: using restricted evaluation key - resets search term to NR14 7PZ" & vbNewLine & _
                                    "To obtain a free evaluation key, signup via " & _
                                    "http://www.alliescomputing.com/address-lookup/postcoder-web/get-started-now" & vbNewLine _
                                    )
            End If

            ' format the url		
            Dim uri As String = String.Format("http://ws.postcoder.com/pcw/{0}/{1}/UK/{2}", _
                searchKey, _
                method, _
                System.Uri.EscapeDataString(searchTerm) _
                )
            ' for other uri options see http://www.alliescomputing.com/address-lookup/postcoder-web/tech-specs

            ' call the service
            Dim request As HttpWebRequest = CType(WebRequest.Create(uri),HttpWebRequest)
            request.Method = "GET"
            request.Accept = "application/xml" ' xml or json permitted

            Using response As HttpWebResponse = CType(request.GetResponse(),HttpWebResponse)
                ' check for call failure
                If (response.StatusCode <> HttpStatusCode.OK) Then ' = HTTP 200 code (OK) 
                    Throw New ApplicationException(String.Format("Failed : HTTP error code : {0}", response.StatusCode))
                End If

                ' grab the response
                Dim addresses As Addresses = New XmlSerializer(GetType(Addresses)).Deserialize(response.GetResponseStream())
                response.Close()

                ' trivial output - display list of address summaries to console
                Console.WriteLine(String.Format("Results for: {0}", searchTerm))
                If addresses.Address IsNot Nothing Then

                    For Each address As Address In addresses.Address
                        Console.WriteLine(address)
                    Next
                End If
            End Using

        Catch e As Exception
            Console.WriteLine(e.ToString())
        End Try
    End Sub
End Class

