Public Class AttestatiPartHelper

    Public Shared Function getDatasetAttestatiPart(dbConn As SqlConnection, sql As String) As dstAttestatiEcm

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        Dim dst As New dstAttestatiEcm
        dst.EnforceConstraints = False

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = sql
        End With
        dbDad = New SqlDataAdapter(dbCmd)

        dbDad.Fill(dst.vw_gf_AttestatiECM)

        dbDad.Dispose()
        dbCmd.Dispose()

        'eventuale caricamento dei loghi - se ci sono righe
        If dst.vw_gf_AttestatiECM.Rows.Count > 0 Then

            Dim attRow As dstAttestatiEcm.vw_gf_AttestatiECMRow = CType(dst.vw_gf_AttestatiECM.Rows(0), dstAttestatiEcm.vw_gf_AttestatiECMRow)
            Dim newRow = dst.loghi.NewloghiRow

            newRow.id_EVENTO = attRow.id_EVENTO

            If Not attRow.Istx_NOMEFILELOGOORGNull Then
                newRow.LogoOrganizzatore = GetFile(attRow.tx_NOMEFILELOGOORG)
            End If
            If Not attRow.Istx_NOMEFILELOGOPRONull Then
                newRow.LogoProvider = GetFile(attRow.tx_NOMEFILELOGOPRO)
            End If
            If Not attRow.Istx_NOMEFILEFIRMANull Then
                newRow.Firma = GetFile(attRow.tx_NOMEFILEFIRMA)
            End If
            If Not attRow.Istx_NOMEFILEFIRMA2Null Then
                newRow.Firma2 = GetFile(attRow.tx_NOMEFILEFIRMA2)
            End If

            dst.loghi.Rows.Add(newRow)

        End If

        Return dst

    End Function

    Private Shared Function GetFile(baseName As String) As Byte()

        ' Recupero il file di logo su FS
        Dim basePath As String = System.Configuration.ConfigurationManager.AppSettings("sitetailor_BinariesBasePath")
        Dim filePath = IO.Path.Combine(basePath, baseName)

        Dim abLogo As Byte() = Nothing

        If IO.File.Exists(filePath) Then
            Dim fsLogo As New IO.FileStream(filePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Dim brLogo As New IO.BinaryReader(fsLogo)

            abLogo = brLogo.ReadBytes(CInt(fsLogo.Length))
            brLogo.Close()
            fsLogo.Close()
        End If

        Return abLogo

    End Function

End Class
