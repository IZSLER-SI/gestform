Imports System.Data.SqlClient
Imports Softailor.Global.SqlUtils


Public Class FormatoAccettato
    Public CODFORMA As String
    Public DESFORMA As String
    Public EXTE_PUTs As List(Of String)
    Public EXTE_GET As String
    Public MIMETYPE As String
    Public DEFTHUMB As String

    Public Sub New()
        CODFORMA = ""
        DESFORMA = ""
        EXTE_PUTs = New List(Of String)
        EXTE_GET = ""
        MIMETYPE = ""
        DEFTHUMB = ""
    End Sub

    Public Property EXTE_PUT() As String
        Set(ByVal value As String)
            EXTE_PUTs.Clear()
            Dim exts As String() = value.Split(";"c)
            Dim ext As String
            For Each ext In exts
                If ext.Trim <> "" Then
                    EXTE_PUTs.Add(ext.Trim.ToLower)
                End If
            Next
        End Set
        Get
            Dim s As String = ""
            Dim ext As String
            For Each ext In EXTE_PUTs
                s &= ext & ";"
            Next
            Return s
        End Get
    End Property

    Public Function ContainsEXTE_PUT(ByVal ext As String) As Boolean
        Return EXTE_PUTs.Contains(ext.ToLower)
    End Function
End Class
