Imports System.Data.SqlClient
Imports Softailor.Global.SqlUtils

'incapsula i dati di un elemento binario (bd_ELEME) e i permessi di accesso di un utente

Public Class UserBinaryElement

    'esiste nel DB
    Public Exists As Boolean = False

    'dati base elemento
    Public ID_ELEME As Integer
    Public DESEL_TX As String
    Public DESEL_FS As String
    Public ELE_WIDT As Integer
    Public ELE_HEIG As Integer
    Public URL_EXTE As String

    'dati categoria
    Public ID_CATEG As Integer
    Public CODCATEG As String
    Public DESCATEG As String

    'dati formato specifico
    Public CODFORMA As String
    Public DESFORMA As String
    Public MIMETYPE As String
    Public EXTE_GET As String

    'dati formato base
    Public CODFORBA As String
    Public DESFORBA As String
    Public IS_LOCAL As Boolean
    Public CodicePlayer As CodiciPlayer
    Public ModalitaGestioneAnteprima As ModalitaGestioneAnteprima

    'dati utenti
    Public USER_INS As Integer
    Public INS_NOM_COG As String
    Public DATA_INS As Date

    Public USER_UPD As Integer
    Public UPD_NOM_COG As String
    Public DATA_UPD As Date

    'dati autorizzazioni
    Public CAN_VIEW As Boolean
    Public CAN_INSE As Boolean
    Public CAN_UPDA As Boolean
    Public CAN_DELE As Boolean

    'dati "in output"
    Public ElementFileName As String
    Public BackofficeThumbnail As Thumbnail
    Public Thumbnail1 As Thumbnail
    Public Thumbnail2 As Thumbnail
    Public Thumbnail3 As Thumbnail
    Public Thumbnail4 As Thumbnail
    Public Thumbnail5 As Thumbnail

    Public Sub New()
        Exists = False

        'dati base elemento
        ID_ELEME = 0
        DESEL_TX = ""
        DESEL_FS = ""
        ELE_WIDT = 0
        ELE_HEIG = 0
        URL_EXTE = ""

        'dati categoria
        ID_CATEG = 0
        CODCATEG = ""
        DESCATEG = ""

        'dati formato specifico
        CODFORMA = ""
        DESFORMA = ""
        MIMETYPE = ""
        EXTE_GET = ""

        'dati formato base
        CODFORBA = ""
        DESFORBA = ""
        IS_LOCAL = False
        CodicePlayer = CodiciPlayer.Direct
        ModalitaGestioneAnteprima = Binaries.ModalitaGestioneAnteprima.Auto

        'dati utenti
        USER_INS = 0
        INS_NOM_COG = ""
        DATA_INS = Date.Now

        USER_UPD = 0
        UPD_NOM_COG = ""
        DATA_UPD = Date.Now

        'dati autorizzazioni
        CAN_VIEW = False
        CAN_INSE = False
        CAN_UPDA = False
        CAN_DELE = False

        'dati "in output"
        ElementFileName = ""
        BackofficeThumbnail = New Thumbnail

    End Sub

    Public Sub New(ByVal dbConn As SqlConnection, ByVal ID_AZIEN As Integer, ByVal ID_UTENT As Integer, ByVal ID_ELEME As Integer, ByVal binariesBasePath As String)

        Me.New()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim basePath As String
        Dim baseName As String

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bd_getELEMEN_UTENTI"
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ID_AZIEN
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ID_UTENT
            .Parameters.Add("@id_eleme", SqlDbType.Int).Value = ID_ELEME
        End With

        'provo a leggere
        dbRdr = dbCmd.ExecuteReader()
        With dbRdr
            If .Read Then
                'OK, ha letto. Allora l'elemento esiste.
                Exists = True

                'lettura dati base
                Me.ID_ELEME = .GetInt32(0)
                Me.DESEL_TX = .GetString(3)
                Me.DESEL_FS = .GetString(4)
                Me.ELE_WIDT = Nz(.GetSqlInt32(5))
                Me.ELE_HEIG = Nz(.GetSqlInt32(6))
                Me.URL_EXTE = Nz(.GetSqlString(7))

                'lettura dati categoria
                Me.ID_CATEG = .GetInt32(1)
                Me.CODCATEG = .GetString(22)
                Me.DESCATEG = .GetString(23)

                'lettura dati formato specifico
                Me.CODFORMA = .GetString(2)
                Me.DESFORMA = .GetString(24)
                Me.MIMETYPE = Nz(.GetSqlString(25))
                Me.EXTE_GET = .GetString(26)

                'lettura dati formato base
                Me.CODFORBA = .GetString(27)
                Me.DESFORBA = .GetString(28)
                Me.IS_LOCAL = .GetBoolean(29)
                Me.CodicePlayer = CType(System.Enum.Parse(GetType(CodiciPlayer), .GetString(30), True), CodiciPlayer)
                Me.ModalitaGestioneAnteprima = CType(System.Enum.Parse(GetType(ModalitaGestioneAnteprima), .GetString(35), True), ModalitaGestioneAnteprima)

                'lettura dati utenti
                USER_INS = .GetInt32(36)
                INS_NOM_COG = .GetString(37)
                DATA_INS = .GetDateTime(38)

                USER_UPD = Nz(.GetSqlInt32(39))
                UPD_NOM_COG = Nz(.GetSqlString(40))
                DATA_UPD = Nz(.GetSqlDateTime(41), Date.Now)

                'lettura autorizzazioni utente
                Me.CAN_VIEW = .GetInt32(31) <> 0
                Me.CAN_INSE = .GetInt32(32) <> 0
                Me.CAN_UPDA = .GetInt32(33) <> 0
                Me.CAN_DELE = .GetInt32(34) <> 0

                'generazione percorso base files
                basePath = BinariesHelpers.PercorsoFisicoAziendaCategoria(binariesBasePath, ID_AZIEN, CODCATEG)

                'calcolo del nome file base
                baseName = DESEL_FS & "_" & ID_ELEME.ToString & "_"

                'percorso/nome file originale
                If IS_LOCAL Then
                    Me.ElementFileName = IO.Path.Combine(basePath, baseName & "BIN" & EXTE_GET)
                End If

                'dati thumbnail backoffice (esiste sempre)
                With Me.BackofficeThumbnail
                    .FileName = IO.Path.Combine(basePath, baseName & "TDF" & ThumbnailDefaults.EstensioneThumbnailBackOffice)
                    .Width = ThumbnailDefaults.LarghezzaThumbnailBackOffice
                    .Height = ThumbnailDefaults.AltezzaThumbnailBackOffice
                    .MimeType = ThumbnailDefaults.MimeTypeThumbnailBackOffice
                End With

                'eventuali thumbnail 1-2-3-4-5
                AddThumbnailIfExists(dbRdr, basePath, baseName, 1, 10, 11, 12, 13)
                AddThumbnailIfExists(dbRdr, basePath, baseName, 2, 14, 15, 16, 17)
                AddThumbnailIfExists(dbRdr, basePath, baseName, 3, 18, 19, 20, 21)
                AddThumbnailIfExists(dbRdr, basePath, baseName, 4, 42, 43, 44, 45)
                AddThumbnailIfExists(dbRdr, basePath, baseName, 5, 46, 47, 48, 49)

            End If
        End With

        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Private Sub AddThumbnailIfExists(ByVal dbRdr As SqlDataReader, ByVal basePath As String, ByVal baseName As String, ByVal idx As Integer, ByVal iG As Integer, ByVal iE As Integer, ByVal iW As Integer, ByVal iH As Integer)
        Dim ext As String
        With dbRdr
            If .GetBoolean(iG) Then
                Dim thumbnail As New Thumbnail
                ext = .GetString(iE)
                thumbnail.FileName = IO.Path.Combine(basePath, baseName & "T0" & idx.ToString & ext)
                thumbnail.MimeType = ThumbnailDefaults.GetThumbnailMimeType(ext)
                thumbnail.Width = Nz(.GetSqlInt32(iW))
                thumbnail.Height = Nz(.GetSqlInt32(iH))
                Select Case idx
                    Case 1 : Thumbnail1 = thumbnail
                    Case 2 : Thumbnail2 = thumbnail
                    Case 3 : Thumbnail3 = thumbnail
                    Case 4 : Thumbnail4 = thumbnail
                    Case 5 : Thumbnail5 = thumbnail
                End Select

            End If
        End With
    End Sub

    Public Function HasThumbnail(ByVal index As Integer) As Boolean
        Select Case index
            Case 1 : Return Not Thumbnail1 Is Nothing
            Case 2 : Return Not Thumbnail2 Is Nothing
            Case 3 : Return Not Thumbnail3 Is Nothing
            Case 4 : Return Not Thumbnail4 Is Nothing
            Case 5 : Return Not Thumbnail5 Is Nothing
        End Select
        Return False
    End Function

    Public Function GetThumbnail(ByVal index As Integer) As Thumbnail
        Select Case index
            Case 1 : Return Thumbnail1
            Case 2 : Return Thumbnail2
            Case 3 : Return Thumbnail3
            Case 4 : Return Thumbnail4
            Case 5 : Return Thumbnail5
        End Select
        Return Nothing
    End Function
End Class
