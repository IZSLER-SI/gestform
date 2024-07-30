Imports System.Web.HttpContext
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Collections.Specialized


Public Class ContextHandler
    Private Const ID_AZIENkey As String = "sitetailor_ID_AZIEN"
    Private Const ID_UTENTkey As String = "sitetailor_ID_UTENT"
    Private Const COGNUTENkey As String = "sitetailor_COGNUTEN"
    Private Const NOMEUTENkey As String = "sitetailor_NOMEUTEN"
    Private Const USERNAMEKey As String = "sitetailor_USERNAME"
    Private Const EMAIL_UTkey As String = "sitetailor_EMAIL_UT"
    Private Const RAGSOCIAkey As String = "sitetailor_RAGSOCIA"
    Private Const USERMENUkey As String = "sitetailor_USERMENU"
    Private Const USERMENUOriginalkey As String = "sitetailor_USERMENUOriginal"
    Private Const SGLAPPLIkey As String = "sitetailor_SGLAPPLI"
    Private Const DESAPPLIkey As String = "sitetailor_DESAPPLI"
    Private Const USERFUNCkey As String = "sitetailor_USERFUNC"
    Private Const ID_UNITKey As String = "sitetailor_ID_UNIT"
    Private Const AC_UNITKey As String = "sitetailor_AC_UNIT"
    Private Const TX_UNITKey As String = "sitetailor_TX_UNIT"
    Private Const DISCRIMIKey As String = "sitetailor_DISCRIMI"

    Private Const BinariesBasePathkey As String = "sitetailor_BinariesBasePath"

    Private Const SmtpServerkey As String = "sitetailor_SmtpServer"
    Private Const ErrorReportMailFromkey As String = "sitetailor_ErrorReportMailFrom"
    Private Const ErrorReportMailTokey As String = "sitetailor_ErrorReportMailTo"
    Private Const ErrorReportMailSubjectkey As String = "sitetailor_ErrorReportMailSubject"


    Public Shared Property ID_AZIEN() As Integer
        Get
            Return CInt(Current.Session(ID_AZIENkey))
        End Get
        Set(ByVal value As Integer)
            Current.Session(ID_AZIENkey) = value
        End Set
    End Property

    Public Shared Property ID_UTENT() As Integer
        Get
            Return CInt(Current.Session(ID_UTENTkey))
        End Get
        Set(ByVal value As Integer)
            Current.Session(ID_UTENTkey) = value
        End Set
    End Property

    Public Shared Property ID_UNIT() As Integer
        Get
            Return CInt(Current.Session(ID_UNITKey))
        End Get
        Set(ByVal value As Integer)
            Current.Session(ID_UNITKey) = value
        End Set
    End Property

    Public Shared Property DISCRIMI() As String
        Get
            Return CStr(Current.Session(DISCRIMIKey))
        End Get
        Set(ByVal value As String)
            Current.Session(DISCRIMIKey) = value
        End Set
    End Property

    Public Shared Property AC_UNIT() As String
        Get
            Return CStr(Current.Session(AC_UNITKey))
        End Get
        Set(ByVal value As String)
            Current.Session(AC_UNITKey) = value
        End Set
    End Property

    Public Shared Property TX_UNIT() As String
        Get
            Return CStr(Current.Session(TX_UNITKey))
        End Get
        Set(ByVal value As String)
            Current.Session(TX_UNITKey) = value
        End Set
    End Property

    Public Shared Property COGNUTEN() As String
        Get
            Return CStr(Current.Session(COGNUTENkey))
        End Get
        Set(ByVal value As String)
            Current.Session(COGNUTENkey) = value
        End Set
    End Property

    Public Shared Property NOMEUTEN() As String
        Get
            Return CStr(Current.Session(NOMEUTENkey))
        End Get
        Set(ByVal value As String)
            Current.Session(NOMEUTENkey) = value
        End Set
    End Property

    Public Shared Property USERNAME() As String
        Get
            Return CStr(Current.Session(USERNAMEKey))
        End Get
        Set(ByVal value As String)
            Current.Session(USERNAMEKey) = value
        End Set
    End Property

    Public Shared Property EMAIL_UT() As String
        Get
            Return CStr(Current.Session(EMAIL_UTkey))
        End Get
        Set(ByVal value As String)
            Current.Session(EMAIL_UTkey) = value
        End Set
    End Property

    Public Shared Property RAGSOCIA() As String
        Get
            Return CStr(Current.Session(RAGSOCIAkey))
        End Get
        Set(ByVal value As String)
            Current.Session(RAGSOCIAkey) = value
        End Set
    End Property

    Public Shared Property USERMENU() As XmlDocument
        Get
            Return CType(Current.Session(USERMENUkey), XmlDocument)
        End Get
        Set(ByVal value As XmlDocument)
            Current.Session(USERMENUkey) = value
        End Set
    End Property

    Public Shared Property USERMENUOriginal() As XmlDocument
        Get
            Return CType(Current.Session(USERMENUOriginalkey), XmlDocument)
        End Get
        Set(ByVal value As XmlDocument)
            Current.Session(USERMENUOriginalkey) = value
        End Set
    End Property

    Public Shared ReadOnly Property SGLAPPLI() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(SGLAPPLIkey)
        End Get
    End Property

    Public Shared ReadOnly Property DESAPPLI() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(DESAPPLIkey)
        End Get
    End Property

    Public Shared ReadOnly Property SmtpServer() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(SmtpServerkey)
        End Get
    End Property

    Public Shared ReadOnly Property ErrorReportMailFrom() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(ErrorReportMailFromkey)
        End Get
    End Property

    Public Shared ReadOnly Property ErrorReportMailTo() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(ErrorReportMailTokey)
        End Get
    End Property

    Public Shared ReadOnly Property ErrorReportMailSubject() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(ErrorReportMailSubjectkey)
        End Get
    End Property

    Public Shared ReadOnly Property BinariesBasePath() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings(BinariesBasePathkey)
        End Get
    End Property


    Public Shared Property USERFUNC() As Dictionary(Of String, SiteTailorFunctionAuthorization)
        Get
            Return CType(Current.Session(USERFUNCkey), Dictionary(Of String, SiteTailorFunctionAuthorization))
        End Get
        Set(ByVal value As Dictionary(Of String, SiteTailorFunctionAuthorization))
            Current.Session(USERFUNCkey) = value
        End Set
    End Property

    Public Shared Function LogonUser(ByVal USERNAME As String, ByVal PASSWORD As String, ByVal ParamArray additionalPagesOpenToAllUsersRelativePathsAndTitles() As String) As Boolean
        ' Connessione al db
        Dim dbConn As SqlConnection = DbConnectionHandler.GetOpenAclDbConn
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        Dim dbRdr As SqlDataReader
        Dim xmlRdr As XmlReader
        Dim xmlDoc As XmlDocument
        Dim validUser As Boolean = False
        Dim s As String
        Dim relPath As String = ""
        Dim pTitle As String = ""
        Dim isPath As Boolean
        Dim funzKey As String

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_acl_AuthenticateUser"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = USERNAME
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = PASSWORD
        End With

        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then
            validUser = True
            ContextHandler.ID_UTENT = dbRdr.GetInt32(0)
            ContextHandler.COGNUTEN = dbRdr.GetString(1)
            ContextHandler.NOMEUTEN = dbRdr.GetString(2)
            ContextHandler.EMAIL_UT = dbRdr.GetString(3)
            ContextHandler.ID_AZIEN = dbRdr.GetInt32(7)
            ContextHandler.RAGSOCIA = dbRdr.GetString(8)
            ContextHandler.ID_UNIT = dbRdr.GetInt32(9)
            ContextHandler.DISCRIMI = dbRdr.GetString(10)
            ContextHandler.AC_UNIT = dbRdr.GetString(11)
            ContextHandler.TX_UNIT = dbRdr.GetString(12)
            ContextHandler.USERNAME = USERNAME
        End If
        dbRdr.Close()
        dbCmd = Nothing
        '@id_utent int,
        '@sglappli nvarchar(50)
        'se ho trovato l'utente costruisco il menù
        If validUser Then
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_acl_BuildMenu_xml"
                .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
                .Parameters.Add("@sglappli", SqlDbType.NVarChar, 50).Value = SGLAPPLI
            End With
            xmlRdr = dbCmd.ExecuteXmlReader
            xmlDoc = New XmlDocument()
            xmlDoc.Load(xmlRdr)
            xmlRdr.Close()
            ContextHandler.USERMENUOriginal = xmlDoc
            ContextHandler.USERMENU = CType(xmlDoc.Clone, XmlDocument)

            'generazione delle autorizzazioni
            Dim funzColl As New Dictionary(Of String, SiteTailorFunctionAuthorization)

            'inserisco nella stringcollection tutte le funzioni
            'per le queli è garantito l'accesso
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ac_UserAuthorizations"
                .Parameters.Add("@id_utent", SqlDbType.Int).Value = ID_UTENT
                .Parameters.Add("@sglappli", SqlDbType.NVarChar, 50).Value = SGLAPPLI
            End With

            dbRdr = dbCmd.ExecuteReader
            Do While dbRdr.Read

                Dim fAccessLevel As Integer
                Dim fDescription As String
                Dim trPos As Integer

                fAccessLevel = dbRdr.GetInt32(2)
                fDescription = dbRdr.GetString(3)
                trPos = InStr(fDescription, " - ")
                If trPos > 0 Then fDescription = Mid(fDescription, trPos + 3)
                'tutte writabili
                funzColl.Add(Current.Server.MapPath("~/" & dbRdr.GetString(1)).ToLower, New ACL.SiteTailorFunctionAuthorization(fDescription, fAccessLevel, "~/" & dbRdr.GetString(1).ToLower, True, dbRdr.GetString(4), dbRdr.GetBoolean(5)))
            Loop
            dbRdr.Close()
            dbCmd.Dispose()

            'inserisco le pagine definite dall'array (titolo, nome, titolo, nome, etc) - se non sono già state inserite
            isPath = True
            For Each s In additionalPagesOpenToAllUsersRelativePathsAndTitles
                If isPath Then
                    relPath = s
                    isPath = False
                Else
                    pTitle = s
                    funzKey = Current.Server.MapPath(relPath).ToLower
                    If Not funzColl.ContainsKey(funzKey) Then
                        funzColl.Add(funzKey, New ACL.SiteTailorFunctionAuthorization(pTitle, 0, relPath, True, "", False))
                    End If
                    isPath = True
                End If
            Next

            ContextHandler.USERFUNC = funzColl

            'Gestione funzioni e autorizzazioni dal punto di vista dei customcontexthandlers
            For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
                iCustomContextHandler.ProcessFunctionsAndMenus(dbConn, ContextHandler.USERFUNC, ContextHandler.USERMENUOriginal, ContextHandler.USERMENU)
            Next

            dbCmd = Nothing
        End If


        dbConn.Close()

        Return validUser
    End Function

    Public Shared Function RequestIPAddress() As String
        If System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") Is Nothing Then
            Return ""
        Else
            Return System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        End If
    End Function
End Class
