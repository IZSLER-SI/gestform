Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web
Imports Softailor.Global.SqlUtils
Imports System.IO

Public Class BinarySaver

    Private _formatoBaseAccettato As FormatoBaseAccettato
    Private _ID_AZIEN As Integer

    'dati specifici dell'oggetto (tbl db)
    Private _DESEL_TX As String
    Private _DESEL_FS As String
    Private _ELE_WIDT As Integer
    Private _ELE_HEIG As Integer
    Private _URL_EXTE As String
    Private _filePostedFile As HttpPostedFile
    Private _thumbPostedFile As HttpPostedFile
    Private _fileName As String
    Private _fileExtension As String
    Private _thumbName As String
    Private _thumbExtension As String
    Private _binBasePath As String

    Public Sub New(ByVal ID_AZIEN As Integer, ByVal formatoBaseAccettato As FormatoBaseAccettato, ByVal binBasePath As String)

        _ID_AZIEN = ID_AZIEN
        _formatoBaseAccettato = formatoBaseAccettato
        _binBasePath = binBasePath
        _DESEL_TX = ""
        _DESEL_FS = ""
        _ELE_WIDT = 0
        _ELE_HEIG = 0
        _URL_EXTE = ""
        _filePostedFile = Nothing
        _thumbPostedFile = Nothing
        _fileName = ""
        _thumbName = ""

    End Sub

    Public Property DESEL_TX() As String
        Get
            Return _DESEL_TX
        End Get
        Set(ByVal value As String)
            _DESEL_TX = value
            _DESEL_FS = BinariesHelpers.CalcoloDESEL_FS(_DESEL_TX)
        End Set
    End Property

    Public ReadOnly Property DESEL_FS() As String
        Get
            Return _DESEL_FS
        End Get
    End Property

    Public Property ELE_WIDT() As Integer
        Get
            Return _ELE_WIDT
        End Get
        Set(ByVal value As Integer)
            _ELE_WIDT = value
        End Set
    End Property

    Public Property ELE_HEIG() As Integer
        Get
            Return _ELE_HEIG
        End Get
        Set(ByVal value As Integer)
            _ELE_HEIG = value
        End Set
    End Property

    Public Property URL_EXTE() As String
        Get
            Return _URL_EXTE
        End Get
        Set(ByVal value As String)
            _URL_EXTE = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
            If _fileName = "" Then
                _fileExtension = ""
            Else
                _fileExtension = IO.Path.GetExtension(_fileName)
            End If
        End Set
    End Property

    Public Property ThumbName() As String
        Get
            Return _thumbName
        End Get
        Set(ByVal value As String)
            _thumbName = value
            If _thumbName = "" Then
                _thumbExtension = ""
            Else
                _thumbExtension = IO.Path.GetExtension(_thumbName).ToLower
            End If
        End Set
    End Property

    Public Property FilePostedFile() As HttpPostedFile
        Get
            Return _filePostedFile
        End Get
        Set(ByVal value As HttpPostedFile)
            _filePostedFile = value
        End Set
    End Property

    Public Property ThumbPostedFile() As HttpPostedFile
        Get
            Return _thumbPostedFile
        End Get
        Set(ByVal value As HttpPostedFile)
            _thumbPostedFile = value
        End Set
    End Property

    Public Enum SaveResults
        OK
        DESEL_Duplicate
        UnexpectedError
    End Enum

    Public Class SaveResult
        Public result As SaveResults = SaveResults.OK
        Public ID_ELEME As Integer = 0
        Public exceptionText As String = ""
    End Class

    Public Function SaveToDbAndFS(ByVal dbConn As SqlConnection, ByVal ID_UTENT As Integer, ByVal DATA_INS As Date) As SaveResult

        Dim imgFile As Bitmap = Nothing
        Dim imgThumb As Bitmap = Nothing
        Dim dbCmd As SqlCommand
        Dim prmID_ELEME As SqlParameter

        Dim myCODFORMA As String = ""
        Dim myEXTE_GET As String = ""
        Dim myELE_WIDT As Integer = 0
        Dim myELE_HEIG As Integer = 0
        Dim myTHUMBO_P As Boolean = False
        Dim myTHUMBO_E As String = ""
        Dim myTHUMB1_G As Boolean = False
        Dim myTHUMB1_E As String = ""
        Dim myTHUMB2_G As Boolean = False
        Dim myTHUMB2_E As String = ""
        Dim myTHUMB3_G As Boolean = False
        Dim myTHUMB3_E As String = ""
        Dim myTHUMB4_G As Boolean = False
        Dim myTHUMB4_E As String = ""
        Dim myTHUMB5_G As Boolean = False
        Dim myTHUMB5_E As String = ""

        Dim ID_ELEME As Integer = 0

        Dim basePath As String
        Dim baseName As String
        Dim BINfileName As String
        Dim TORfileName As String

        Dim result As New SaveResult

        'salvataggio vero e proprio

        'innanzitutto, per poter popolare i dati del DB, apro le immagini 
        '(se ce ne sono)
        If _fileName <> "" And _formatoBaseAccettato.GESTHUMB = ModalitaGestioneAnteprima.Auto Then
            imgFile = New Bitmap(_filePostedFile.InputStream)
        End If
        If _thumbName <> "" Then
            imgThumb = New Bitmap(_thumbPostedFile.InputStream)
        End If

        'ricavo i dati che vanno "calcolati"
        'CODFORMA: cerco in EXTE_PUTs
        If _formatoBaseAccettato.IS_LOCAL Then
            myCODFORMA = _formatoBaseAccettato.getCODFORMA(_fileExtension)
        Else
            'i formati non locali gestiscono UN SOLO CODFORMA
            myCODFORMA = _formatoBaseAccettato.FormatiAccettati.Keys(0)
        End If
        myEXTE_GET = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).EXTE_GET

        'ELE_WIDT e ELE_HEIG: se ho immagine le ricavo
        If _formatoBaseAccettato.GESTHUMB = ModalitaGestioneAnteprima.Auto Then
            myELE_WIDT = imgFile.Width
            myELE_HEIG = imgFile.Height
        Else
            myELE_WIDT = _ELE_WIDT
            myELE_HEIG = _ELE_HEIG
        End If

        'thumbnail originale presente
        If _formatoBaseAccettato.GESTHUMB = ModalitaGestioneAnteprima.Optional And _thumbName <> "" Then
            myTHUMBO_P = True
            'estensione del thumbnail originale
            myTHUMBO_E = ThumbnailHelpers.GetEXTE_GET(dbConn, _thumbExtension)
        End If

        'presenza thumbnails 12345
        With _formatoBaseAccettato
            If .FormatiGenerazioneThumbnail.ContainsKey(1) Then
                myTHUMB1_G = True
                myTHUMB1_E = "." & System.Enum.GetName(GetType(FormatiImmagine), .FormatiGenerazioneThumbnail(1).FormatoImmagine).ToLower
            End If
            If .FormatiGenerazioneThumbnail.ContainsKey(2) Then
                myTHUMB2_G = True
                myTHUMB2_E = "." & System.Enum.GetName(GetType(FormatiImmagine), .FormatiGenerazioneThumbnail(2).FormatoImmagine).ToLower
            End If
            If .FormatiGenerazioneThumbnail.ContainsKey(3) Then
                myTHUMB3_G = True
                myTHUMB3_E = "." & System.Enum.GetName(GetType(FormatiImmagine), .FormatiGenerazioneThumbnail(3).FormatoImmagine).ToLower
            End If
            If .FormatiGenerazioneThumbnail.ContainsKey(4) Then
                myTHUMB4_G = True
                myTHUMB4_E = "." & System.Enum.GetName(GetType(FormatiImmagine), .FormatiGenerazioneThumbnail(4).FormatoImmagine).ToLower
            End If
            If .FormatiGenerazioneThumbnail.ContainsKey(5) Then
                myTHUMB5_G = True
                myTHUMB5_E = "." & System.Enum.GetName(GetType(FormatiImmagine), .FormatiGenerazioneThumbnail(5).FormatoImmagine).ToLower
            End If
        End With

        'OK ora posso provare a creare l'elemento nel DB
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bd_addELEMEN"
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = _ID_AZIEN
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = _formatoBaseAccettato.CODCATEG
            .Parameters.Add("@codforma", SqlDbType.NVarChar, 16).Value = myCODFORMA
            .Parameters.Add("@desel_tx", SqlDbType.NVarChar, 200).Value = _DESEL_TX
            .Parameters.Add("@desel_fs", SqlDbType.NVarChar, 200).Value = _DESEL_FS
            .Parameters.Add("@ele_widt", SqlDbType.Int).Value = ZeroToDbNull(myELE_WIDT)
            .Parameters.Add("@ele_heig", SqlDbType.Int).Value = ZeroToDbNull(myELE_HEIG)
            .Parameters.Add("@url_exte", SqlDbType.NVarChar, 256).Value = EmptyToDbNull(_URL_EXTE)
            .Parameters.Add("@thumbo_p", SqlDbType.Bit).Value = myTHUMBO_P
            .Parameters.Add("@thumbo_e", SqlDbType.NVarChar, 4).Value = EmptyToDbNull(myTHUMBO_E)
            .Parameters.Add("@thumb1_g", SqlDbType.Bit).Value = myTHUMB1_G
            .Parameters.Add("@thumb1_e", SqlDbType.NVarChar, 4).Value = EmptyToDbNull(myTHUMB1_E)
            .Parameters.Add("@thumb2_g", SqlDbType.Bit).Value = myTHUMB2_G
            .Parameters.Add("@thumb2_e", SqlDbType.NVarChar, 4).Value = EmptyToDbNull(myTHUMB2_E)
            .Parameters.Add("@thumb3_g", SqlDbType.Bit).Value = myTHUMB3_G
            .Parameters.Add("@thumb3_e", SqlDbType.NVarChar, 4).Value = EmptyToDbNull(myTHUMB3_E)
            .Parameters.Add("@thumb4_g", SqlDbType.Bit).Value = myTHUMB4_G
            .Parameters.Add("@thumb4_e", SqlDbType.NVarChar, 4).Value = EmptyToDbNull(myTHUMB4_E)
            .Parameters.Add("@thumb5_g", SqlDbType.Bit).Value = myTHUMB5_G
            .Parameters.Add("@thumb5_e", SqlDbType.NVarChar, 4).Value = EmptyToDbNull(myTHUMB5_E)

            .Parameters.Add("@data_ins", SqlDbType.DateTime).Value = DATA_INS
            .Parameters.Add("@user_ins", SqlDbType.Int).Value = ID_UTENT
            prmID_ELEME = .Parameters.Add("@id_eleme", SqlDbType.Int)
            prmID_ELEME.Direction = ParameterDirection.Output
        End With

        Try
            'provo a inserire il dato nel DB
            dbCmd.ExecuteNonQuery()

            'verifica doppione
            If TypeOf prmID_ELEME.Value Is DBNull Then
                dbCmd.Dispose()
                If Not imgFile Is Nothing Then imgFile.Dispose()
                If Not imgThumb Is Nothing Then imgThumb.Dispose()
                With result
                    .result = SaveResults.DESEL_Duplicate
                    .ID_ELEME = 0
                    .exceptionText = ""
                End With
                Return result
            End If
            ID_ELEME = CInt(prmID_ELEME.Value)
            dbCmd.Dispose()
        Catch ex As Exception
            'se siamo arrivati qui, ci sono problemi nel salvataggio nel DB (unexpected)
            'disposiamo il comando e le immagini e usciamo.
            dbCmd.Dispose()
            If Not imgFile Is Nothing Then imgFile.Dispose()
            If Not imgThumb Is Nothing Then imgThumb.Dispose()
            With result
                .result = SaveResults.UnexpectedError
                .ID_ELEME = 0
                .exceptionText = ex.ToString
            End With
            Return result
        End Try


        'OK. A questo punto il DB è a posto.

        'generazione thumbnail e salvataggio dati: tutto in un "try-catch"
        Try

            'calcolo del percorso base: root\AZI_x\CODCATEG\
            basePath = BinariesHelpers.PercorsoFisicoAziendaCategoria(_binBasePath, _ID_AZIEN, _formatoBaseAccettato.CODCATEG)

            'creo il percorso se non c'è
            If Not IO.Directory.Exists(basePath) Then IO.Directory.CreateDirectory(basePath)

            'calcolo del nome file base
            baseName = DESEL_FS & "_" & ID_ELEME.ToString & "_"

            'salvataggio file originale (se c'è)
            If FileName <> "" Then
                BINfileName = baseName & "BIN" & myEXTE_GET
                FilePostedFile.SaveAs(Path.Combine(basePath, BINfileName))
            End If

            'salvataggio del thumbnail originale (se c'è)
            If ThumbName <> "" Then
                TORfileName = baseName & "TOR" & myTHUMBO_E
                ThumbPostedFile.SaveAs(Path.Combine(basePath, TORfileName))
            End If

            'generazione thumbnails e loro salvataggio

            'ottengo l'origine del thumbnail
            Dim thumbOrig1 As Bitmap = Nothing
            Dim thumbOrig2 As Bitmap = Nothing
            Dim useDefaultThumb As Boolean = False
            Dim thumb As Bitmap
            Dim destFile As String
            Dim srcFile As String
            Dim defaultSrcPath As String

            If myTHUMBO_P Then
                'esiste thumb origin > uso quello
                thumbOrig1 = imgThumb
            Else
                'non esiste thumb orig: o è un'immagine, oppure devo usare quello di default
                If _formatoBaseAccettato.GESTHUMB = ModalitaGestioneAnteprima.Auto Then
                    'è un'immagine
                    thumbOrig1 = imgFile
                Else
                    'non è un immagine > default
                    useDefaultThumb = True
                End If
            End If

            'percorso sorgenti default
            defaultSrcPath = Path.Combine(_binBasePath, ThumbnailDefaults.DefaultThumbnailsFolder)

            'salvataggio thumbnail backoffice
            If useDefaultThumb Then
                'prendo quello del backoffice
                srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                srcFile = Path.GetFileNameWithoutExtension(srcFile) & "_bo" & Path.GetExtension(srcFile)
                If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
                thumbOrig2 = New Bitmap(Path.Combine(defaultSrcPath, srcFile))
            Else
                thumbOrig2 = thumbOrig1
            End If
            destFile = Path.Combine(basePath, baseName & "TDF" & ThumbnailDefaults.EstensioneThumbnailBackOffice)
            'Thumb BO: lo faccio trasparente
            thumb = ThumbnailHelpers.CreateThumbnail(thumbOrig2, ThumbnailDefaults.LarghezzaThumbnailBackOffice, ThumbnailDefaults.AltezzaThumbnailBackOffice, True)
            ThumbnailHelpers.SaveAndCompressThumbnail(thumb, destFile, ThumbnailDefaults.FormatoThumbnailBackOffice, ThumbnailDefaults.QualitaJpegThumbnailBackOffice)

            'salvataggio thumbnail 1
            If _formatoBaseAccettato.FormatiGenerazioneThumbnail.ContainsKey(1) Then
                With _formatoBaseAccettato.FormatiGenerazioneThumbnail(1)
                    If useDefaultThumb Then
                        If .Altezza = ThumbnailDefaults.AltezzaThumbnailBackOffice And .Larghezza = ThumbnailDefaults.LarghezzaThumbnailBackOffice Then
                            'prendo quello del backoffice
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                            srcFile = Path.GetFileNameWithoutExtension(srcFile) & "_bo" & Path.GetExtension(srcFile)
                        Else
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                        End If
                        If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
                        thumbOrig2 = New Bitmap(Path.Combine(defaultSrcPath, srcFile))
                    Else
                        thumbOrig2 = thumbOrig1
                    End If
                    destFile = Path.Combine(basePath, baseName & "T01" & myTHUMB1_E)
                    thumb = ThumbnailHelpers.CreateThumbnail(thumbOrig2, .Larghezza, .Altezza, False)
                    ThumbnailHelpers.SaveAndCompressThumbnail(thumb, destFile, .FormatoImmagine, .QualitaJpeg)
                End With
            End If
            'salvataggio thumbnail 2
            If _formatoBaseAccettato.FormatiGenerazioneThumbnail.ContainsKey(2) Then
                With _formatoBaseAccettato.FormatiGenerazioneThumbnail(2)
                    If useDefaultThumb Then
                        If .Altezza = ThumbnailDefaults.AltezzaThumbnailBackOffice And .Larghezza = ThumbnailDefaults.LarghezzaThumbnailBackOffice Then
                            'prendo quello del backoffice
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                            srcFile = Path.GetFileNameWithoutExtension(srcFile) & "_bo" & Path.GetExtension(srcFile)
                        Else
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                        End If
                        If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
                        thumbOrig2 = New Bitmap(Path.Combine(defaultSrcPath, srcFile))
                    Else
                        thumbOrig2 = thumbOrig1
                    End If
                    destFile = Path.Combine(basePath, baseName & "T02" & myTHUMB2_E)
                    thumb = ThumbnailHelpers.CreateThumbnail(thumbOrig2, .Larghezza, .Altezza, False)
                    ThumbnailHelpers.SaveAndCompressThumbnail(thumb, destFile, .FormatoImmagine, .QualitaJpeg)
                End With
            End If
            'salvataggio thumbnail 3
            If _formatoBaseAccettato.FormatiGenerazioneThumbnail.ContainsKey(3) Then
                With _formatoBaseAccettato.FormatiGenerazioneThumbnail(3)
                    If useDefaultThumb Then
                        If .Altezza = ThumbnailDefaults.AltezzaThumbnailBackOffice And .Larghezza = ThumbnailDefaults.LarghezzaThumbnailBackOffice Then
                            'prendo quello del backoffice
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                            srcFile = Path.GetFileNameWithoutExtension(srcFile) & "_bo" & Path.GetExtension(srcFile)
                        Else
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                        End If
                        If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
                        thumbOrig2 = New Bitmap(Path.Combine(defaultSrcPath, srcFile))
                    Else
                        thumbOrig2 = thumbOrig1
                    End If
                    destFile = Path.Combine(basePath, baseName & "T03" & myTHUMB3_E)
                    thumb = ThumbnailHelpers.CreateThumbnail(thumbOrig2, .Larghezza, .Altezza, False)
                    ThumbnailHelpers.SaveAndCompressThumbnail(thumb, destFile, .FormatoImmagine, .QualitaJpeg)
                End With
            End If
            'salvataggio thumbnail 4
            If _formatoBaseAccettato.FormatiGenerazioneThumbnail.ContainsKey(4) Then
                With _formatoBaseAccettato.FormatiGenerazioneThumbnail(4)
                    If useDefaultThumb Then
                        If .Altezza = ThumbnailDefaults.AltezzaThumbnailBackOffice And .Larghezza = ThumbnailDefaults.LarghezzaThumbnailBackOffice Then
                            'prendo quello del backoffice
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                            srcFile = Path.GetFileNameWithoutExtension(srcFile) & "_bo" & Path.GetExtension(srcFile)
                        Else
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                        End If
                        If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
                        thumbOrig2 = New Bitmap(Path.Combine(defaultSrcPath, srcFile))
                    Else
                        thumbOrig2 = thumbOrig1
                    End If
                    destFile = Path.Combine(basePath, baseName & "T04" & myTHUMB4_E)
                    thumb = ThumbnailHelpers.CreateThumbnail(thumbOrig2, .Larghezza, .Altezza, False)
                    ThumbnailHelpers.SaveAndCompressThumbnail(thumb, destFile, .FormatoImmagine, .QualitaJpeg)
                End With
            End If
            'salvataggio thumbnail 5
            If _formatoBaseAccettato.FormatiGenerazioneThumbnail.ContainsKey(5) Then
                With _formatoBaseAccettato.FormatiGenerazioneThumbnail(5)
                    If useDefaultThumb Then
                        If .Altezza = ThumbnailDefaults.AltezzaThumbnailBackOffice And .Larghezza = ThumbnailDefaults.LarghezzaThumbnailBackOffice Then
                            'prendo quello del backoffice
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                            srcFile = Path.GetFileNameWithoutExtension(srcFile) & "_bo" & Path.GetExtension(srcFile)
                        Else
                            srcFile = _formatoBaseAccettato.FormatiAccettati(myCODFORMA).DEFTHUMB
                        End If
                        If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
                        thumbOrig2 = New Bitmap(Path.Combine(defaultSrcPath, srcFile))
                    Else
                        thumbOrig2 = thumbOrig1
                    End If
                    destFile = Path.Combine(basePath, baseName & "T05" & myTHUMB5_E)
                    thumb = ThumbnailHelpers.CreateThumbnail(thumbOrig2, .Larghezza, .Altezza, False)
                    ThumbnailHelpers.SaveAndCompressThumbnail(thumb, destFile, .FormatoImmagine, .QualitaJpeg)
                End With
            End If

            'dispose variabili thumbs
            If Not thumbOrig1 Is Nothing Then thumbOrig1.Dispose()
            If Not thumbOrig2 Is Nothing Then thumbOrig2.Dispose()
            If Not thumb Is Nothing Then thumb.Dispose()

            'OK se siamo qui significa che abbiamo salvato il tutto nel DB.
            'e ritorniamo un bel "tutto OK"
            With result
                .result = SaveResults.OK
                .ID_ELEME = ID_ELEME
                .exceptionText = ""
            End With
            'chiusura immagini
            If Not imgFile Is Nothing Then imgFile.Dispose()
            If Not imgThumb Is Nothing Then imgThumb.Dispose()
            'ritorno il risultato
            Return result
        Catch ex As Exception
            'qualcosa è andato storto
            With result
                .result = SaveResults.UnexpectedError
                .ID_ELEME = 0
                .exceptionText = ex.ToString
                'cancello il dato dal DB per precauzione (anche se potrebbero esserci dei dati "spurii" nel filesystem)
                dbCmd = dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "sp_bd_delELEMEN"
                    .Parameters.Add("@id_azien", SqlDbType.Int).Value = _ID_AZIEN
                    .Parameters.Add("@id_eleme", SqlDbType.Int).Value = ID_ELEME
                    .ExecuteNonQuery()
                    .Dispose()
                End With
            End With
            'chiusura immagini
            If Not imgFile Is Nothing Then imgFile.Dispose()
            If Not imgThumb Is Nothing Then imgThumb.Dispose()
            'ritorno il risultato
            Return result
        End Try

    End Function

End Class
