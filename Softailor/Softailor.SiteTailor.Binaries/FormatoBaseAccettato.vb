Imports System.Data.SqlClient
Imports Softailor.Global.SqlUtils

Public Class FormatoBaseAccettato

    Public CODCATEG As String
    Public DESCATEG As String
    Public CODFORBA As String
    Public DESFORBA As String
    Public MAX_SIZE As Integer
    Public MIN_WIDT As Integer
    Public MAX_WIDT As Integer
    Public MIN_HEIG As Integer
    Public MAX_HEIG As Integer
    Public GESTHUMB As ModalitaGestioneAnteprima
    Public IS_LOCAL As Boolean
    Public CODPLAYE As CodiciPlayer
    Public NEEDSIZE As Boolean

    Public FormatiAccettati As Dictionary(Of String, FormatoAccettato) 'key=CODFORMA
    Public FormatiGenerazioneThumbnail As Dictionary(Of Integer, FormatoGenerazioneThumbnail) 'key=1,2,3

    Public Sub New(ByVal dbConn As SqlConnection, ByVal ID_AZIEN As Integer, ByVal CODCATEG As String, ByVal CODFORBA As String)

        'creazione da DB

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim firstRow As Boolean
        Dim intMAX_SIZE As Integer
        Dim formatoAccettato As FormatoAccettato

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT * FROM vw_bd_CATEGO_FORCAT_FORMAT_FORBAS WHERE ID_AZIEN=@id_azien AND CODCATEG=@codcateg AND CODFORBA=@codforba ORDER BY NRORDINE_FORMAT"
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ID_AZIEN
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = CODCATEG
            .Parameters.Add("@codforba", SqlDbType.NVarChar, 16).Value = CODFORBA
        End With

        dbRdr = dbCmd.ExecuteReader

        firstRow = True
        intMAX_SIZE = Integer.MaxValue
        With dbRdr
            Do While .Read
                If firstRow Then
                    'dati base
                    Me.CODCATEG = .GetString(2)
                    Me.DESCATEG = .GetString(3)
                    Me.CODFORBA = .GetString(34)
                    Me.DESFORBA = .GetString(35)
                    Me.MIN_WIDT = Nz(.GetSqlInt32(17))
                    Me.MAX_WIDT = Nz(.GetSqlInt32(18))
                    Me.MIN_HEIG = Nz(.GetSqlInt32(19))
                    Me.MAX_HEIG = Nz(.GetSqlInt32(20))
                    Me.GESTHUMB = CType(System.Enum.Parse(GetType(ModalitaGestioneAnteprima), .GetString(29), True), ModalitaGestioneAnteprima)
                    Me.IS_LOCAL = .GetBoolean(31)
                    Me.CODPLAYE = CType(System.Enum.Parse(GetType(CodiciPlayer), .GetString(32), True), CodiciPlayer)
                    Me.NEEDSIZE = .GetBoolean(33)

                    'creazione collezioni
                    FormatiAccettati = New Dictionary(Of String, FormatoAccettato)
                    FormatiGenerazioneThumbnail = New Dictionary(Of Integer, FormatoGenerazioneThumbnail)

                    'creazione FormatiGenerazioneThumbnail 1-2-3-4-5
                    AddFormatoGenerazioneThumbnail(dbRdr, 1, 4, 5, 6, 7, 38)
                    AddFormatoGenerazioneThumbnail(dbRdr, 2, 8, 9, 10, 11, 39)
                    AddFormatoGenerazioneThumbnail(dbRdr, 3, 12, 13, 14, 15, 40)
                    AddFormatoGenerazioneThumbnail(dbRdr, 4, 41, 42, 43, 44, 45)
                    AddFormatoGenerazioneThumbnail(dbRdr, 5, 46, 47, 48, 49, 50)

                End If
                firstRow = False

                'dimensione massima
                If .GetInt32(24) < intMAX_SIZE Then intMAX_SIZE = .GetInt32(24)

                'dati sub-formato
                formatoAccettato = New FormatoAccettato
                With formatoAccettato
                    .CODFORMA = dbRdr.GetString(22)
                    .DESFORMA = dbRdr.GetString(25)
                    .EXTE_GET = dbRdr.GetString(27)
                    .MIMETYPE = Nz(dbRdr.GetSqlString(28))
                    .DEFTHUMB = Nz(dbRdr.GetSqlString(30))
                    .EXTE_PUT = Nz(dbRdr.GetSqlString(26))
                End With
                Me.FormatiAccettati.Add(formatoAccettato.CODFORMA, formatoAccettato)
            Loop
        End With

        'imposto MAX_SIZE che è il minimo tra tutti
        Me.MAX_SIZE = intMAX_SIZE
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Sub AddFormatoGenerazioneThumbnail(ByVal dbRdr As SqlDataReader, ByVal idx As Integer, ByVal iG As Integer, ByVal iE As Integer, ByVal iW As Integer, ByVal iH As Integer, ByVal iQ As Integer)
        If dbRdr.GetBoolean(iG) Then
            Dim fgt As New FormatoGenerazioneThumbnail
            With fgt
                Dim ext As String = dbRdr.GetString(iE)
                .FormatoImmagine = CType(System.Enum.Parse(GetType(FormatiImmagine), Mid(ext, 2), True), FormatiImmagine)
                .Larghezza = Nz(dbRdr.GetSqlInt32(iW), 0)
                .Altezza = Nz(dbRdr.GetSqlInt32(iH), 0)
                If .FormatoImmagine = FormatiImmagine.jpg Then .QualitaJpeg = Nz(dbRdr.GetSqlInt32(iQ), ThumbnailDefaults.QualitaJpegDefaultThumbnailEsterni) Else .QualitaJpeg = 0
            End With
            Me.FormatiGenerazioneThumbnail.Add(idx, fgt)
        End If
    End Sub

    Public Function EsisteLimiteDimensioniImmagine() As Boolean
        Return (MIN_WIDT > 0 Or MAX_WIDT > 0 Or MIN_HEIG > 0 Or MAX_HEIG > 0)
    End Function

    Public Function EsisteLimiteLarghezzaElemento() As Boolean
        Return (MIN_WIDT > 0 Or MAX_WIDT > 0)
    End Function

    Public Function EsisteLimiteAltezzaElemento() As Boolean
        Return (MIN_HEIG > 0 Or MAX_HEIG > 0)
    End Function

    Public Function AltezzaFissaElemento() As Integer
        'restituisce zero se non c'è altezza fissa, oppure l'altezza fissa in pixel
        If MIN_HEIG > 0 And MAX_HEIG > 0 And MIN_HEIG = MAX_HEIG Then
            Return MIN_HEIG
        Else
            Return 0
        End If
    End Function

    Public Function LarghezzaFissaElemento() As Integer
        'restituisce zero se non c'è larghezza fissa, oppure la larghezza fissa in pixel
        If MIN_WIDT > 0 And MAX_WIDT > 0 And MIN_WIDT = MAX_WIDT Then
            Return MIN_WIDT
        Else
            Return 0
        End If
    End Function

    Public Function DescrizioneLimiteDimensioniImmagine(ByVal sep As String) As String

        Dim limiteW As String = ""
        Dim limiteH As String = ""

        limiteW = DescrizioneLimiteLarghezzaElemento("Larghezza")
        limiteH = DescrizioneLimiteAltezzaElemento("Altezza")

        If limiteW = "" And limiteH = "" Then
            Return ""
        ElseIf limiteW = "" And limiteH <> "" Then
            Return limiteH
        ElseIf limiteW <> "" And limiteH = "" Then
            Return limiteW
        Else
            Return limiteW & sep & limiteH
        End If

    End Function

    Public Function DescrizioneLimiteLarghezzaElemento(ByVal intestazione As String) As String

        Dim limiteW As String = ""

        If MIN_WIDT > 0 Or MAX_WIDT > 0 Then
            If intestazione <> "" Then limiteW = intestazione & ": "
            If MIN_WIDT = MAX_WIDT Then
                limiteW &= MIN_WIDT.ToString & " pixel"
            ElseIf MIN_WIDT > 0 And MAX_WIDT > 0 Then
                limiteW &= MIN_WIDT.ToString & "-" & MAX_WIDT.ToString & " pixel"
            ElseIf MIN_WIDT > 0 And MAX_WIDT = 0 Then
                limiteW &= "minimo " & MIN_WIDT.ToString & " pixel"
            ElseIf MIN_WIDT = 0 And MAX_WIDT > 0 Then
                limiteW &= "massimo " & MAX_WIDT.ToString & " pixel"
            End If
        End If

        Return limiteW

    End Function

    Public Function DescrizioneLimiteAltezzaElemento(ByVal intestazione As String) As String

        Dim limiteH As String = ""

        If MIN_HEIG > 0 Or MAX_HEIG > 0 Then
            If intestazione <> "" Then limiteH = intestazione & ": "
            If MIN_HEIG = MAX_HEIG Then
                limiteH &= MIN_HEIG.ToString & " pixel"
            ElseIf MIN_HEIG > 0 And MAX_HEIG > 0 Then
                limiteH &= MIN_HEIG.ToString & "-" & MAX_HEIG.ToString & " pixel"
            ElseIf MIN_HEIG > 0 And MAX_HEIG = 0 Then
                limiteH &= "minimo " & MIN_HEIG.ToString & " pixel"
            ElseIf MIN_HEIG = 0 And MAX_HEIG > 0 Then
                limiteH &= "massimo " & MAX_HEIG.ToString & " pixel"
            End If
        End If

        Return limiteH

    End Function

    Public Function ContainsEXTE_PUT(ByVal ext As String) As Boolean

        Dim found As Boolean = False

        For Each fa As FormatoAccettato In FormatiAccettati.Values
            If fa.ContainsEXTE_PUT(ext.ToLower) Then
                found = True
                Exit For
            End If
        Next

        Return found
    End Function

    Public ReadOnly Property EXTE_PUT() As String
        Get
            Dim s As String = ""

            For Each fa As FormatoAccettato In FormatiAccettati.Values
                s &= fa.EXTE_PUT
            Next

            Return s
        End Get
    End Property

    Public Function getCODFORMA(ByVal ext As String) As String

        Dim CODFORMA As String = ""

        For Each fa As FormatoAccettato In FormatiAccettati.Values
            If fa.ContainsEXTE_PUT(ext.ToLower) Then
                CODFORMA = fa.CODFORMA
                Exit For
            End If
        Next

        Return CODFORMA
    End Function
End Class