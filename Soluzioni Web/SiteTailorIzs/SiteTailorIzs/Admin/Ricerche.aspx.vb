Imports Softailor.Global.SqlUtils

Partial Public Class Ricerche
    Inherits System.Web.UI.Page


    Protected Sub btnEsportaRicerca_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEsportaRicerca.Click
        If grdRICERC.SelectedIndex = -1 Then
            txtScriptSql.Text = "SELEZIONA UNA RICERCA!!!"
        Else
            Dim dbConn As SqlConnection
            dbConn = DbConnectionHandler.GetOpenDataDbConn
            txtScriptSql.Text = ScriptEsportazioneRicerca(dbConn, CInt(grdRICERC.SelectedDataKey.Value.ToString))
            dbConn.Close()
        End If
    End Sub

    Private Function ScriptEsportazioneRicerca(ByVal dbConn As SqlConnection, ByVal ID_RICER As Integer) As String

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim lines As New List(Of String)
        Dim line As String

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT ID_RICER, NOME_RIC, SQL_BASE, SQL_EXPO, USEWHERE, CAMPO_ID, COSAFARE, COSAFLBL, " & _
                                  "COSAFAR1, COSAFAR2, COSAFAR3, COSAFAR4, SHOWSTRT, ANNOTAZI, TIT_REPO FROM mb_RICERC WHERE ID_RICER=@id_ricer"
            .Parameters.Add("@id_ricer", SqlDbType.Int).Value = ID_RICER
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        With dbRdr
            'commenti iniziali
            lines.Add("--Cancellazione e ri-creazione ricerca '" & .GetString(1) & "'")
            lines.Add("------------------------------------------------------------------------------------------------")
            'cancellazione
            lines.Add("DECLARE @id_ricer int")
            lines.Add("SET @id_ricer = null")
            lines.Add("SELECT @id_ricer=ID_RICER FROM mb_RICERC WHERE NOME_RIC=" & SQL_String(.GetString(1)))
            lines.Add("--Cancellazione vecchi dati")
            lines.Add("DELETE FROM mb_RITEMS WHERE RICERCAA=@id_ricer")
            lines.Add("DELETE FROM mb_RICERC WHERE ID_RICER=@id_ricer")
            lines.Add("--Creazione record in mb_RICERC")
            lines.Add("INSERT INTO mb_RICERC (NOME_RIC, SQL_BASE, SQL_EXPO, USEWHERE, CAMPO_ID, COSAFARE, COSAFLBL, COSAFAR1, COSAFAR2, COSAFAR3, COSAFAR4, SHOWSTRT, ANNOTAZI, TIT_REPO)")
            line = ""
            line &= "VALUES ("
            line &= SQL_String(.GetSqlString(1)) & ", "     'NOME_RIC
            line &= SQL_String(.GetSqlString(2)) & ", "     'SQL_BASE
            line &= SQL_String(.GetSqlString(3)) & ", "     'SQL_EXPO
            line &= SQL_Boolean(.GetSqlBoolean(4)) & ", "   'USEWHERE
            line &= SQL_String(.GetSqlString(5)) & ", "     'CAMPO_ID
            line &= SQL_String(.GetSqlString(6)) & ", "     'COSAFARE
            line &= SQL_String(.GetSqlString(7)) & ", "     'COSAFLBL
            line &= SQL_String(.GetSqlString(8)) & ", "     'COSAFAR1
            line &= SQL_String(.GetSqlString(9)) & ", "     'COSAFAR2
            line &= SQL_String(.GetSqlString(10)) & ", "    'COSAFAR3
            line &= SQL_String(.GetSqlString(11)) & ", "    'COSAFAR4
            line &= SQL_Boolean(.GetSqlBoolean(12)) & ", "  'SHOWSTRT
            line &= SQL_String(.GetSqlString(13)) & ", "    'ANNOTAZI
            line &= SQL_String(.GetSqlString(14))           'TIT_REPO
            line &= ")"
            lines.Add(line)
            lines.Add("-- Recupero del nuovo ID_RICER creato")
            lines.Add("SET @id_ricer=CAST(SCOPE_IDENTITY() as int)")

        End With
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura e inserimento righe
        lines.Add("-- Creazione delle righe in mb_RITEMS")

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            '                      0         1         2         3         4         5         6         7         8         9       
            .CommandText = "SELECT RICERCAA, ORD_RICE, ORD_VISU, ORDINAME, TIPOCTRL, X_COORDI, Y_COORDI, ALTEZZAA, LARGHEZZ, LARGHVIS, " & _
                                  "SFONDRIC, SFONDVIS, BOLDRICE, BOLDVISU, ALIGNVIS, NOMECAMP, LABELRIC, LABELVIS, TIPODATO, INPUTMSK, " & _
                                  "MAXLUNGH, FORM__IN, FORM_OUT, CIFDECIN, CIFDECOU, TIPO_QRY, SQLCMBEV, NUM_COLO, LARGHTOT, LARGHCOL, " & _
                                  "SOLOELEN, RIGHEELE, COLONASS, RIEPMULT, NUOVOFLD, NUOVOREQ, LABELWID, ALIGNRIC, COMPARAZ " & _
                           "FROM mb_RITEMS WHERE RICERCAA=@ricercaa ORDER BY ID_RITEM"
            .Parameters.Add("@ricercaa", SqlDbType.Int).Value = ID_RICER
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            lines.Add("INSERT INTO mb_RITEMS (RICERCAA, ORD_RICE, ORD_VISU, ORDINAME, TIPOCTRL, X_COORDI, Y_COORDI, ALTEZZAA, LARGHEZZ, LARGHVIS, " & _
                                             "SFONDRIC, SFONDVIS, BOLDRICE, BOLDVISU, ALIGNVIS, NOMECAMP, LABELRIC, LABELVIS, TIPODATO, INPUTMSK, " & _
                                             "MAXLUNGH, FORM__IN, FORM_OUT, CIFDECIN, CIFDECOU, TIPO_QRY, SQLCMBEV, NUM_COLO, LARGHTOT, LARGHCOL, " & _
                                             "SOLOELEN, RIGHEELE, COLONASS, RIEPMULT, NUOVOFLD, NUOVOREQ, LABELWID, ALIGNRIC, COMPARAZ)")
            With dbRdr
                line = "VALUES (@id_ricer, "
                line &= SQL_Int16(.GetSqlInt16(1)) & ", "       'ORD_RICE
                line &= SQL_Int16(.GetSqlInt16(2)) & ", "       'ORD_VISU
                line &= SQL_Int16(.GetSqlInt16(3)) & ", "       'ORDINAME
                line &= SQL_String(.GetSqlString(4)) & ", "     'TIPOCTRL
                line &= SQL_Single(.GetSqlSingle(5)) & ", "     'X_COORDI
                line &= SQL_Single(.GetSqlSingle(6)) & ", "     'Y_COORDI
                line &= SQL_Single(.GetSqlSingle(7)) & ", "     'ALTEZZAA
                line &= SQL_Single(.GetSqlSingle(8)) & ", "     'LARGHEZZ
                line &= SQL_Single(.GetSqlSingle(9)) & ", "     'LARGHVIS
                line &= SQL_Int32(.GetSqlInt32(10)) & ", "      'SFONDRIC
                line &= SQL_Int32(.GetSqlInt32(11)) & ", "      'SFONDVIS
                line &= SQL_Boolean(.GetSqlBoolean(12)) & ", "  'BOLDRICE
                line &= SQL_Boolean(.GetSqlBoolean(13)) & ", "  'BOLDVISU
                line &= SQL_String(.GetSqlString(14)) & ", "    'ALIGNVIS
                line &= SQL_String(.GetSqlString(15)) & ", "    'NOMECAMP
                line &= SQL_String(.GetSqlString(16)) & ", "    'LABELRIC
                line &= SQL_String(.GetSqlString(17)) & ", "    'LABELVIS
                line &= SQL_String(.GetSqlString(18)) & ", "    'TIPODATO
                line &= SQL_String(.GetSqlString(19)) & ", "    'INPUTMSK
                line &= SQL_Int32(.GetSqlInt32(20)) & ", "      'MAXLUNGH
                line &= SQL_String(.GetSqlString(21)) & ", "    'FORM__IN
                line &= SQL_String(.GetSqlString(22)) & ", "    'FORM_OUT
                line &= SQL_Int16(.GetSqlInt16(23)) & ", "      'CIFDECIN
                line &= SQL_Int16(.GetSqlInt16(24)) & ", "      'CIFDECOU
                line &= SQL_String(.GetSqlString(25)) & ", "    'TIPO_QRY
                line &= SQL_String(.GetSqlString(26)) & ", "    'SQLCMBEV
                line &= SQL_Int16(.GetSqlInt16(27)) & ", "      'NUM_COLO
                line &= SQL_Single(.GetSqlSingle(28)) & ", "    'LARGHTOT
                line &= SQL_String(.GetSqlString(29)) & ", "    'LARGHCOL
                line &= SQL_Boolean(.GetSqlBoolean(30)) & ", "  'SOLOELEN
                line &= SQL_Int16(.GetSqlInt16(31)) & ", "      'RIGHEELE
                line &= SQL_Int16(.GetSqlInt16(32)) & ", "      'COLONASS
                line &= SQL_Boolean(.GetSqlBoolean(33)) & ", "  'RIEPMULT
                line &= SQL_Boolean(.GetSqlBoolean(34)) & ", "  'NUOVOFLD
                line &= SQL_Boolean(.GetSqlBoolean(35)) & ", "  'NUOVOREQ
                line &= SQL_Single(.GetSqlSingle(36)) & ", "    'LABELWID
                line &= SQL_String(.GetSqlString(37)) & ", "    'ALIGNRIC
                line &= SQL_String(.GetSqlString(38))           'COMPARAZ
                line &= ")"
            End With
            lines.Add(line)
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        'restituzione dello script
        Dim sScript As String = ""
        For Each line In lines
            sScript &= line & vbCrLf
        Next

        Return sScript

    End Function
End Class