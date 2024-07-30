Imports System.Data.SqlClient

Public Class BinariesHelpers

    Public Shared Function GetCategorieAbilitate(ByVal dbConn As SqlConnection, ByVal ID_AZIEN As Integer, ByVal ID_UTENT As Integer, ByVal AbilVIEW As Boolean, ByVal AbilINSE As Boolean, ByVal AbilUPDA As Boolean, ByVal AbilDELE As Boolean) As Dictionary(Of String, String)

        'dato un ID_AZIEN e ID_UTENT, restituisce una lista di CODCATEG / DESCATEG
        'che rappresentano le categorie per le quali l'utente può:
        ' visualizzare dati se abilVIEW=true, and
        ' inserire dati se abilINSE=true, and
        ' modificare dati se abilUPDA=true, and
        ' cancellare dati se abilDELE=true

        Dim query As String
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim dOut As Dictionary(Of String, String)


        query = "SELECT CODCATEG, DESCATEG FROM vw_bd_AutorizzazioniUtentiBackOffice WHERE ID_AZIEN=@id_azien AND ID_UTENT=@id_UTENT"
        If AbilVIEW Then query &= " AND CAN_VIEW=1"
        If AbilINSE Then query &= " AND CAN_INSE=1"
        If AbilUPDA Then query &= " AND CAN_UPDA=1"
        If AbilDELE Then query &= " AND CAN_DELE=1"
        query &= " ORDER BY DESCATEG"

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = query
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ID_AZIEN
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ID_UTENT
        End With
        dbRdr = dbCmd.ExecuteReader
        dOut = New Dictionary(Of String, String)

        Do While dbRdr.Read
            dOut.Add(dbRdr.GetString(0), dbRdr.GetString(1))
        Loop

        dbRdr.Close()
        dbCmd.Dispose()

        Return dOut

    End Function

    Public Shared Function GetFormatiCategoria(ByVal dbConn As SqlConnection, ByVal ID_AZIEN As Integer, ByVal CODCATEG As String) As Dictionary(Of String, String)

        'dato un codice categoria, restituisce tutti i possibili formati

        Dim query As String
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim dOut As Dictionary(Of String, String)
        Dim lastCODFORBA As String
        Dim CODFORBA As String
        Dim DESFORBA_EXT As String
        Dim DESFORBA As String
        Dim LISTAEXT As String
        Dim IS_LOCAL As Boolean
        Dim EXTE_GET As String
        Dim isFirst As Boolean


        query = "SELECT CODFORBA, DESFORBA, EXTE_GET, IS_LOCAL FROM vw_bd_CATEGO_FORCAT_FORMAT_FORBAS " & _
        "WHERE ID_AZIEN=@id_azien AND CODCATEG=@codcateg ORDER BY NRORDINE_FORBAS, CODFORBA, NRORDINE_FORMAT"

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = query
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ID_AZIEN
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = CODCATEG
        End With
        dbRdr = dbCmd.ExecuteReader
        dOut = New Dictionary(Of String, String)

        lastCODFORBA = ""
        CODFORBA = ""
        DESFORBA = ""
        LISTAEXT = ""
        DESFORBA_EXT = ""
        IS_LOCAL = False
        EXTE_GET = ""
        isFirst = True

        Do While dbRdr.Read
            CODFORBA = dbRdr.GetString(0)

            If isFirst Then
                lastCODFORBA = CODFORBA
                isFirst = False
            End If

            'se è cambiato CODFORBA, aggiungo l'ultimo e resetto tutto
            If (lastCODFORBA <> CODFORBA) Then
                DESFORBA_EXT = DESFORBA
                If LISTAEXT <> "" Then
                    LISTAEXT = Left(LISTAEXT, Len(LISTAEXT) - 1)
                    DESFORBA_EXT &= " (" & LISTAEXT & ")"
                End If
                dOut.Add(lastCODFORBA, DESFORBA_EXT)
                lastCODFORBA = CODFORBA
                DESFORBA = ""
                LISTAEXT = ""
                DESFORBA_EXT = ""
                IS_LOCAL = False
                EXTE_GET = ""
            End If

            'leggo i dati di questo record
            DESFORBA = dbRdr.GetString(1)
            IS_LOCAL = dbRdr.GetBoolean(3)
            EXTE_GET = dbRdr.GetString(2)
            If IS_LOCAL Then LISTAEXT &= EXTE_GET & ";"
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        'aggiungo l'ultimo elemento (se ce ne è almeno uno)
        If CODFORBA <> "" Then
            DESFORBA_EXT = DESFORBA
            If LISTAEXT <> "" Then
                LISTAEXT = Left(LISTAEXT, Len(LISTAEXT) - 1)
                DESFORBA_EXT &= " (" & LISTAEXT & ")"
            End If
            dOut.Add(lastCODFORBA, DESFORBA_EXT)
        End If

        Return dOut

    End Function

    Public Shared Function VerificaDESEL_TX(ByVal dbConn As SqlConnection, ByVal ID_AZIEN As Integer, ByVal CODCATEG As String, ByVal DESEL_TX As String) As VerificaDESEL_TXResult

        'deve ricevere stringhe già trimmate

        Dim result As New VerificaDESEL_TXResult
        Dim dbCmd As SqlCommand
        Dim prmOut As SqlParameter

        result.DESEL_FS = CalcoloDESEL_FS(DESEL_TX)

        'verifica 1: stringa non nulla
        If DESEL_TX = "" Then
            result.Result = VerificaDESEL_TXResult.Results.Empty
            Return result
        End If

        'verifica 2: esistenza nel DB
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bd_VerificaEsistenzaELEMEN"
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ID_AZIEN
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = CODCATEG
            .Parameters.Add("@desel_tx", SqlDbType.NVarChar, 200).Value = DESEL_TX
            .Parameters.Add("@desel_fs", SqlDbType.NVarChar, 200).Value = result.DESEL_FS
            prmOut = .Parameters.Add("@id_eleme", SqlDbType.Int)
            prmOut.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        If TypeOf (prmOut.Value) Is System.DBNull Then
            result.Result = VerificaDESEL_TXResult.Results.OK
        Else
            result.Result = VerificaDESEL_TXResult.Results.AlreadyExisting
        End If

        Return result

    End Function

    Public Shared Function CalcoloDESEL_FS(ByVal DESEL_TX As String) As String

        Dim c As String, i As Integer
        Dim DESEL_FS As String = ""

        For i = 1 To Len(DESEL_TX)
            c = Mid(DESEL_TX, i, 1)
            If InStr("abcdefghijklmnopqrstuvwxyz", c.ToLower, CompareMethod.Binary) > 0 Then
                DESEL_FS &= c
            ElseIf InStr("0123456789", c.ToLower, CompareMethod.Binary) > 0 Then
                DESEL_FS &= c
            ElseIf c = "_" Then
                DESEL_FS &= c
            ElseIf c = " " Then
                DESEL_FS &= "_"
            ElseIf c.ToLower = "à" Then
                DESEL_FS &= "a"
            ElseIf c.ToLower = "è" Or c.ToLower = "é" Then
                DESEL_FS &= "e"
            ElseIf c.ToLower = "ì" Then
                DESEL_FS &= "i"
            ElseIf c.ToLower = "ò" Then
                DESEL_FS &= "o"
            ElseIf c.ToLower = "ù" Then
                DESEL_FS &= "u"
            End If
        Next
        Return DESEL_FS
    End Function

    Public Shared Function KBtoMB(ByVal i As Integer) As String
        If i < 1024 Then
            Return i.ToString & " KB"
        Else
            Return (i / 1024).ToString("#,##0.##", Softailor.Global.Cultures.CulturaItalian) & " MB"
        End If
    End Function

    Public Class VerificaDESEL_TXResult

        Public Enum Results
            OK
            Empty
            AlreadyExisting
        End Enum

        Public Result As Results
        Public DESEL_FS As String

    End Class

    Public Shared Function PercorsoFisicoAziendaCategoria(ByVal binariesBasePath As String, ByVal ID_AZIEN As Integer, ByVal CODCATEG As String) As String

        Dim percorso As String
        percorso = IO.Path.Combine(binariesBasePath, "AZI_" & ID_AZIEN.ToString)
        percorso = IO.Path.Combine(percorso, CODCATEG)
        Return percorso

    End Function
End Class



