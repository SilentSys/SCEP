﻿Imports Fiddler
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO
Imports System.IO.Compression
Imports System.Threading
Imports HtmlAgilityPack
Imports System.Xml
Imports Newtonsoft.Json.Linq
Imports System.Web
Imports System.Text

Public Class SCEP
    Dim Mytrade As New Trades
    Dim Theirtrade As New Trades

    Public EnabledOnProfiles As Boolean = True
    Public EnabledInInvites As Boolean = True
    Public EnableInTrades As Boolean = True

    Public Shared Inventory As New List(Of String)
    Sub StartProxy()

        If Not FiddlerApplication.IsSystemProxy Then
            AddHandler FiddlerApplication.BeforeResponse, AddressOf FiddlerBeforeResponseHandler
            AddHandler FiddlerApplication.BeforeRequest, AddressOf FiddlerBeforeRequestHandler
        End If

        FiddlerApplication.Startup(8090, True, False, False)


    End Sub
    Sub StopProxy()

        Try
            RemoveHandler FiddlerApplication.BeforeResponse, AddressOf FiddlerBeforeResponseHandler
            RemoveHandler FiddlerApplication.BeforeRequest, AddressOf FiddlerBeforeRequestHandler
            FiddlerApplication.Shutdown()
            System.Threading.Thread.Sleep(500)
        Catch ex As Exception
        End Try

    End Sub
    Private Sub FiddlerBeforeRequestHandler(ByVal tSession As Session)
        tSession.bBufferResponse = True

        Dim WebDir As List(Of String) = tSession.url.Split("/").ToList
        WebDir.RemoveAll(AddressOf String.IsNullOrEmpty)



        If WebDir(0) = "steamcommunity.com" Then

            If WebDir.Count > 1 Then
                If WebDir(1) = "scep" Then
                    Select Case WebDir(2)
                        Case "profiles"
                            Dim ReturnS As String = SteamRep(WebDir(3))
                            If String.IsNullOrEmpty(ReturnS) Then
                                ReturnS = "SR ERROR"
                            End If
                            Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes("<span class=""profile_in_game_header"" style=""color:#FFFFFF;"">Status:</span><br>" & ReturnS & "<br>"))
                            tSession.LoadResponseFromStream(stream, True)
                            tSession.Ignore()

                        Case "invites"
                            Dim ReturnS As String = SteamRep(WebDir(3))
                            If String.IsNullOrEmpty(ReturnS) Then
                                ReturnS = "SR ERROR"
                            End If
                            Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(ReturnS))
                            tSession.LoadResponseFromStream(stream, True)
                            tSession.Ignore()

                        Case "trade"
                            Dim response As String = "0"

                            If WebDir.Count = 4 Then
                                Select Case WebDir(3)

                                    Case "me"
                                        Dim WholeAmmout As Integer = 0

                                        If Mytrade.ItemAmmount.Count > 0 Then
                                            For Each item In Mytrade.ItemAmmount
                                                If IsNumeric(item) Then
                                                    WholeAmmout += item
                                                End If
                                            Next
                                        End If

                                        response = WholeAmmout.ToString & " Item(s) in trade"
                                        If Not String.IsNullOrEmpty(Mytrade.ItemsCost) Then
                                            response = response & " @ $" & Mytrade.ItemsCost
                                        End If


                                    Case "them"
                                        Dim WholeAmmout As Integer = 0

                                        If Theirtrade.ItemAmmount.Count > 0 Then
                                            For Each item In Theirtrade.ItemAmmount
                                                If IsNumeric(item) Then
                                                    WholeAmmout += item
                                                End If
                                            Next
                                        End If

                                        response = WholeAmmout.ToString & " Item(s) in trade"
                                        If Not String.IsNullOrEmpty(Theirtrade.ItemsCost) Then
                                            response = response & " @ $" & Theirtrade.ItemsCost
                                        End If
                                End Select
                            End If


                            Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(response))
                            tSession.LoadResponseFromStream(stream, True)
                            tSession.Ignore()
                    End Select


                End If
            End If

        Else
            tSession.Ignore()
        End If
    End Sub
    Private Sub FiddlerBeforeResponseHandler(ByVal tSession As Session)



        If tSession.fullUrl.Contains("steamcommunity.com/") Then

            Dim HeaderOnly As Boolean = False

            Dim WebDir As List(Of String) = tSession.url.Split("/").ToList
            WebDir.RemoveAll(AddressOf String.IsNullOrEmpty)



            If WebDir.Count > 1 Then
                Select Case True
                    Case EnabledInInvites And WebDir(WebDir.Count - 1) = "invites" And tSession.oResponse.headers.ExistsAndContains("Content-Type", "text/html")
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        Body = Invites(Body)

                        Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(Body))
                        tSession.LoadResponseFromStream(stream, True)

                    Case EnabledOnProfiles And WebDir(1) = "profiles" And tSession.oResponse.headers.ExistsAndContains("Location", "http://steamcommunity.com/id/") And tSession.oResponse.headers.ExistsAndContains("Content-Type", "text/html")

                        Dim headers As String = tSession.oResponse.headers.ToString
                        HeaderOnly = True

                    Case EnabledOnProfiles And WebDir(1) = "profiles" And tSession.oResponse.headers.ExistsAndContains("Content-Type", "text/html")
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        Body = Profile(Body)

                        Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(Body))
                        tSession.LoadResponseFromStream(stream, True)

                    Case EnabledOnProfiles And WebDir(1) = "id" And tSession.oResponse.headers.ExistsAndContains("Content-Type", "text/html")
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        Body = Profile(Body)

                        Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(Body))
                        tSession.LoadResponseFromStream(stream, True)

                    Case EnableInTrades And WebDir(1) = "trade" And tSession.oResponse.headers.ExistsAndContains("Content-Type", "text/html")
                        If WebDir.Count = 3 Then
                            tSession.utilDecodeResponse()
                            Dim Body As String = tSession.GetResponseBodyAsString

                            Body = Trade(Body)
                            ClearAllTrade()

                            Dim stream As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(Body))
                            tSession.LoadResponseFromStream(stream, True)
                        End If

                    Case WebDir(1) = "trade" And WebDir(WebDir.Count - 1) = "additem"
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        MyTradeJSON(Body)
                        Mytrade.PriceCheck()

                    Case WebDir(1) = "trade" And WebDir(WebDir.Count - 1) = "removeitem"
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        MyTradeJSON(Body)
                        Mytrade.PriceCheck()

                    Case WebDir(1) = "trade" And WebDir(WebDir.Count - 1) = "tradestatus"
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        TheirTradeJSON(Body)

                    Case WebDir(WebDir.Count - 1) = "?trading=1"
                        If WebDir.Count = 8 Then
                            If WebDir(3) = "inventory" And WebDir(4) = "json" Then
                                tSession.utilDecodeResponse()
                                Dim Body As String = tSession.GetResponseBodyAsString

                                Inventory.Add(Body)
                            End If
                        End If


                    Case WebDir(1) = "trade" And WebDir(WebDir.Count - 2) = "foreigninventory"
                        tSession.utilDecodeResponse()
                        Dim Body As String = tSession.GetResponseBodyAsString

                        Inventory.Add(Body)
                        Theirtrade.PriceCheck()

                End Select
            End If

        End If

    End Sub
    Public Shared Function recog(ByVal str As String, ByVal reg As String) As String
        Dim keyd As Match = Regex.Match(str, reg)
        Return (keyd.Groups(1).ToString)
    End Function
    Shared Function RequestPage(ByVal URL As String, ByVal Referer As String, ByVal Post As String, Optional ByVal ReturnHeader As Boolean = False) As String
        Dim EncodedURL As New Uri(HttpUtility.UrlPathEncode(URL))

        Dim WebR As HttpWebRequest = DirectCast(WebRequest.Create(EncodedURL), HttpWebRequest)

        WebR.Method = "GET"
        WebR.AllowAutoRedirect = False

        WebR.KeepAlive = True
        WebR.UserAgent = "SteamCEP"
        WebR.ContentType = "application/x-www-form-urlencoded"
        WebR.Referer = Referer
        WebR.Headers.Add("Accept-Encoding: gzip, deflate")
        WebR.Headers.Add("Accept-Language: en-US,en;q=0.5")
        WebR.Headers.Add("X-Requested-With: XMLHttpRequest")
        WebR.Accept = "text/javascript, text/html, application/xml, text/xml, */*"

        Dim WebResponse As HttpWebResponse = DirectCast(WebR.GetResponse, HttpWebResponse)

        Dim responseStream As Stream = WebResponse.GetResponseStream()
        If (WebResponse.ContentEncoding.ToLower().Contains("gzip")) Then
            responseStream = New GZipStream(responseStream, CompressionMode.Decompress)
        ElseIf (WebResponse.ContentEncoding.ToLower().Contains("deflate")) Then
            responseStream = New DeflateStream(responseStream, CompressionMode.Decompress)
        End If


        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(responseStream)

        Select Case ReturnHeader
            Case False
                Dim ret As String = sr.ReadToEnd
                responseStream.Close()
                sr.Close()
                Return ret
            Case True
                Dim ret As String = WebResponse.Headers.ToString()
                responseStream.Close()
                sr.Close()
                Return ret
        End Select

        responseStream.Close()
        sr.Close()
    End Function
    Private Function Profile(ByVal HTML As String, Optional ByVal status As Boolean = False) As String
        Try

            Dim DivSelectr As String
            If HTML.Contains("<div class=""profile_badges"">") Then
                DivSelectr = "//div[@class='profile_badges']"
            ElseIf HTML.Contains("<div class=""profile_private_info"">") Then
                DivSelectr = "//div[@class='profile_private_info']"
            Else
                Return HTML
            End If

            Dim htmldoc As New HtmlDocument
            htmldoc.LoadHtml(HTML)

            Dim HtmlDivNode As HtmlNode = HtmlNode.CreateNode("<div id=""scep-rightcol""><img src=""http://steamcommunity.com/public/images/login/throbber.gif""/></div>")

            Dim FriendDiv = htmldoc.DocumentNode.SelectSingleNode(DivSelectr)
            Dim NewHTML As HtmlNode = FriendDiv.ParentNode.InsertAfter(HtmlDivNode, FriendDiv)
            Dim ProcessedHTML As String = AddEndScript(htmldoc.DocumentNode.OuterHtml, "profiles", "rightcol", SteamIDprofile(HTML))

            Return ProcessedHTML

        Catch ex As Exception
        End Try
        Return HTML
    End Function
    Private Function Invites(ByVal HTML As String) As String
        Try
            If HTML.Contains("<div class=""invite_row""") Then

                Dim htmldoc As New HtmlDocument
                htmldoc.LoadHtml(HTML)
                Dim sIDs As New List(Of String)

                Dim PersonDivs As HtmlAgilityPack.HtmlNodeCollection = htmldoc.DocumentNode.SelectNodes("//div[@class='invite_row']")
                For Each person In PersonDivs
                    Dim sID As String = recog(person.InnerHtml, "<a class=""linkStandard"" href=""javascript:FriendAccept\( '(.*?)', 'accept' \)"">Accept</a>")
                    sIDs.Add(sID)


                    Dim HtmlDivNode As HtmlNode = HtmlNode.CreateNode("<div id=""scep-" & sID & """><img src=""http://steamcommunity.com/public/images/login/throbber.gif""/></div>")
                    Dim TitleDiv = person.SelectSingleNode(".//div[@class='eventTitle']")

                    Dim NewHTML As HtmlNode = TitleDiv.ParentNode.InsertAfter(HtmlDivNode, TitleDiv)
                Next


                Dim ProcessedHTML As String = htmldoc.DocumentNode.OuterHtml

                For Each id In sIDs
                    ProcessedHTML = AddEndScript(ProcessedHTML, "invites", id, id)
                Next






                Return ProcessedHTML
            End If

        Catch ex As Exception
        End Try
        Return HTML
    End Function
    Private Function AddEndScript(ByVal HTML As String, ByVal location As String, ByVal DivName As String, ByVal sID As String) As String
        Try
            If HTML.Contains("jQuery(document).ready") Then

            ElseIf HTML.Contains("<body class=""") Then


                Dim BodyTag As String = "<body class=""" & recog(HTML, "<body class=""(.*?)"">") & """>"
                HTML = HTML.Insert(HTML.LastIndexOf(BodyTag) + BodyTag.Length, "<script type=""text/javascript"">jQuery(document).ready(function($) {});</script>")
            Else
                Return vbNullString
            End If

            Dim htmldoc As New HtmlDocument
            htmldoc.LoadHtml(HTML)
            Dim ScriptDiv = htmldoc.DocumentNode.SelectSingleNode("//script[@type='text/javascript' and contains(.,'jQuery(document).ready')]")
            ScriptDiv.InnerHtml = ScriptDiv.InnerHtml.Insert(ScriptDiv.InnerHtml.LastIndexOf("});"), "jQuery(""#scep-" & DivName & """).load(""/scep/" & location & "/" & sID & """);")

            Return htmldoc.DocumentNode.OuterHtml

        Catch ex As Exception
            Return vbNullString
        End Try

    End Function
    Private Function SteamIDprofile(ByVal HTML As String) As String
        If HTML.Contains("g_rgProfileData = {") Then
            Try

                Dim SID As String = recog(HTML, "g_rgProfileData = {.*?""steamid"":""(\d*?)"".*?};")
                Return SID

            Catch ex As Exception
                Return vbNullString
            End Try

        End If
        Return vbNullString
    End Function
    Private Function SteamRep(ByVal SID As String) As String
        If Not String.IsNullOrEmpty(SID) Then
            Try
                Dim SRbody As String = RequestPage("http://steamrep.com/api/beta2/reputation/" & SID, "", "")
                Dim SRep As String = recog(SRbody, "<full>(.*?)</full>")
                Dim SRlink As String = recog(SRbody, "<steamrepurl>(.*?)</steamrepurl>")

                If Not String.IsNullOrWhiteSpace(SRep) Then
                    Dim stats As List(Of String) = SRep.Split(",").ToList
                    Dim SCEPrep As New List(Of String)

                    For Each stat In stats
                        Select Case True
                            Case stat.ToLower.Contains("donator")
                                SCEPrep.Add("<font color=""#57CBDE"">" & stat & "</font>")

                            Case stat.ToLower.Contains("admin")
                                SCEPrep.Add("<font color=""#90BA3C"">" & stat & "</font>")

                            Case stat.ToLower.Contains("caution")
                                SCEPrep.Add("<font color=""#FC970A"">" & stat & "</font>")

                            Case stat.ToLower.Contains("banned")
                                SCEPrep.Add("<font color=""#A94847"">" & stat & "</font>")

                            Case stat.ToLower.Contains("scammer")
                                SCEPrep.Add("<font color=""#A94847"">" & stat & "</font>")

                            Case Else
                                SCEPrep.Add(stat)
                        End Select

                    Next

                    SRep = String.Join(", ", SCEPrep.ToArray)
                Else
                    SRep = "CLEAN"
                End If

                Return " <a href=""" & SRlink & """><span class=""count_link_label"" style=""color:#FFFFFF;font-size:14px;"">" & SRep & "</span></a>"

            Catch ex As Exception
                Return vbNullString
            End Try

        End If
        Return vbNullString
    End Function
    Private Function Trade(ByVal HTML As String) As String
        If HTML.Contains("<div class=""tutorial_arrow_ctn"">") And HTML.Contains("<div class=""readystate"" id=""them_notready"">") And HTML.Contains("<body>") Then
            HTML = HTML.Insert(HTML.LastIndexOf("<body>"), "<script type=""text/javascript"">jQuery(document).ready(function() {setInterval(function() {jQuery(""#scep-mytrade"").load(""/scep/trade/me"");jQuery(""#scep-theirtrade"").load(""/scep/trade/them"");}, 1000);});</script>")

            HTML = HTML.Insert(HTML.LastIndexOf("<div class=""tutorial_arrow_ctn"">"), "<div id=""scep-mytrade"" style=""color:#57CBDE;font-size:14px;text-align:center;""><img src=""http://steamcommunity.com/public/images/login/throbber.gif""/></div><br>")

            HTML = HTML.Insert(HTML.LastIndexOf("<div class=""readystate"" id=""them_notready"">"), "<div id=""scep-theirtrade"" style=""color:#57CBDE;font-size:14px;text-align:center;""><img src=""http://steamcommunity.com/public/images/login/throbber.gif""/></div><br>")
        End If

        Return HTML
    End Function
    Public Class Trades
        Public Items As New List(Of String)
        Public ItemAmmount As New List(Of String)
        Public ItemsCost As String

        Public Sub AddItem(ByVal id As String, ByVal ammount As String)
            Items.Add(id)
            ItemAmmount.Add(ammount)
        End Sub
        Public Sub clear()
            Items.Clear()
            ItemAmmount.Clear()
            ItemsCost = vbNullString
        End Sub
        Public Sub PriceCheck()
            Try
                ItemsCost = 0

                For Each item In Items
                    Dim record As String = String.Join("", Inventory)

                    Dim ClassId As String = recog(record, """" & item & """:{""id"":""" & item & """,""classid"":""(.*?)""")

                    Dim appid As String = recog(record, "{""appid"":""(\d{3})"",""classid"":""" & ClassId & """")

                    Dim MarketHash As String = recog(record, "{""appid"":""" & appid & """,""classid"":""" & ClassId & """.*?""market_hash_name"":""(.*?)""")

                    If Not String.IsNullOrWhiteSpace(MarketHash) And Not String.IsNullOrWhiteSpace(appid) Then
                        Dim pricepage As String = RequestPage("http://steamcommunity.com/market/listings/" & appid & "/" & MarketHash, "", "")
                        Dim ItemLastPrice As String = recog(pricepage, "var line1=\[.*\[.*?,(.*?),.*?\]\];")
                        Dim ItemLastPrice2 As String = recog(pricepage, "var line1=\[.*\[(.*?)\]\];")

                        Dim ItemPrice As Double
                        If Double.TryParse(ItemLastPrice, ItemPrice) Then
                            ItemsCost += ItemPrice
                        End If
                    End If



                Next

            Catch ex As Exception
            End Try
        End Sub
    End Class
    Private Sub MyTradeJSON(ByVal Data As String)
        Try
            Mytrade.clear()

            Dim jo As JObject = JObject.Parse(Data)
            Dim playerid As List(Of JToken) = jo("me")("assets").Children.ToList

            For Each item In playerid
                Dim thestring As String = item.ToString
                Dim id As String = recog(thestring, """assetid"":.*?""(.*?)""")
                Dim ammount As String = recog(thestring, """amount"":.*?""(.*?)""")
                Mytrade.AddItem(id, ammount)
            Next
        Catch ex As Exception
            Mytrade.clear()
        End Try

    End Sub
    Private Sub TheirTradeJSON(ByVal Data As String)
        Try
            If Not String.IsNullOrEmpty(recog(Data, """them"":\{.*?""assets"":(.*?)\}")) Then

                Theirtrade.clear()

                Dim jo As JObject = JObject.Parse(Data)

                Dim playerid As List(Of JToken) = jo("them")("assets").Children.ToList

                For Each item In playerid
                    Dim thestring As String = item.ToString
                    Dim id As String = recog(thestring, """assetid"":.*?""(.*?)""")
                    Dim ammount As String = recog(thestring, """amount"":.*?""(.*?)""")
                    Theirtrade.AddItem(id, ammount)
                    Theirtrade.PriceCheck()
                Next

            End If
        Catch ex As Exception
            Theirtrade.clear()
        End Try

    End Sub
    Private Sub ClearAllTrade()
        Mytrade.clear()
        Theirtrade.clear()
        Inventory.Clear()
    End Sub
End Class
