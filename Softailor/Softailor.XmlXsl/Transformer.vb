Imports System.Xml
Imports System.Xml.Xsl
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Web
Imports System.Web.UI

Public Class Transformer

    Public Overloads Shared Sub Transform(ByVal dbCmd As SqlCommand, ByVal xsltRelativePath As String, ByVal control As Control, ByVal ParamArray paramNameValue() As String)
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'lettura dei dati
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'genero il literal
        control.Controls.Add(New System.Web.UI.LiteralControl(sWriter.ToString))
        sWriter.Close()

        'disistanzio
        xReader = Nothing
        xDoc = Nothing
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing

    End Sub
    Public Overloads Shared Sub Transform_AbsoluteTemplatePath(ByVal dbCmd As SqlCommand, ByVal xsltAbsolutePath As String, ByVal control As Control, ByVal ParamArray paramNameValue() As String)
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'lettura dei dati
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'genero il literal
        control.Controls.Add(New System.Web.UI.LiteralControl(sWriter.ToString))
        sWriter.Close()

        'disistanzio
        xReader = Nothing
        xDoc = Nothing
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing

    End Sub

    Public Overloads Shared Function Transform(ByVal dbCmd As SqlCommand, ByVal xsltRelativePath As String, ByVal ParamArray paramNameValue() As String) As String
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'lettura dei dati
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'ritorno il testo della trasformazione
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xReader = Nothing
        xDoc = Nothing
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing

    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal dbCmd As SqlCommand, ByVal xsltAbsolutePath As String, ByVal ParamArray paramNameValue() As String) As String
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'lettura dei dati
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'ritorno il testo della trasformazione
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xReader = Nothing
        xDoc = Nothing
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing

    End Function

    Public Overloads Shared Sub Transform(ByVal xDoc As XmlDocument, ByVal xsltRelativePath As String, ByVal control As Control, ByVal ParamArray paramNameValue() As String)
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'genero il literal
        control.Controls.Add(New System.Web.UI.LiteralControl(sWriter.ToString))
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Sub
    Public Overloads Shared Sub Transform_AbsoluteTemplatePath(ByVal xDoc As XmlDocument, ByVal xsltAbsolutePath As String, ByVal control As Control, ByVal ParamArray paramNameValue() As String)
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'genero il literal
        control.Controls.Add(New System.Web.UI.LiteralControl(sWriter.ToString))
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Sub

    Public Overloads Shared Sub Transform(ByVal xReader As XmlReader, ByVal xsltRelativePath As String, ByVal control As Control, ByVal ParamArray paramNameValue() As String)
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xReader, xArgs, sWriter)
        xReader.Close()
        xReader = Nothing

        'genero il literal
        control.Controls.Add(New System.Web.UI.LiteralControl(sWriter.ToString))
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Sub
    Public Overloads Shared Sub Transform_AbsoluteTemplatePath(ByVal xReader As XmlReader, ByVal xsltAbsolutePath As String, ByVal control As Control, ByVal ParamArray paramNameValue() As String)
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xReader, xArgs, sWriter)
        xReader.Close()
        xReader = Nothing

        'genero il literal
        control.Controls.Add(New System.Web.UI.LiteralControl(sWriter.ToString))
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Sub

    Public Overloads Shared Function Transform(ByVal xDoc As XmlDocument, ByVal xsltRelativePath As String, ByVal ParamArray paramNameValue() As String) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal xDoc As XmlDocument, ByVal xsltAbsolutePath As String, ByVal ParamArray paramNameValue() As String) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function

    Public Overloads Shared Function Transform(ByVal xDoc As XmlDocument, ByVal xsltRelativePath As String, ByVal ParamArray paramNameValue() As Object) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(CStr(paramNameValue(i)), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal xDoc As XmlDocument, ByVal xsltAbsolutePath As String, ByVal ParamArray paramNameValue() As Object) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(CStr(paramNameValue(i)), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function

    Public Overloads Shared Function Transform(ByVal xReader As XmlReader, ByVal xsltRelativePath As String, ByVal ParamArray paramNameValue() As String) As String

        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xReader, xArgs, sWriter)
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing

    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal xReader As XmlReader, ByVal xsltAbsolutePath As String, ByVal ParamArray paramNameValue() As String) As String

        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'genero gli argomenti
        xArgs = New XsltArgumentList
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xReader, xArgs, sWriter)
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing

    End Function

    Public Overloads Shared Function Transform(ByVal xDoc As XmlDocument, ByVal xsltRelativePath As String, ByVal xpathNodeIteratorParameterName As String, ByVal xpathNodeIterator As XPath.XPathNodeIterator, ByVal ParamArray paramNameValue() As String) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        xArgs = New XsltArgumentList

        'genero l'argomento xmldoc
        xArgs.AddParam(xpathNodeIteratorParameterName, "", xpathNodeIterator)

        'genero gli argomenti
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal xDoc As XmlDocument, ByVal xsltAbsolutePath As String, ByVal xpathNodeIteratorParameterName As String, ByVal xpathNodeIterator As XPath.XPathNodeIterator, ByVal ParamArray paramNameValue() As String) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        xArgs = New XsltArgumentList

        'genero l'argomento xmldoc
        xArgs.AddParam(xpathNodeIteratorParameterName, "", xpathNodeIterator)

        'genero gli argomenti
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function

    Public Overloads Shared Function Transform(ByVal xDoc As XmlDocument, ByVal xsltRelativePath As String, ByVal xpathNodeIteratorParameterName1 As String, ByVal xpathNodeIterator1 As XPath.XPathNodeIterator, ByVal xpathNodeIteratorParameterName2 As String, ByVal xpathNodeIterator2 As XPath.XPathNodeIterator, ByVal ParamArray paramNameValue() As String) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        xArgs = New XsltArgumentList

        'genero gli argomenti xmldoc
        xArgs.AddParam(xpathNodeIteratorParameterName1, "", xpathNodeIterator1)
        xArgs.AddParam(xpathNodeIteratorParameterName2, "", xpathNodeIterator2)

        'genero gli argomenti
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal xDoc As XmlDocument, ByVal xsltAbsolutePath As String, ByVal xpathNodeIteratorParameterName1 As String, ByVal xpathNodeIterator1 As XPath.XPathNodeIterator, ByVal xpathNodeIteratorParameterName2 As String, ByVal xpathNodeIterator2 As XPath.XPathNodeIterator, ByVal ParamArray paramNameValue() As String) As String
        Dim xTransform As XslCompiledTransform
        Dim xArgs As XsltArgumentList
        Dim sWriter As StringWriter
        Dim i As Integer

        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        xArgs = New XsltArgumentList

        'genero gli argomenti xmldoc
        xArgs.AddParam(xpathNodeIteratorParameterName1, "", xpathNodeIterator1)
        xArgs.AddParam(xpathNodeIteratorParameterName2, "", xpathNodeIterator2)

        'genero gli argomenti
        For i = paramNameValue.GetLowerBound(0) To paramNameValue.GetUpperBound(0) Step 2
            xArgs.AddParam(paramNameValue(i), "", paramNameValue(i + 1))
        Next

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing
        xArgs = Nothing
    End Function

    Public Overloads Shared Function Transform(ByVal xDoc As XmlDocument, ByVal xsltRelativePath As String, ByVal xArgs As XsltArgumentList) As String
        Dim xTransform As XslCompiledTransform
        Dim sWriter As StringWriter


        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(HttpContext.Current.Server.MapPath(xsltRelativePath))

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'ritorno il testo della trasformazione
        Transform = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing

    End Function
    Public Overloads Shared Function Transform_AbsoluteTemplatePath(ByVal xDoc As XmlDocument, ByVal xsltAbsolutePath As String, ByVal xArgs As XsltArgumentList) As String
        Dim xTransform As XslCompiledTransform
        Dim sWriter As StringWriter


        'istanzio il trasformatore
        xTransform = New XslCompiledTransform
        xTransform.Load(xsltAbsolutePath)

        'trasformo
        sWriter = New StringWriter
        xTransform.Transform(xDoc, xArgs, sWriter)

        'ritorno il testo della trasformazione
        Transform_AbsoluteTemplatePath = sWriter.ToString
        sWriter.Close()

        'disistanzio
        xTransform = Nothing
        sWriter = Nothing

    End Function

    
End Class
