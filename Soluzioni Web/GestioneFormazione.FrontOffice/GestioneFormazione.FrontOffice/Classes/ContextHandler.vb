Imports System.Web.HttpContext

Public Class ContextHandler

    Private Const regionKey = "gff_region"
    Private Const id_PERSONAKey = "gff_id_persona"
    Private Const fl_FOADMINKey = "gff_fl_foadmin"
    Private Const fl_DIPENDENTEKey = "gff_fl_dipendente"
    Private Const tx_TITOLOKey = "gff_tx_titolo"
    Private Const tx_COGNOMEKey = "gff_tx_cognome"
    Private Const tx_NOMEKey = "gff_tx_nome"
    Private Const ac_MATRICOLAKey = "gff_ac_matricola"
    Private Const ac_USERNAMEKey = "gff_ac_username"
    Private Const fl_SPIDKey = "gff_fl_spid"
    Private Const tx_EMAILKey = "gff_tx_email"

    'Validazione pre-login per check completezza profilo
    Public Shared id_Persona_ControlloPreLogin As Integer = -1

    Public Enum Regions
        LoggedOut
        LoggedIn
        CompleteProfile
    End Enum

    Public Shared Property Region() As Regions
        Get
            Return CType(Current.Session(regionKey), Regions)
        End Get
        Set(value As Regions)
            Current.Session(regionKey) = value
        End Set
    End Property

    Public Shared ReadOnly Property RegionString() As String
        Get
            Return Region.ToString
        End Get
    End Property

    Public Shared ReadOnly Property id_PERSONA As Integer
        Get
            Return CInt(Current.Session(id_PERSONAKey))
        End Get
    End Property

    Public Shared ReadOnly Property fl_FOADMIN As Boolean
        Get
            Return CBool(Current.Session(fl_FOADMINKey))
        End Get
    End Property

    Public Shared ReadOnly Property fl_FOADMIN01 As String
        Get
            Return If(CBool(Current.Session(fl_FOADMINKey)), "1", "0")
        End Get
    End Property

    Public Shared ReadOnly Property fl_DIPENDENTE As Boolean
        Get
            Return CBool(Current.Session(fl_DIPENDENTEKey))
        End Get
    End Property

    Public Shared ReadOnly Property fl_DIPENDENTE01 As String
        Get
            Return If(CBool(Current.Session(fl_DIPENDENTEKey)), "1", "0")
        End Get
    End Property

    Public Shared ReadOnly Property tx_TITOLO() As String
        Get
            If Current.Session(tx_TITOLOKey) Is Nothing Then
                Return ""
            Else
                Return CStr(Current.Session(tx_TITOLOKey))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property tx_NOME() As String
        Get
            If Current.Session(tx_NOMEKey) Is Nothing Then
                Return ""
            Else
                Return CStr(Current.Session(tx_NOMEKey))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property tx_COGNOME() As String
        Get
            If Current.Session(tx_COGNOMEKey) Is Nothing Then
                Return ""
            Else
                Return CStr(Current.Session(tx_COGNOMEKey))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ac_MATRICOLA() As String
        Get
            If Current.Session(ac_MATRICOLAKey) Is Nothing Then
                Return ""
            Else
                Return CStr(Current.Session(ac_MATRICOLAKey))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ac_USERNAME() As String
        Get
            If Current.Session(ac_USERNAMEKey) Is Nothing Then
                Return ""
            Else
                Return CStr(Current.Session(ac_USERNAMEKey))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property fl_SPID() As Boolean
        Get
            Return CBool(Current.Session(fl_SPIDKey))
        End Get
    End Property

    Public Shared Property tx_EMAIL() As String
        Get
            If Current.Session(tx_EMAILKey) Is Nothing Then
                Return ""
            Else
                Return CStr(Current.Session(tx_EMAILKey))
            End If
        End Get
        Set(value As String)
            Current.Session(tx_EMAILKey) = value
        End Set
    End Property

    Public Shared Sub NewEmptySession()
        Current.Session.Clear()
        Current.Session(regionKey) = Regions.LoggedOut
    End Sub

#Region "Login"

    Public Enum TryLoginResult
        NoMatch
        DipendentePrimoLogin
        DipendenteProfiloIncompleto
        EsternoProfiloIncompleto
        Ok
    End Enum

    'Validazione pre-login per check completezza profilo
    Public Shared Function ControlloPreLogin(dbconn As SqlConnection, username As String, password As String) As Integer
        'pulizia della session
        ContextHandler.NewEmptySession()

        Dim dbCmd As SqlCommand
        Dim prm_ac_RISULTATO As SqlParameter
        Dim prm_id_PERSONA As SqlParameter
        Dim prm_fl_DIPENDENTE As SqlParameter
        Dim prm_fl_FOADMIN As SqlParameter
        Dim Result As Integer

        dbCmd = dbconn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_TryPreLogin"
            .Parameters.Add("@ac_username", SqlDbType.NVarChar, 50).Value = Left(username, 50)
            .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = Left(password, 50)
            .Parameters.Add("@ac_ipaddress", SqlDbType.NVarChar, 50).Value = ContextHandler.RequestIPAddress
            prm_ac_RISULTATO = .Parameters.Add("@ac_risultato", SqlDbType.NVarChar, 100)
            prm_ac_RISULTATO.Direction = ParameterDirection.Output
            prm_id_PERSONA = .Parameters.Add("@id_persona", SqlDbType.Int)
            prm_id_PERSONA.Direction = ParameterDirection.Output
            prm_fl_DIPENDENTE = .Parameters.Add("@fl_dipendente", SqlDbType.Bit)
            prm_fl_DIPENDENTE.Direction = ParameterDirection.Output
            prm_fl_FOADMIN = .Parameters.Add("@fl_foadmin", SqlDbType.Bit)
            prm_fl_FOADMIN.Direction = ParameterDirection.Output
        End With

        dbCmd.ExecuteNonQuery()
        'Nel caso la persona abbia sbagliato le credenziali
        If (prm_ac_RISULTATO.Value.ToString = "NoMatch") Then
            Return TryLoginResult.NoMatch
        End If
        id_Persona_ControlloPreLogin = CInt(prm_id_PERSONA.Value)
        Dim controllo_pre_login As String = CStr(prm_ac_RISULTATO.Value)
        dbCmd.Dispose()
        Select Case (controllo_pre_login)
            Case "DipendenteProfiloIncompleto"
                Result = TryLoginResult.DipendenteProfiloIncompleto
                Current.Session(fl_DIPENDENTEKey) = CBool(prm_fl_DIPENDENTE.Value)
                Return Result
            Case "EsternoProfiloIncompleto"
                Result = TryLoginResult.EsternoProfiloIncompleto
                Current.Session(fl_DIPENDENTEKey) = CBool(prm_fl_DIPENDENTE.Value)
                Return Result
            Case "Ok"
                Result = TryLoginResult.Ok
                Current.Session(fl_DIPENDENTEKey) = CBool(prm_fl_DIPENDENTE.Value)
                Return Result
        End Select
    End Function

    Public Shared Function TryLogin(dbconn As SqlConnection, username As String, password As String, Optional fl_SPID As Boolean = False) As TryLoginResult

        'pulizia della session
        ContextHandler.NewEmptySession()

        'variabili
        Dim dbCmd As SqlCommand
        Dim prm_ac_RISULTATO As SqlParameter
        Dim prm_id_PERSONA As SqlParameter
        Dim prm_fl_DIPENDENTE As SqlParameter
        Dim prm_fl_FOADMIN As SqlParameter
        Dim prm_tx_TITOLO As SqlParameter
        Dim prm_tx_NOME As SqlParameter
        Dim prm_tx_COGNOME As SqlParameter
        Dim prm_ac_MATRICOLA As SqlParameter
        Dim prm_tx_EMAIL As SqlParameter
        Dim result As TryLoginResult = TryLoginResult.NoMatch

        dbCmd = dbconn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_TryLogin"
            .Parameters.Add("@ac_username", SqlDbType.NVarChar, 50).Value = Left(username, 50)
            .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = Left(password, 50)
            .Parameters.Add("@ac_ipaddress", SqlDbType.NVarChar, 50).Value = ContextHandler.RequestIPAddress

            prm_ac_RISULTATO = .Parameters.Add("@ac_risultato", SqlDbType.NVarChar, 100)
            prm_ac_RISULTATO.Direction = ParameterDirection.Output

            prm_id_PERSONA = .Parameters.Add("@id_persona", SqlDbType.Int)
            prm_id_PERSONA.Direction = ParameterDirection.Output

            prm_fl_DIPENDENTE = .Parameters.Add("@fl_dipendente", SqlDbType.Bit)
            prm_fl_DIPENDENTE.Direction = ParameterDirection.Output

            prm_fl_FOADMIN = .Parameters.Add("@fl_foadmin", SqlDbType.Bit)
            prm_fl_FOADMIN.Direction = ParameterDirection.Output

            prm_tx_TITOLO = .Parameters.Add("@tx_titolo", SqlDbType.NVarChar, 20)
            prm_tx_TITOLO.Direction = ParameterDirection.Output

            prm_tx_NOME = .Parameters.Add("@tx_nome", SqlDbType.NVarChar, 100)
            prm_tx_NOME.Direction = ParameterDirection.Output

            prm_tx_COGNOME = .Parameters.Add("@tx_cognome", SqlDbType.NVarChar, 100)
            prm_tx_COGNOME.Direction = ParameterDirection.Output

            prm_ac_MATRICOLA = .Parameters.Add("@ac_matricola", SqlDbType.NVarChar, 16)
            prm_ac_MATRICOLA.Direction = ParameterDirection.Output

            prm_tx_EMAIL = .Parameters.Add("@tx_email", SqlDbType.NVarChar, 150)
            prm_tx_EMAIL.Direction = ParameterDirection.Output

        End With

        'esecuzione
        dbCmd.ExecuteNonQuery()

        Select Case CStr(prm_ac_RISULTATO.Value)
            Case "NoMatch"
                result = TryLoginResult.NoMatch
            Case "DipendentePrimoLogin"
                result = TryLoginResult.DipendentePrimoLogin
                'cambio stato
                Current.Session(regionKey) = Regions.CompleteProfile
                Current.Session(id_PERSONAKey) = CInt(prm_id_PERSONA.Value)
                Current.Session(fl_DIPENDENTEKey) = CBool(prm_fl_DIPENDENTE.Value)
                Current.Session(fl_FOADMINKey) = CBool(prm_fl_FOADMIN.Value)
                Current.Session(tx_TITOLOKey) = CStr(prm_tx_TITOLO.Value)
                Current.Session(tx_NOMEKey) = CStr(prm_tx_NOME.Value)
                Current.Session(tx_COGNOMEKey) = CStr(prm_tx_COGNOME.Value)
                Current.Session(ac_MATRICOLAKey) = CStr(prm_ac_MATRICOLA.Value)
                Current.Session(ac_USERNAMEKey) = CStr(username)
                Current.Session(fl_SPIDKey) = fl_SPID
                Current.Session(tx_EMAILKey) = CStr(prm_tx_EMAIL.Value)
            Case "DipendenteProfiloIncompleto"
                'nessun cambio di stato
                result = TryLoginResult.DipendenteProfiloIncompleto
            Case "EsternoProfiloIncompleto"
                result = TryLoginResult.EsternoProfiloIncompleto
                'cambio stato
                Current.Session(regionKey) = Regions.CompleteProfile
                Current.Session(id_PERSONAKey) = CInt(prm_id_PERSONA.Value)
                Current.Session(fl_DIPENDENTEKey) = CBool(prm_fl_DIPENDENTE.Value)
                Current.Session(fl_FOADMINKey) = CBool(prm_fl_FOADMIN.Value)
                Current.Session(tx_TITOLOKey) = CStr(prm_tx_TITOLO.Value)
                Current.Session(tx_NOMEKey) = CStr(prm_tx_NOME.Value)
                Current.Session(tx_COGNOMEKey) = CStr(prm_tx_COGNOME.Value)
                Current.Session(ac_MATRICOLAKey) = CStr(prm_ac_MATRICOLA.Value)
                Current.Session(ac_USERNAMEKey) = CStr(username)
                Current.Session(fl_SPIDKey) = fl_SPID
                Current.Session(tx_EMAILKey) = CStr(prm_tx_EMAIL.Value)
            Case "Ok"
                result = TryLoginResult.Ok
                'cambio stato
                Current.Session(regionKey) = Regions.LoggedIn
                Current.Session(id_PERSONAKey) = CInt(prm_id_PERSONA.Value)
                Current.Session(fl_DIPENDENTEKey) = CBool(prm_fl_DIPENDENTE.Value)
                Current.Session(fl_FOADMINKey) = CBool(prm_fl_FOADMIN.Value)
                Current.Session(tx_TITOLOKey) = CStr(prm_tx_TITOLO.Value)
                Current.Session(tx_NOMEKey) = CStr(prm_tx_NOME.Value)
                Current.Session(tx_COGNOMEKey) = CStr(prm_tx_COGNOME.Value)
                Current.Session(ac_MATRICOLAKey) = CStr(prm_ac_MATRICOLA.Value)
                Current.Session(ac_USERNAMEKey) = CStr(username)
                Current.Session(fl_SPIDKey) = fl_SPID
                Current.Session(tx_EMAILKey) = CStr(prm_tx_EMAIL.Value)
        End Select

        dbCmd.Dispose()

        Return result

    End Function

    Public Shared Function RequestIPAddress() As String
        If HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") Is Nothing Then
            Return ""
        Else
            Return HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        End If
    End Function

    Public Shared Function ProfiloECM(dbConn As SqlConnection) As Boolean

        Dim dbCmd = dbConn.CreateCommand
        Dim prm_fl_CREDITIECM As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_ProfiloECM"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            prm_fl_CREDITIECM = .Parameters.Add("@fl_CREDITIECM", SqlDbType.Bit)
            prm_fl_CREDITIECM.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        ProfiloECM = CBool(prm_fl_CREDITIECM.Value)
        dbCmd.Dispose()

    End Function

#End Region

    Public Shared ReadOnly Property NomeReportAttestatoECM() As String
        Get
            If System.Configuration.ConfigurationManager.AppSettings("GF_NomeReportAttestatoECM") Is Nothing Then
                Return "rptAttestatoECM.rpt"
            Else
                Return System.Configuration.ConfigurationManager.AppSettings("GF_NomeReportAttestatoECM")

            End If
        End Get
    End Property

End Class
