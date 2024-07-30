<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="OrariEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.OrariEvento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updCALENDARIO_g" runat="server" Top="0px" Left="0px"
        Width="800px" Height="365px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCALENDARIO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataSourceID="sdsCALENDARIO_g" DeleteConfirmationQuestion="" EnableViewState="False"
                AllowInsert="True" AllowDelete="True" ItemDescriptionPlural="righe" ItemDescriptionSingular="riga"
                Title="Orari e Aule" DataKeyNames="id_CALENDARIO" BoundStlFormViewID="frmCALENDARIO">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="dt_DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="tm_INIZIO" HeaderText="Ora inizio" />
                    <asp:BoundField DataField="tm_FINE" HeaderText="Ora fine" />
                    <asp:TemplateField HeaderText="Durata (hh:mm)">
                        <ItemTemplate>
                            <%# ((CInt(Eval("ni_DURATAMINUTI")) - CInt(Eval("ni_DURATAMINUTI")) Mod 60) / 60).ToString("00") & ":" & (CInt(Eval("ni_DURATAMINUTI")) Mod 60).ToString("00")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_AULA" HeaderText="Aula" />
                    <asp:BoundField DataField="tx_NOTE" HeaderText="Note" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsCALENDARIO_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT * FROM vw_eve_CALENDARIO_grid WHERE id_EVENTO=@id_EVENTO ORDER BY dt_DATA, tm_INIZIO, id_CALENDARIO"
                DeleteCommand="DELETE FROM eve_CALENDARIO WHERE id_CALENDARIO=@id_CALENDARIO">
                <SelectParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_CALENDARIO" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:UpdatePanel ID="updTotale" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div style="position: absolute; top: 373px; left: 0px; width: 600px; font-size: 14px">
                Durata totale dell'evento (hh:mm) - contando le sessioni in parallelo:
                <b>
                    <asp:Label ID="lblDurataTotaleDisaggr" runat="server" EnableViewState="false" />
                </b>
                <br />
                Durata totale dell'evento (hh:mm) - NON contando le sessioni in parallelo:
                <b>
                    <asp:Label ID="lblDurataTotaleAggr" runat="server" EnableViewState="false" />
                </b>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <stl:StlUpdatePanel ID="updCALENDARIO_f" runat="server" Top="415px" Left="0px"
        Width="800px" Height="70px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmCALENDARIO" DataSourceID="sdsCALENDARIO_f"
                NewItemText="Nuova riga" DataKeyNames="id_CALENDARIO" BoundStlGridViewID="grdCALENDARIO">
                <EditItemTemplate>
                    <span class="flbl" style="width: 40px;">Data</span>
                    <asp:DropDownList ID="dt_DATADropDownList" runat="server"
                        SelectedValue='<%# Bind("dt_DATA","{0:dd/MM/yyyy}") %>' DataSourceID="sdsDateEvento"
                        DataTextField="tx_DATA" DataValueField="tx_DATA" Width="150px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 60px;">Ora inizio</span>
                    <asp:TextBox ID="tm_INIZIOTextBox" runat="server" Text='<%# Bind("tm_INIZIO", "{0:HH:mm:ss}")%>'
                        Width="70px" />
                    <ajaxToolkit:MaskedEditExtender ID="medINIZIO" runat="server" TargetControlID="tm_INIZIOTextBox"
                        Mask="99:99:99" MaskType="Time" />
                    <span class="slbl" style="width: 60px;">Ora fine</span>
                    <asp:TextBox ID="tm_FINETextBox" runat="server" Text='<%# Bind("tm_FINE", "{0:HH:mm:ss}")%>'
                        Width="70px" />
                    <ajaxToolkit:MaskedEditExtender ID="medFINE" runat="server" TargetControlID="tm_FINETextBox"
                        Mask="99:99:99" MaskType="Time" />
                    <span class="slbl" style="width: 40px;">Aula</span>
                    <asp:DropDownList ID="id_AULADropDownList" runat="server"
                        SelectedValue='<%# Bind("id_AULA")%>' DataSourceID="sdsAuleEvento"
                        DataTextField="tx_AULA" DataValueField="id_AULA" Width="290px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 40px;">Note</span>
                    <asp:TextBox ID="tx_NOTETextBox" runat="server" Text='<%# Bind("tx_NOTE")%>'
                        Width="744px" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsCALENDARIO_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT 
                    id_CALENDARIO,
                    id_EVENTO,
                    CAST(dt_DATA as datetime) as dt_DATA, 
                    CAST(tm_INIZIO as datetime) as tm_INIZIO, 
                    CAST(tm_FINE as datetime) as tm_FINE,
                    id_AULA,
                    tx_NOTE
                FROM eve_CALENDARIO WHERE id_CALENDARIO=@id_CALENDARIO"
                InsertCommand="INSERT INTO eve_CALENDARIO (dt_CREAZIONE, tx_CREAZIONE, id_EVENTO, dt_DATA, tm_INIZIO, tm_FINE, id_AULA, tx_NOTE)
                                                VALUES (GETDATE(), @tx_CREAZIONE, @id_EVENTO, @dt_DATA, @tm_INIZIO, @tm_FINE, @id_AULA, @tx_NOTE); SELECT @id_CALENDARIO = CAST(SCOPE_IDENTITY() as int)"
                UpdateCommand=" UPDATE   eve_CALENDARIO
                                SET      dt_MODIFICA=GETDATE(), 
                                         tx_MODIFICA=@tx_MODIFICA,
                                         dt_DATA=@dt_DATA,
                                         tm_INIZIO=@tm_INIZIO,
                                         tm_FINE=@tm_FINE,
                                         id_AULA=@id_AULA,
                                         tx_NOTE=@tx_NOTE
                                WHERE    id_CALENDARIO=@id_CALENDARIO">
                <SelectParameters>
                    <asp:Parameter Name="id_CALENDARIO" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="tx_CREAZIONE" Type="String" />
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                    <asp:Parameter Name="dt_DATA" Type="DateTime" />
                    <asp:Parameter Name="tm_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="tm_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_AULA" Type="Int32" />
                    <asp:Parameter Name="tx_NOTE" Type="String" />
                    <asp:Parameter Name="id_CALENDARIO" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="tx_MODIFICA" Type="String" />
                    <asp:Parameter Name="dt_DATA" Type="DateTime" />
                    <asp:Parameter Name="tm_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="tm_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_AULA" Type="Int32" />
                    <asp:Parameter Name="tx_NOTE" Type="String" />
                    <asp:Parameter Name="id_CALENDARIO" Type="Int32" />
                </UpdateParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:SqlDataSource ID="sdsDateEvento" runat="server" 
        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
        SelectCommandType="StoredProcedure" SelectCommand="sp_eve_DateEvento">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsAuleEvento" runat="server" 
        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
        SelectCommandType="StoredProcedure" SelectCommand="sp_eve_AuleEvento">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
