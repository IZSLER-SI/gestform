Imports System.Drawing
Imports System.Drawing.Imaging
Imports Softailor.SiteTailor.Binaries
Imports System.Runtime.InteropServices
Imports System.Data.SqlClient

Public Class ThumbnailHelpers

    Private Shared Function GetImageEncoder(ByVal formato As FormatiImmagine) As ImageCodecInfo

        Dim allCodecs As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
        Dim codecFound As ImageCodecInfo = Nothing
        For Each codec In allCodecs
            Select Case formato
                Case FormatiImmagine.jpg
                    If codec.FormatDescription.ToLower = "jpeg" Then
                        codecFound = codec
                        Exit For
                    End If
                Case FormatiImmagine.gif
                    If codec.FormatDescription.ToLower = "gif" Then
                        codecFound = codec
                        Exit For
                    End If
                Case FormatiImmagine.png
                    If codec.FormatDescription.ToLower = "png" Then
                        codecFound = codec
                        Exit For
                    End If
            End Select
        Next
        Return codecFound
    End Function

    Public Shared Function CreateThumbnail(ByVal img As Bitmap, ByVal thumbW As Integer, ByVal thumbH As Integer, Optional transparent As Boolean = True) As Bitmap

        Dim oriW As Integer, oriH As Integer    'dimensioni originali immagine
        Dim innW As Integer, innH As Integer    'dimensioni a cui scalare l'immagine
        Dim outW As Integer, outH As Integer    'dimensioni finali del thumbnail
        Dim offX As Integer, offY As Integer    'offset x/y per disegnare il thumbnail
        Dim scaX As Single, scaY As Single, sca As Single      'fattore di scala x/y

        Dim imgOut As Bitmap
        Dim grpOut As Graphics

        'lettura dimensioni originali immagine
        oriW = img.Width
        oriH = img.Height

        'calcolo delle dimensioni in uscita

        If thumbW <> 0 And thumbH <> 0 Then
            '--------------------------dimensioni fisse W/H

            'determino se devo ridurre o meno
            If oriW > thumbW Or oriH > thumbH Then
                'devo ridurre l'immagine
                scaX = CSng(thumbW) / CSng(oriW)
                scaY = CSng(thumbH) / CSng(oriH)
                'prendo il fattore di scala più piccolo
                If scaX < scaY Then sca = scaX Else sca = scaY
                'calcolo innW e innH
                innW = CInt(CSng(oriW) * sca)
                innH = CInt(CSng(oriH) * sca)
                If innW > thumbW Then innW = thumbW
                If innH > thumbH Then innH = thumbH
                'calcolo outW e outH
                outW = thumbW
                outH = thumbH
                offX = (outW - innW) \ 2
                offY = (outH - innH) \ 2
            Else
                'non devo ridurre l'immagine
                innW = oriW
                innH = oriH
                outW = thumbW
                outH = thumbH
                offX = (outW - innW) \ 2
                offY = (outH - innH) \ 2
            End If
        ElseIf thumbW <> 0 And thumbH = 0 Then
            '--------------------------dimensione fissa W
            If oriW > thumbW Then
                'devo ridurre l'immagine
                sca = CSng(thumbW) / CSng(oriW)
                'calcolo innW e innH
                innW = CInt(CSng(oriW) * sca)
                innH = CInt(CSng(oriH) * sca)
                If innW > thumbW Then innW = thumbW
                'calcolo outW e outH
                outW = thumbW
                outH = innH
                offX = (outW - innW) \ 2
                offY = 0
            Else
                'non devo ridurre l'immagine
                innW = oriW
                innH = oriH
                outW = thumbW
                outH = oriH
                offX = (outW - innW) \ 2
                offY = 0
            End If
        ElseIf thumbH <> 0 And thumbW = 0 Then
            '--------------------------dimensione fissa H
            If oriH > thumbH Then
                'devo ridurre l'immagine
                sca = CSng(thumbH) / CSng(oriH)
                'calcolo innW e innH
                innW = CInt(CSng(oriW) * sca)
                innH = CInt(CSng(oriH) * sca)
                If innH > thumbH Then innH = thumbH
                'calcolo outW e outH
                outW = innW
                outH = thumbH
                offX = 0
                offY = (outH - innH) \ 2
            Else
                'non devo ridurre l'immagine
                innW = oriW
                innH = oriH
                outW = oriW
                outH = thumbH
                offX = 0
                offY = (outH - innH) \ 2
            End If
        End If

        'VECCHIA VERSIONE
        '-------------------------------------------------------------------------------------------------------
        ''OK ora abbiamo tutto quello che ci serve

        ''creo l'immagine
        'imgOut = New Bitmap(outW, outH, PixelFormat.Format24bppRgb)
        'grpOut = Graphics.FromImage(imgOut)

        ''fondo bianco
        'grpOut.FillRectangle(Brushes.White, New Rectangle(0, 0, outW, outH))

        ''creo l'immagine scalata
        'imgThumb = img.GetThumbnailImage(innW, innH, Nothing, System.IntPtr.Zero)

        ''immagine in sovraimpressione
        'grpOut.DrawImage(imgThumb, New RectangleF(CSng(offX), CSng(offY), CSng(innW), CSng(innH)))
        'grpOut.Dispose()

        ''ritorno l'immagine
        'Return imgOut

        '---------------------------------------------------------------------------------------------------------
        'FINE VECCHIA VERSIONE

        'NUOVA VERSIONE
        '-------------------------------------------------------------------------------------------------------
        'VERSIONE PIU' EVOLUTA... MA FUNZIONERA' CON LE TRASPARENZE? lascio qui...

        'OK ora abbiamo tutto quello che ci serve

        If Not transparent Then
            'creo l'immagine
            imgOut = New Bitmap(outW, outH, PixelFormat.Format24bppRgb)
            grpOut = Graphics.FromImage(imgOut)

            'fondo bianco
            grpOut.FillRectangle(Brushes.White, New Rectangle(0, 0, outW, outH))
        Else
            'creo l'immagine
            imgOut = New Bitmap(outW, outH, PixelFormat.Format32bppArgb)
            grpOut = Graphics.FromImage(imgOut)

            'fondo bianco
            'grpOut.FillRectangle(Brushes.White, New Rectangle(0, 0, outW, outH))
        End If


        'creo l'immagine scalata
        'imgThumb = img.GetThumbnailImage(innW, innH, Nothing, System.IntPtr.Zero)

        'immagine in sovraimpressione
        '        grpOut.DrawImage(imgThumb, New RectangleF(CSng(offX), CSng(offY), CSng(innW), CSng(innH)))

        grpOut.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        grpOut.DrawImage(img, New RectangleF(CSng(offX), CSng(offY), CSng(innW), CSng(innH)))

        grpOut.Dispose()

        'ritorno l'immagine
        Return imgOut


        '---------------------------------------------------------------------------------------------------------
        'FINE NUOVA VERSIONE
    End Function

    Public Shared Sub SaveAndCompressThumbnail(ByVal imgThumb As Bitmap, ByVal destFile As String, ByVal formato As FormatiImmagine, ByVal qualitaJpeg As Integer)

        Dim encoder As ImageCodecInfo = GetImageEncoder(formato)

        Select Case formato
            Case FormatiImmagine.jpg
                'impostazione della qualità
                Dim qJpeg As Integer
                If qualitaJpeg <= 0 Or qualitaJpeg > 100 Then qJpeg = 80 Else qJpeg = CByte(qualitaJpeg)

                Dim qualityEncoder As Encoder = System.Drawing.Imaging.Encoder.Quality
                Dim quality As EncoderParameter = New EncoderParameter(qualityEncoder, qJpeg)
                Dim codecParams As New EncoderParameters(1)
                codecParams.Param(0) = quality

                imgThumb.Save(destFile, encoder, codecParams)
            Case FormatiImmagine.gif
                'quantizzo
                Dim imgQuantized As Bitmap
                Dim quantizer As New Softailor.ImageQuantization.OctreeQuantizer(255, 8)
                imgQuantized = quantizer.Quantize(imgThumb)
                imgQuantized.Save(destFile, encoder, Nothing)
                imgQuantized.Dispose()
            Case FormatiImmagine.png
                imgThumb.Save(destFile, encoder, Nothing)
        End Select

    End Sub

    Public Shared Function GetEXTE_GET(ByVal dbConn As sqlconnection, ByVal EXTE_PUT As String) As String
        Dim dbCmd As sqlcommand
        Dim dbRdr As SqlDataReader
        Dim EXTE_GET As String = ""

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT EXTE_GET FROM bd_FORMAT WHERE COALESCE(EXTE_PUT,N'.invalid') + ';' LIKE '%' + @exte_put + ';%'"
            .Parameters.Add("@exte_put", SqlDbType.NVarChar, 16).Value = EXTE_PUT
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then
            EXTE_GET = dbRdr.GetString(0)
        End If
        dbRdr.Close()
        dbCmd.Dispose()
        Return EXTE_GET
    End Function
End Class
