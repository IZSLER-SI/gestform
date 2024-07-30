Imports System.Xml
Imports System.Data.SqlTypes

Public Class XmlParser

    Public Shared Function ParseXmlString(ByVal xNode As xmlnode, ByVal attributeName As String) As String
        If xNode.Attributes(attributeName) Is Nothing Then
            Return ""
        Else
            Return xNode.Attributes(attributeName).Value
        End If
    End Function

    Public Overloads Shared Function ParseXmlInteger(ByVal xNode As XmlNode, ByVal attributeName As String) As Integer
        If xNode.Attributes(attributeName) Is Nothing Then
            Return 0
        Else
            Return CInt(xNode.Attributes(attributeName).Value)
        End If
    End Function

    Public Overloads Shared Function ParseXmlInteger(s As String) As Integer
        If s = "" Then
            Return 0
        Else
            Return CInt(s)
        End If
    End Function

    Public Overloads Shared Function ParseXmlDecimal(ByVal xNode As XmlNode, ByVal attributeName As String) As Decimal
        If xNode.Attributes(attributeName) Is Nothing Then
            Return 0D
        Else
            Return Decimal.Parse(xNode.Attributes(attributeName).Value, Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Overloads Shared Function ParseXmlDecimal(s As String) As Decimal
        If s = "" Then
            Return 0D
        Else
            Return Decimal.Parse(s, Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Overloads Shared Function ParseXmlDouble(ByVal xNode As XmlNode, ByVal attributeName As String) As Double
        If xNode.Attributes(attributeName) Is Nothing Then
            Return CDbl(0)
        Else
            Return Double.Parse(xNode.Attributes(attributeName).Value, Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Overloads Shared Function ParseXmlDouble(s As String) As Double
        If s = "" Then
            Return CDbl(0)
        Else
            Return Double.Parse(s, Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Shared Function ParseXmlBoolean01(ByVal xNode As XmlNode, ByVal attributeName As String) As Boolean
        If xNode.Attributes(attributeName) Is Nothing Then
            Return False
        Else
            Return xNode.Attributes(attributeName).Value = "1"
        End If
    End Function

    Public Overloads Shared Function ParseXmlDateOnly(ByVal xNode As XmlNode, ByVal attributeName As String) As DateTime
        If xNode.Attributes(attributeName) Is Nothing Then
            Return Date.Today
        Else
            Return Date.ParseExact(Left(xNode.Attributes(attributeName).Value, 10), "yyyy-MM-dd", Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Overloads Shared Function ParseXmlDateOnly(ByVal s As String) As DateTime
        If s = "" Then
            Return Date.Today
        Else
            Return Date.ParseExact(Left(s, 10), "yyyy-MM-dd", Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Shared Function FormatXmlDateOnly(d As Date) As String
        Return d.ToString("yyyy-MM-dd", Softailor.Global.Cultures.CulturaXml)
    End Function

    Public Overloads Shared Function ParseXmlTimeOnly(ByVal xNode As XmlNode, ByVal attributeName As String) As DateTime
        If xNode.Attributes(attributeName) Is Nothing Then
            Return Date.Today
        Else
            Return Date.ParseExact(Left(xNode.Attributes(attributeName).Value, 8), "HH:mm:ss", Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Overloads Shared Function ParseXmlTimeOnly(ByVal s As String) As DateTime
        If s = "" Then
            Return Date.Today
        Else
            Return Date.ParseExact(Left(s, 8), "HH:mm:ss", Softailor.Global.Cultures.CulturaXml)
        End If
    End Function

    Public Shared Function FormatXmlTimeOnly(d As Date) As String
        Return d.ToString("HH:mm:ss", Softailor.Global.Cultures.CulturaXml)
    End Function


    Public Shared Function ParseXmlSqlString(ByVal xNode As XmlNode, ByVal attributeName As String) As SqlString
        If xNode.Attributes(attributeName) Is Nothing Then
            Return SqlString.Null
        Else
            Return New SqlString(xNode.Attributes(attributeName).Value)
        End If
    End Function

    Public Shared Function ParseXmlSqlInt32(ByVal xNode As XmlNode, ByVal attributeName As String) As SqlInt32
        If xNode.Attributes(attributeName) Is Nothing Then
            Return SqlInt32.Null
        Else
            Return New SqlInt32(CInt(xNode.Attributes(attributeName).Value))
        End If
    End Function

    Public Shared Function ParseXmlSqlMoney(ByVal xNode As XmlNode, ByVal attributeName As String) As SqlMoney
        If xNode.Attributes(attributeName) Is Nothing Then
            Return SqlMoney.Null
        Else
            Return New SqlMoney(Decimal.Parse(xNode.Attributes(attributeName).Value, Softailor.Global.Cultures.CulturaXml))
        End If
    End Function

    Public Shared Function ParseXmlSqlBoolean01(ByVal xNode As XmlNode, ByVal attributeName As String) As SqlBoolean
        If xNode.Attributes(attributeName) Is Nothing Then
            Return SqlBoolean.Null
        Else
            Return New SqlBoolean(xNode.Attributes(attributeName).Value = "1")
        End If
    End Function

    Public Shared Function ParseXmlSqlDateTimeDateOnly(ByVal xNode As XmlNode, ByVal attributeName As String) As SqlDateTime
        If xNode.Attributes(attributeName) Is Nothing Then
            Return SqlDateTime.Null
        Else
            Return New SqlDateTime(Date.ParseExact(Left(xNode.Attributes(attributeName).Value, 10), "yyyy-MM-dd", Softailor.Global.Cultures.CulturaXml))
        End If
    End Function



End Class
