Public Class ThumbnailDefaults

    'dati di default per generazione thumbnail interni
    'e per upload immagini/generazione thumbnail aggiuntivi


    'larghezza e altezza thumbnail interno generato
    Public Shared LarghezzaThumbnailBackOffice As Integer = 70
    Public Shared AltezzaThumbnailBackOffice As Integer = 70

    'valore di default per generazione thumbnail ESTERNI jpeg
    '(viene usato se il dato non è presente nel DB)
    Public Shared QualitaJpegDefaultThumbnailEsterni As Integer = 80

    'formato di default per generazione thumbnails backoffice
    'ed eventuale rapporto compressione Jpeg
    Public Shared FormatoThumbnailBackOffice As FormatiImmagine = FormatiImmagine.png
    Public Shared EstensioneThumbnailBackOffice As String = ".png"
    Public Shared QualitaJpegThumbnailBackOffice As Integer = 80
    Public Shared MimeTypeThumbnailBackOffice As String = "image/png"

    'dimensione massima (KB) per immagine sorgente thumbnail
    'ci vuole un limite perchè queste vengono memorizzate comunque!
    Public Shared DimensioneMassimaSorgenteThumbnail As Integer = 2048
    Public Shared EstensioniAccettateSorgenteThumbnail As String = ".gif;.jpg;.jpeg;.jpe;.png;"

    'cartella di default
    Public Shared DefaultThumbnailsFolder As String = "DEFTHUMBS"

    'ricava il mimetype (solo per thumbnails) dall'estensione
    Public Shared Function GetThumbnailMimeType(ByVal extension As String) As String
        Select Case extension
            Case ".gif" : Return "image/gif"
            Case ".jpg" : Return "image/jpeg"
            Case ".png" : Return "image/png"
        End Select
        Return ""
    End Function
End Class