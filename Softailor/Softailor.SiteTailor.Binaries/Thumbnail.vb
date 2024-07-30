Public Class Thumbnail
    Public FileName As String  'nome completo su file system
    Public Width As Integer     'larghezza imposta (o zero se non imposta)
    Public Height As Integer    'altezza imposta (o zero se non imposta)
    Public MimeType As String  'Tipo MIME

    Public Sub New()
        FileName = ""
        Width = 0
        Height = 0
        MimeType = ""
    End Sub
End Class
