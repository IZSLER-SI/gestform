Imports System.Data.SqlClient
Imports Microsoft.SharePoint.Client
Imports System.Configuration.ConfigurationManager

Public Class SharepointHelper

    'costanti
    Public Const f_Documents = "Documents"
    Public Const f_GestioneFormazione = "GestioneFormazione"
    Public Const f_Documenti = "Documenti"
    Public Const f_Modelli = "Modelli"
    Public Const f_Eventi = "Eventi"
    Public Const f_Generici = "Generici"
    Public Const f_EventoPrefix = "Evento_"

    Public Shared Function SharepointBaseUrl() As String
        Return AppSettings("sharepoint_site_root")
    End Function

    Public Shared Function DocsGFBaseUrl() As String
        Return AppSettings("onedrive_site_root") & f_Documents & "/" & f_GestioneFormazione & "/"
    End Function

    Public Shared Function DocsGFRelativeBase() As String
        Return AppSettings("onedrive_site_relativebase") & f_Documents & "/" & f_GestioneFormazione & "/"
    End Function

    Public Shared Function EventDocsUrl(id_EVENTO As Integer) As String
        Return AppSettings("onedrive_site_root") &
            f_Documents & "/" & f_GestioneFormazione & "/" & f_Documenti & "/" & f_Eventi & "/" &
            f_EventoPrefix & id_EVENTO.ToString
    End Function

    Public Shared Function LogonAndStore(session As System.Web.SessionState.HttpSessionState) As ClientContext

        Dim context As ClientContext
        Dim username As String
        Dim password As String

        context = New ClientContext(AppSettings("onedrive_site_root"))
        username = AppSettings("onedrive_username")
        password = AppSettings("onedrive_password")
        Dim securePwd As New System.Security.SecureString()
        For Each c In password.ToCharArray
            securePwd.AppendChar(c)
        Next
        context.Credentials = New SharePointOnlineCredentials(username, securePwd)

        'aggancio root e carico le sottocartelle
        Dim web = context.Web
        context.Load(web)
        context.ExecuteQuery()

        If session IsNot Nothing Then
            session("sharepoint_context") = context
        End If

        Return context

    End Function

    Public Shared Sub EnsureBaseFoldersCreated(context As ClientContext)

        'variabili
        Dim web As Microsoft.SharePoint.Client.Web
        Dim Documents As Folder
        Dim Documents_GestioneFormazione As Folder
        Dim Documents_GestioneFormazione_Modelli As Folder
        Dim Documents_GestioneFormazione_Documenti As Folder
        Dim subfolders As FolderCollection
        Dim folder As Folder
        Dim flag1 As Boolean
        Dim flag2 As Boolean
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim SottocartelleModelliDaCreare As List(Of String)

        'apertura connessione
        dbConn = Softailor.SiteTailor.ACL.DbConnectionHandler.GetOpenDataDbConn

        'aggancio root e carico le sottocartelle
        web = context.Web
        Documents = web.Folders.GetByUrl(f_Documents)
        subfolders = Documents.Folders
        context.Load(Documents)
        context.Load(subfolders)
        context.ExecuteQuery()

        'creo la cartella GestioneFormazione
        flag1 = False
        For Each folder In subfolders
            If folder.Name = f_GestioneFormazione Then
                flag1 = True
            End If
        Next
        If Not flag1 Then
            subfolders.Add(f_GestioneFormazione)
            context.ExecuteQuery()
        End If

        'cartelle all'interno di GestioneFormazione
        Documents_GestioneFormazione = Documents.Folders.GetByUrl(f_GestioneFormazione)
        subfolders = Documents_GestioneFormazione.Folders
        context.Load(Documents_GestioneFormazione)
        context.Load(subfolders)
        context.ExecuteQuery()

        'sottocartelle

        'sottocartella Modelli
        flag1 = False
        For Each folder In subfolders
            If folder.Name = f_Modelli Then
                flag1 = True
            End If
        Next
        'sottocartella Documenti
        flag2 = False
        For Each folder In subfolders
            If folder.Name = f_Documenti Then
                flag2 = True
            End If
        Next
        If Not flag1 Then
            subfolders.Add(f_Modelli)
        End If
        If Not flag2 Then
            subfolders.Add(f_Documenti)
        End If

        If Not flag1 Or Not flag2 Then
            context.ExecuteQuery()
        End If

        'sottocartelle di Modelli (da DB)
        Documents_GestioneFormazione_Modelli = Documents_GestioneFormazione.Folders.GetByUrl(f_Modelli)
        subfolders = Documents_GestioneFormazione_Modelli.Folders
        context.Load(Documents_GestioneFormazione_Modelli)
        context.Load(subfolders)
        context.ExecuteQuery()

        'recupero elenco sottocartelle che devono esistere
        SottocartelleModelliDaCreare = New List(Of String)

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT tx_SHAREPOINTFOLDER FROM rpt_FONTI WHERE tx_SHAREPOINTFOLDER is not null"
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            flag1 = False
            For Each folder In subfolders
                If folder.Name = dbRdr.GetString(0) Then
                    flag1 = True
                End If
            Next
            If Not flag1 Then
                SottocartelleModelliDaCreare.Add(dbRdr.GetString(0))
            End If
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        If SottocartelleModelliDaCreare.Count > 0 Then
            For Each sottocartella In SottocartelleModelliDaCreare
                subfolders.Add(sottocartella)
            Next
            context.ExecuteQuery()
        End If

        'sottocartelle di Documenti
        Documents_GestioneFormazione_Documenti = Documents_GestioneFormazione.Folders.GetByUrl(f_Documenti)
        subfolders = Documents_GestioneFormazione_Documenti.Folders
        context.Load(Documents_GestioneFormazione_Documenti)
        context.Load(subfolders)
        context.ExecuteQuery()

        'sottocartella Eventi
        flag1 = False
        For Each folder In subfolders
            If folder.Name = f_Eventi Then
                flag1 = True
            End If
        Next
        'sottocartella DocumentiGenerali
        flag2 = False
        For Each folder In subfolders
            If folder.Name = f_Generici Then
                flag2 = True
            End If
        Next
        If Not flag1 Then
            subfolders.Add(f_Eventi)
        End If
        If Not flag2 Then
            subfolders.Add(f_Generici)
        End If
        If Not flag1 Or Not flag2 Then
            context.ExecuteQuery()
        End If

        'chiusura connessione
        dbConn.Close()

    End Sub

    Public Shared Sub EnsureEventFolderCreated(id_EVENTO As Integer)

        'client context
        Dim context = GetClientContext()

        'variabili
        Dim web As Microsoft.SharePoint.Client.Web
        Dim Documents_GestioneFormazione_Documenti_Eventi As Folder
        Dim subfolders As FolderCollection
        Dim folder As Folder
        Dim flag1 As Boolean

        'aggancio root e carico le sottocartelle
        web = context.Web

        Documents_GestioneFormazione_Documenti_Eventi = web.GetFolderByServerRelativeUrl(
            f_Documents & "/" & f_GestioneFormazione & "/" & f_Documenti & "/" & f_Eventi)
        subfolders = Documents_GestioneFormazione_Documenti_Eventi.Folders
        context.Load(Documents_GestioneFormazione_Documenti_Eventi)
        context.Load(subfolders)
        context.ExecuteQuery()

        'sottocartella relativa all'evento
        flag1 = False
        For Each folder In subfolders
            If folder.Name = f_EventoPrefix & id_EVENTO.ToString Then
                flag1 = True
            End If
        Next
        If Not flag1 Then
            subfolders.Add(f_EventoPrefix & id_EVENTO.ToString)
        End If
        If Not flag1 Then
            context.ExecuteQuery()
        End If

    End Sub

    Public Shared Function GetClientContext() As ClientContext

        'eseguo l'autenticazione se non ho il context nella request, altrimenti uso quello
        If System.Web.HttpContext.Current.Session("sharepoint_context") IsNot Nothing Then
            Return CType(System.Web.HttpContext.Current.Session("sharepoint_context"), ClientContext)
        Else
            Return LogonAndStore(System.Web.HttpContext.Current.Session)
        End If

    End Function

    Public Shared Function GetFolderFiles(folder As String, extensions() As String) As XmlDocument

        Dim web As Microsoft.SharePoint.Client.Web
        Dim documentsFolder As Folder
        Dim context As ClientContext
        Dim files As FileCollection
        Dim sWriter As System.IO.StringWriter
        Dim xWriter As XmlTextWriter
        Dim xDoc As New XmlDocument
        Dim ext As String
        Dim include As Boolean
        Dim filter As Boolean

        context = GetClientContext()
        web = context.Web
        documentsFolder = web.GetFolderByServerRelativeUrl(
            f_Documents & "/" & f_GestioneFormazione & "/" & folder)
        files = documentsFolder.Files
        context.Load(documentsFolder)
        context.Load(files)
        context.ExecuteQuery()

        sWriter = New System.IO.StringWriter
        xWriter = New XmlTextWriter(sWriter)

        xWriter.WriteStartDocument()
        xWriter.WriteStartElement("files")

        filter = extensions.Count > 0

        For Each file In files
            ext = System.IO.Path.GetExtension(file.Name)

            include = True
            If filter Then
                include = extensions.Contains(ext)
            End If

            If include Then
                xWriter.WriteStartElement("file")
                xWriter.WriteAttributeString("name", file.Name)
                xWriter.WriteAttributeString("ext", ext.Replace(".", ""))
                xWriter.WriteAttributeString("url", file.ServerRelativeUrl)
                xWriter.WriteEndElement()
            End If
        Next

        xWriter.WriteEndElement()
        xWriter.WriteEndDocument()

        xDoc = New XmlDocument
        xDoc.LoadXml(sWriter.ToString)
        xWriter.Dispose()

        Return xDoc

    End Function

    Public Shared Function GetModello(dbConn As SqlConnection, id_REPORT As Integer, loadFile As Boolean) As SharepointFile

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim result As New SharepointFile


        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_rpt_GetDatiModello"
            .Parameters.Add("@id_REPORT", SqlDbType.Int).Value = id_REPORT
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        result.modelSource = dbRdr.GetString(0)
        result.modelSourceDescription = dbRdr.GetString(1)
        result.modelDescription = dbRdr.GetString(4)
        result.modelFileName = dbRdr.GetString(6)
        result.modelFileNameWithoutExtension = IO.Path.GetFileNameWithoutExtension(result.modelFileName)
        result.modelFileExtension = IO.Path.GetExtension(result.modelFileName)
        result.modelType = dbRdr.GetString(5)

        result.modelRelativePath = DocsGFRelativeBase() & f_Modelli & "/" & dbRdr.GetString(2) & "/" & result.modelFileName

        result.sourceXmlDefinitionRelativePath = "~/RPT/Fonti/" & result.modelSource & ".xml"

        Select Case result.modelType
            Case "Word"
                result.MIMEType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            Case "Excel"
                result.MIMEType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        End Select

        result.OutputFilePrefix = CleanFileName(result.modelDescription)

        If dbRdr.IsDBNull(7) Then result.defaultOrdinamento = "" Else result.defaultOrdinamento = dbRdr.GetString(7)
        If dbRdr.IsDBNull(8) Then result.defaultFiltro = 0 Else result.defaultFiltro = dbRdr.GetInt32(8)
        dbRdr.Close()
        dbCmd.Dispose()

        If loadFile Then
            Dim context = GetClientContext()
            result.memoryStream = New IO.MemoryStream
            Using respStream = Microsoft.SharePoint.Client.File.OpenBinaryDirect(context, result.modelRelativePath).Stream
                respStream.CopyTo(result.memoryStream)
            End Using
        End If

        Return result

    End Function

    Public Shared Function GetModelloMail(dbConn As SqlConnection, id_MAILREPORT As Integer) As SharepointFile

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim result As New SharepointFile


        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_rpt_GetDatiModelloMail"
            .Parameters.Add("@id_MAILREPORT", SqlDbType.Int).Value = id_MAILREPORT
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        result.modelSource = dbRdr.GetString(0)
        result.modelSourceDescription = dbRdr.GetString(1)
        result.modelDescription = dbRdr.GetString(3)
        result.sourceXmlDefinitionRelativePath = "~/RPT/Fonti/" & result.modelSource & ".xml"
        If dbRdr.IsDBNull(4) Then result.defaultOrdinamento = "" Else result.defaultOrdinamento = dbRdr.GetString(4)
        If dbRdr.IsDBNull(5) Then result.defaultFiltro = 0 Else result.defaultFiltro = dbRdr.GetInt32(5)
        dbRdr.Close()
        dbCmd.Dispose()

        Return result

    End Function

    Public Shared Function StoreFile(subFolder As String, nomeFile As String, mStream As IO.MemoryStream) As String

        Dim context = GetClientContext()
        Dim fileFullPath = DocsGFRelativeBase() & f_Documenti & "/" & subFolder & "/" & nomeFile
        mStream.Position = 0
        Microsoft.SharePoint.Client.File.SaveBinaryDirect(context, fileFullPath, mStream, False)
        mStream.Dispose()

        Return fileFullPath
    End Function

    Private Shared Function FileExistsInt(folder As String, filename As String) As Boolean

        Dim web As Microsoft.SharePoint.Client.Web
        Dim documentsFolder As Folder
        Dim context As ClientContext
        Dim files As FileCollection
        Dim exists As Boolean

        context = GetClientContext()
        web = context.Web
        documentsFolder = web.GetFolderByServerRelativeUrl(folder)
        files = documentsFolder.Files
        context.Load(documentsFolder)
        context.Load(files)
        context.ExecuteQuery()

        exists = False
        For Each file In files
            If file.Name = filename Then
                exists = True
                Exit For
            End If
        Next

        Return exists

    End Function

    Public Shared Function FileExists(subFolder As String, nomeFile As String) As Boolean

        Dim fullPath = DocsGFRelativeBase() & f_Documenti & "/" & subFolder
        Return FileExistsInt(fullPath, nomeFile)

    End Function

    Public Class SharepointFile
        Public modelSource As String
        Public modelSourceDescription As String
        Public modelRelativePath As String
        Public modelFileName As String
        Public modelDescription As String
        Public modelFileNameWithoutExtension As String
        Public modelFileExtension As String
        Public modelType As String
        Public sourceXmlDefinitionRelativePath As String
        Public memoryStream As IO.MemoryStream
        Public MIMEType As String
        Public OutputFilePrefix As String
        Public defaultOrdinamento As String
        Public defaultFiltro As Integer
    End Class

    Public Const InvalidChars As String = "\/:*?""<>|"

    Private Shared Function CleanFileName(s As String) As String

        Dim sOut As String = ""

        For Each c In s.ToCharArray
            If Not InvalidChars.Contains(c) Then
                sOut &= c
            End If
        Next

        Return sOut

    End Function

    Public Shared Function ValidFileName(s As String) As Boolean

        Dim valid = True

        For Each c In s.ToCharArray
            If InvalidChars.Contains(c) Then
                valid = False
                Exit For
            End If
        Next

        Return valid

    End Function
End Class
