<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="Ricerche.aspx.vb" 
    Inherits="Softailor.SiteTailorIzs.Ricerche" 
    title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updRICERC_g" runat="server" Height="300" Left="0" Top="0" Width="920px">
    <ContentTemplate>
    
        <stl:StlGridView ID="grdRICERC" runat="server" AddCommandText="" 
            AutoGenerateColumns="False" DataKeyNames="ID_RICER" DataSourceID="sdsRICERC_g" 
            EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
            Title="Ricerche"  BoundStlFormViewID="frmRICERC">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                <asp:BoundField DataField="ID_RICER" HeaderText="ID" 
                    InsertVisible="False" ReadOnly="True" SortExpression="ID_RICER" />
                <asp:BoundField DataField="NOME_RIC" HeaderText="Nome" 
                    SortExpression="NOME_RIC" />
                <asp:BoundField DataField="CAMPO_ID" HeaderText="Campo ID" 
                    SortExpression="CAMPO_ID" />
            </Columns>
        </stl:StlGridView>
        <stl:StlSqlDataSource ID="sdsRICERC_g" runat="server" 
            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
            DeleteCommand="DELETE FROM [mb_RICERC] WHERE [ID_RICER] = @ID_RICER" 
            SelectCommand="SELECT [ID_RICER], [NOME_RIC], [CAMPO_ID] FROM [mb_RICERC] ORDER BY [NOME_RIC]" >
            <DeleteParameters>
                <asp:Parameter Name="ID_RICER" Type="Int32" />
            </DeleteParameters>
        </stl:StlSqlDataSource>    
    </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRICERC_f" runat="server" Height="143px" Left="0" Top="310" Width="920px">
    <ContentTemplate>    
        <stl:StlFormView ID="frmRICERC" runat="server" DataKeyNames="ID_RICER" 
            DataSourceID="sdsRICERC_f" BoundStlGridViewID="grdRICERC">
            <EditItemTemplate>
                 <span class="flbl" style="width:80px;">Nome Ricerca</span>
                <asp:TextBox ID="NOME_RICTextBox" runat="server" 
                    Text='<%# Bind("NOME_RIC") %>' Width="140" />
                <span class="slbl" style="width:100px;">Nome Campo ID</span>
                <asp:TextBox ID="CAMPO_IDTextBox" runat="server" 
                    Text='<%# Bind("CAMPO_ID") %>' Width="140" />
                 <span class="slbl" style="width:160px;">Posponi WHERE dopo SQL base</span>                
                <asp:CheckBox ID="USEWHERECheckBox" runat="server" 
                    Checked='<%# Bind("USEWHERE") %>' />
                <span class="slbl" style="width:160px;">Mostra records se tutto vuoto</span> 
                <asp:CheckBox ID="SHOWSTRTCheckBox" runat="server" 
                    Checked='<%# Bind("SHOWSTRT") %>' />
                <br />           
                 <span class="flbl" style="width:80px;vertical-align:top;">SQL base</span>   
                <asp:TextBox ID="SQL_BASETextBox" runat="server" 
                    Text='<%# Bind("SQL_BASE") %>' TextMode="MultiLine" Height="80px" Width="527px" />
                <span class="slbl" style="width:60px;vertical-align:top;">Note</span>   
                <asp:TextBox ID="ANNOTAZITextBox" runat="server" 
                    Text='<%# Bind("ANNOTAZI") %>'  TextMode="MultiLine" Height="80px" Width="227px"/>
            </EditItemTemplate>
        </stl:StlFormView>
        <stl:StlSqlDataSource ID="sdsRICERC_f" runat="server" 
            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
            InsertCommand="INSERT INTO [mb_RICERC] ([NOME_RIC], [SQL_BASE], [SQL_EXPO], [USEWHERE], [COSAFAR3], [COSAFAR2], [COSAFAR1], [COSAFLBL], [COSAFARE], [CAMPO_ID], [COSAFAR4], [SHOWSTRT], [ANNOTAZI], [TIT_REPO]) VALUES (@NOME_RIC, @SQL_BASE, @SQL_EXPO, @USEWHERE, @COSAFAR3, @COSAFAR2, @COSAFAR1, @COSAFLBL, @COSAFARE, @CAMPO_ID, @COSAFAR4, @SHOWSTRT, @ANNOTAZI, @TIT_REPO); SELECT @ID_RICER = SCOPE_IDENTITY()" 
            SelectCommand="SELECT [ID_RICER], [NOME_RIC], [SQL_BASE], [SQL_EXPO], [USEWHERE], [COSAFAR3], [COSAFAR2], [COSAFAR1], [COSAFLBL], [COSAFARE], [CAMPO_ID], [COSAFAR4], [SHOWSTRT], [ANNOTAZI], [TIT_REPO] FROM [mb_RICERC] WHERE ([ID_RICER] = @ID_RICER)" 
            UpdateCommand="UPDATE [mb_RICERC] SET [NOME_RIC] = @NOME_RIC, [SQL_BASE] = @SQL_BASE, [SQL_EXPO] = @SQL_EXPO, [USEWHERE] = @USEWHERE, [COSAFAR3] = @COSAFAR3, [COSAFAR2] = @COSAFAR2, [COSAFAR1] = @COSAFAR1, [COSAFLBL] = @COSAFLBL, [COSAFARE] = @COSAFARE, [CAMPO_ID] = @CAMPO_ID, [COSAFAR4] = @COSAFAR4, [SHOWSTRT] = @SHOWSTRT, [ANNOTAZI] = @ANNOTAZI, [TIT_REPO] = @TIT_REPO WHERE [ID_RICER] = @ID_RICER">
            <UpdateParameters>
                <asp:Parameter Name="NOME_RIC" Type="String" />
                <asp:Parameter Name="SQL_BASE" Type="String" />
                <asp:Parameter Name="SQL_EXPO" Type="String" />
                <asp:Parameter Name="USEWHERE" Type="Boolean" />
                <asp:Parameter Name="COSAFAR3" Type="String" />
                <asp:Parameter Name="COSAFAR2" Type="String" />
                <asp:Parameter Name="COSAFAR1" Type="String" />
                <asp:Parameter Name="COSAFLBL" Type="String" />
                <asp:Parameter Name="COSAFARE" Type="String" />
                <asp:Parameter Name="CAMPO_ID" Type="String" />
                <asp:Parameter Name="COSAFAR4" Type="String" />
                <asp:Parameter Name="SHOWSTRT" Type="Boolean" />
                <asp:Parameter Name="ANNOTAZI" Type="String" />
                <asp:Parameter Name="TIT_REPO" Type="String" />
                <asp:Parameter Name="ID_RICER" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="ID_RICER" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="NOME_RIC" Type="String" />
                <asp:Parameter Name="SQL_BASE" Type="String" />
                <asp:Parameter Name="SQL_EXPO" Type="String" />
                <asp:Parameter Name="USEWHERE" Type="Boolean" />
                <asp:Parameter Name="COSAFAR3" Type="String" />
                <asp:Parameter Name="COSAFAR2" Type="String" />
                <asp:Parameter Name="COSAFAR1" Type="String" />
                <asp:Parameter Name="COSAFLBL" Type="String" />
                <asp:Parameter Name="COSAFARE" Type="String" />
                <asp:Parameter Name="CAMPO_ID" Type="String" />
                <asp:Parameter Name="COSAFAR4" Type="String" />
                <asp:Parameter Name="SHOWSTRT" Type="Boolean" />
                <asp:Parameter Name="ANNOTAZI" Type="String" />
                <asp:Parameter Name="TIT_REPO" Type="String" />
                <asp:Parameter Name="ID_RICER" Type="Int32" Direction="Output" />
            </InsertParameters>
        </stl:StlSqlDataSource>
    
    </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRITEMS_g" runat="server" Height="250" Left="0" Top="462" Width="520">
    <ContentTemplate>
        <stl:StlGridView ID="grdRITEMS" runat="server" AddCommandText="" 
            AutoGenerateColumns="False" DataKeyNames="ID_RITEM" DataSourceID="sdsRITEMS_g" 
            EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
            Title="" BoundStlFormViewID="frmRITEMS" ParentStlGridViewID="grdRICERC">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                <asp:BoundField DataField="ORD_RICE" HeaderText="Ric" 
                    SortExpression="ORD_RICE" />
                <asp:BoundField DataField="ORDINAME" HeaderText="Ord" 
                    SortExpression="ORDINAME" />
                <asp:BoundField DataField="NOMECAMP" HeaderText="Nome Campo" 
                    SortExpression="NOMECAMP" />
                <asp:BoundField DataField="LABELRIC" HeaderText="Etichetta" 
                    SortExpression="LABELRIC" />
                <asp:BoundField DataField="TIPOCTRL" HeaderText="Tipo Ctrl" 
                    SortExpression="TIPOCTRL" />
                <asp:BoundField DataField="TIPODATO" HeaderText="Tipo Dato" 
                    SortExpression="TIPODATO" />
                <asp:BoundField DataField="NUOVOFLD" HeaderText="Nuovo" 
                    SortExpression="NUOVOFLD" />
                <asp:BoundField DataField="NUOVOREQ" HeaderText="Not Null" 
                    SortExpression="NUOVOREQ" />
            </Columns>
        </stl:StlGridView>
        <stl:StlSqlDataSource ID="sdsRITEMS_g" runat="server" 
            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
            DeleteCommand="DELETE FROM [mb_RITEMS] WHERE [ID_RITEM] = @ID_RITEM" 
            SelectCommand="SELECT [ID_RITEM], [ORD_RICE], [ORDINAME], [NOMECAMP], [LABELRIC], [TIPOCTRL], [TIPODATO], [NUOVOFLD], [NUOVOREQ] FROM [mb_RITEMS] WHERE ([RICERCAA] = @ID_RICER) ORDER BY [ORD_RICE]" >
            <DeleteParameters>
                <asp:Parameter Name="ID_RITEM" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="ID_RICER" Type="Int32" />
            </SelectParameters>
        </stl:StlSqlDataSource>
    </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRITEMS_f" runat="server" Height="250" Left="530" Top="462" Width="390">
    <ContentTemplate>
        <stl:StlFormView ID="frmRITEMS" runat="server" DataSourceID="sdsRITEMS_f" 
            DataKeyNames="ID_RITEM" BoundStlGridViewID="grdRITEMS">
            <EditItemTemplate>
                 <span class="flbl" style="width:77px;">N° Ordine</span>
                <asp:TextBox ID="ORD_RICETextBox" runat="server" 
                    Text='<%# Bind("ORD_RICE") %>' Width="103px" />
                 <span class="slbl" style="width:85px;">N° Order By</span>
                <asp:TextBox ID="ORDINAMETextBox" runat="server" 
                    Text='<%# Bind("ORDINAME") %>' Width="103px" />
                <br />
                 <span class="flbl" style="width:77px;">Nome Campo</span>
                <asp:TextBox ID="NOMECAMPTextBox" runat="server" 
                    Text='<%# Bind("NOMECAMP") %>' Width="103px" />
                 <span class="slbl" style="width:85px;">Op Comparaz</span>
                <asp:TextBox ID="COMPARAZTextBox" runat="server" 
                    Text='<%# Bind("COMPARAZ") %>' Width="103px" />
                <br />
                 <span class="flbl" style="width:77px;">Tipo controllo</span>
                 <asp:DropDownList ID="TIPOCTRLDropDownList" runat="server" selectedvalue='<%# Bind("TIPOCTRL") %>' Width="107px">                 
                     <asp:ListItem></asp:ListItem>
                     <asp:ListItem>Testo</asp:ListItem>
                     <asp:ListItem>CheckBox</asp:ListItem>
                     <asp:ListItem>ComboBox</asp:ListItem>
                     <asp:ListItem>Riepilogo</asp:ListItem>
                     <asp:ListItem>NullBox</asp:ListItem>
                 </asp:DropDownList>
                 <span class="slbl" style="width:85px;">Tipo dato</span>
                 <asp:DropDownList ID="TIPODATODropDownList" runat="server" selectedvalue='<%# Bind("TIPODATO") %>' Width="107px">                 
                     <asp:ListItem></asp:ListItem>
                     <asp:ListItem>Stringa</asp:ListItem>
                     <asp:ListItem>N° intero</asp:ListItem>
                     <asp:ListItem>N° decimale</asp:ListItem>
                     <asp:ListItem>Data</asp:ListItem>
                     <asp:ListItem>Ora</asp:ListItem>
                     <asp:ListItem>Barcode</asp:ListItem>
                     <asp:ListItem>CheckBox</asp:ListItem>
                     <asp:ListItem>NullBox</asp:ListItem>
                 </asp:DropDownList>
                 <br />
                 <span class="flbl" style="width:77px;">Tipo Orig Rec</span>
                 <asp:DropDownList ID="TIPO_QRYDropDownList" runat="server" selectedvalue='<%# Bind("TIPO_QRY") %>' Width="107px">                 
                     <asp:ListItem></asp:ListItem>
                     <asp:ListItem>Query</asp:ListItem>
                     <asp:ListItem>Elenco Valori</asp:ListItem>
                 </asp:DropDownList>
                 <span class="slbl" style="width:85px;">Origine Record</span>
                <asp:TextBox ID="SQLCMBEVTextBox" runat="server" 
                    Text='<%# Bind("SQLCMBEV") %>' Width="103px" />
                <br />
                 <span class="flbl" style="width:77px;">N° Colonne</span>
                <asp:TextBox ID="NUM_COLOTextBox" runat="server" 
                    Text='<%# Bind("NUM_COLO") %>' Width="103px"  />
                 <span class="slbl" style="width:85px;">Lar Colonne (;)</span>
                <asp:TextBox ID="LARGHCOLTextBox" runat="server" 
                    Text='<%# Bind("LARGHCOL") %>' Width="103px"  />
                <br />
                 <span class="flbl" style="width:77px;">Colonna Assoc</span>
                <asp:TextBox ID="COLONASSTextBox" runat="server" 
                    Text='<%# Bind("COLONASS") %>' Width="103px"  />
                 <span class="slbl" style="width:85px;">Lungh. max</span>
                <asp:TextBox ID="MAXLUNGHTextBox" runat="server" 
                    Text='<%# Bind("MAXLUNGH") %>' Width="103px"  />
                <br />
                 <span class="flbl" style="width:77px;">Altezza cm</span>
                <asp:TextBox ID="ALTEZZAATextBox" runat="server" 
                    Text='<%# Bind("ALTEZZAA") %>' Width="103px"  />
                 <span class="slbl" style="width:85px;">Larghezza cm</span>
                <asp:TextBox ID="LARGHEZZTextBox" runat="server" 
                    Text='<%# Bind("LARGHEZZ") %>' Width="103px"  />
                <br />
                <br />
                 <span class="flbl" style="width:77px;">Etichetta</span>
                <asp:TextBox ID="LABELRICTextBox" runat="server" 
                    Text='<%# Bind("LABELRIC") %>' Width="103px"  />
                 <span class="slbl" style="width:85px;">Largh Etichetta</span>
                <asp:TextBox ID="LABELWIDTextBox" runat="server" 
                    Text='<%# Bind("LABELWID") %>' Width="103px"  />
                <br />
                <br />
                 <span class="flbl" style="width:77px;">Usa in nuovo</span>
                <asp:CheckBox ID="NUOVOFLDCheckBox" runat="server" 
                    Checked='<%# Bind("NUOVOFLD") %>' Width="103px" />
                <span class="slbl" style="width:85px;">Richiesto</span>
                <asp:CheckBox ID="NUOVOREQCheckBox" runat="server" 
                    Checked='<%# Bind("NUOVOREQ") %>' Width="103px" />
            </EditItemTemplate>
        </stl:StlFormView>
        <stl:StlSqlDataSource ID="sdsRITEMS_f" runat="server" 
            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
            InsertCommand="INSERT INTO [mb_RITEMS] ([RICERCAA], [ORD_RICE], [ORDINAME], [TIPOCTRL], [LARGHEZZ], [ALTEZZAA], [NOMECAMP], [LABELRIC],  [TIPODATO], [MAXLUNGH], [TIPO_QRY], [SQLCMBEV], [NUM_COLO], [LARGHCOL], [COLONASS], [NUOVOFLD], [NUOVOREQ], [LABELWID], [COMPARAZ]) VALUES (@ID_RICER, @ORD_RICE, @ORDINAME, @TIPOCTRL, @LARGHEZZ, @ALTEZZAA, @NOMECAMP, @LABELRIC, @TIPODATO, @MAXLUNGH, @TIPO_QRY, @SQLCMBEV, @NUM_COLO, @LARGHCOL, @COLONASS, @NUOVOFLD, @NUOVOREQ, @LABELWID, @COMPARAZ); SELECT @ID_RITEM = SCOPE_IDENTITY()" 
            SelectCommand="SELECT [ID_RITEM], [RICERCAA], [ORD_RICE], [ORDINAME], [TIPOCTRL], [LARGHEZZ], [ALTEZZAA], [NOMECAMP], [LABELRIC],  [TIPODATO], [MAXLUNGH], [TIPO_QRY], [SQLCMBEV], [NUM_COLO], [LARGHCOL], [COLONASS], [NUOVOFLD], [NUOVOREQ], [LABELWID], [COMPARAZ] FROM [mb_RITEMS] WHERE ([ID_RITEM] = @ID_RITEM)" 
            UpdateCommand="UPDATE [mb_RITEMS] SET [ORD_RICE] = @ORD_RICE, [ORDINAME] = @ORDINAME, [TIPOCTRL] = @TIPOCTRL, [LARGHEZZ] = @LARGHEZZ, [ALTEZZAA] = @ALTEZZAA, [NOMECAMP] = @NOMECAMP, [LABELRIC] = @LABELRIC, [TIPODATO] = @TIPODATO, [MAXLUNGH] = @MAXLUNGH, [TIPO_QRY] = @TIPO_QRY, [SQLCMBEV] = @SQLCMBEV, [NUM_COLO] = @NUM_COLO, [LARGHCOL] = @LARGHCOL, [COLONASS] = @COLONASS, [NUOVOFLD] = @NUOVOFLD, [NUOVOREQ] = @NUOVOREQ, [LABELWID] = @LABELWID, [COMPARAZ] = @COMPARAZ WHERE [ID_RITEM] = @ID_RITEM" >
            <UpdateParameters>
                <asp:Parameter Name="ORD_RICE" Type="Int16" />
                <asp:Parameter Name="ORDINAME" Type="Int16" />
                <asp:Parameter Name="TIPOCTRL" Type="String" />
                <asp:Parameter Name="LARGHEZZ" Type="Single" />
                <asp:Parameter Name="ALTEZZAA" Type="Single" />
                <asp:Parameter Name="NOMECAMP" Type="String" />
                <asp:Parameter Name="LABELRIC" Type="String" />
                <asp:Parameter Name="TIPODATO" Type="String" />
                <asp:Parameter Name="MAXLUNGH" Type="Int32" />
                <asp:Parameter Name="TIPO_QRY" Type="String" />
                <asp:Parameter Name="SQLCMBEV" Type="String" />
                <asp:Parameter Name="NUM_COLO" Type="Int16" />
                <asp:Parameter Name="LARGHCOL" Type="String" />
                <asp:Parameter Name="COLONASS" Type="Int16" />
                <asp:Parameter Name="NUOVOFLD" Type="Boolean" />
                <asp:Parameter Name="NUOVOREQ" Type="Boolean" />
                <asp:Parameter Name="LABELWID" Type="Single" />
                <asp:Parameter Name="COMPARAZ" Type="String" />
                <asp:Parameter Name="ID_RITEM" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="ID_RITEM" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="ID_RICER" Type="Int32" />
                <asp:Parameter Name="ORD_RICE" Type="Int16" />
                <asp:Parameter Name="ORDINAME" Type="Int16" />
                <asp:Parameter Name="TIPOCTRL" Type="String" />
                <asp:Parameter Name="LARGHEZZ" Type="Single" />
                <asp:Parameter Name="ALTEZZAA" Type="Single" />
                <asp:Parameter Name="NOMECAMP" Type="String" />
                <asp:Parameter Name="LABELRIC" Type="String" />
                <asp:Parameter Name="TIPODATO" Type="String" />
                <asp:Parameter Name="MAXLUNGH" Type="Int32" />
                <asp:Parameter Name="TIPO_QRY" Type="String" />
                <asp:Parameter Name="SQLCMBEV" Type="String" />
                <asp:Parameter Name="NUM_COLO" Type="Int16" />
                <asp:Parameter Name="LARGHCOL" Type="String" />
                <asp:Parameter Name="COLONASS" Type="Int16" />
                <asp:Parameter Name="NUOVOFLD" Type="Boolean" />
                <asp:Parameter Name="NUOVOREQ" Type="Boolean" />
                <asp:Parameter Name="LABELWID" Type="Single" />
                <asp:Parameter Name="COMPARAZ" Type="String" />
                <asp:Parameter Name="ID_RITEM" Type="Int32" Direction="Output" />
            </InsertParameters>
        </stl:StlSqlDataSource>
    </ContentTemplate>
    </stl:StlUpdatePanel>
    <div style="position:absolute;left:0px;top:722px;width:910px;height:280px;">
        <asp:LinkButton runat="server" ID="btnEsportaRicerca" Text="Esporta la ricerca selezionata" CssClass="classicA" /><br /><br />
        <asp:TextBox runat="server" ID="txtScriptSql" EnableViewState="false" TextMode="MultiLine" Width="918px" Height="260px" CssClass="txt" Font-Names="Lucida Console" Font-Size="12px" />
    </div>
</asp:Content>
