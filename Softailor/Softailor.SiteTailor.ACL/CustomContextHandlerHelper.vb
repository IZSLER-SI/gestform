Public Class CustomContextHandlerHelper
    Public Shared Function GetApplicationCustomContextHandlerList() As List(Of ICustomContextHandler)
        Dim l As New List(Of ICustomContextHandler)
        Dim key As String
        Dim assemblyClassName As String
        Dim assemblyName As String
        Dim className As String

        For Each key In System.Configuration.ConfigurationManager.AppSettings.AllKeys
            If key Like "sitetailor_CustomContextHandler*" Then
                assemblyClassName = System.Configuration.ConfigurationManager.AppSettings(key)
                assemblyName = assemblyClassName.Split(","c)(0).Trim
                className = assemblyClassName.Split(","c)(1).Trim
                Dim iCustomContextHandler As ICustomContextHandler
                Dim o As System.Runtime.Remoting.ObjectHandle = Activator.CreateInstance(assemblyName, className)
                iCustomContextHandler = CType(o.Unwrap, ICustomContextHandler)
                l.Add(iCustomContextHandler)
            End If
        Next

        Return l
    End Function
End Class
